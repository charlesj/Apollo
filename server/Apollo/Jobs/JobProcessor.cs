using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;
using Apollo.Utilities;

namespace Apollo.Jobs
{
    public interface IJobProcessor
    {
        Task Process(CancellationToken token);
    }

    public class JobProcessor : IJobProcessor
    {
        private readonly IClock clock;
        private readonly ICommandLocator commandLocator;
        private readonly ICommandProcessor commandProcessor;
        private readonly IConfiguration configuration;
        private readonly IJsonSerializer jsonSerializer;
        private readonly IJobsDataService jobsDataService;
        private readonly ISchedulerService schedulerService;

        public JobProcessor(
            IClock clock,
            ICommandLocator commandLocator,
            ICommandProcessor commandProcessor,
            IConfiguration configuration,
            IJsonSerializer jsonSerializer,
            IJobsDataService jobsDataService,
            ISchedulerService schedulerService)
        {
            this.clock = clock;
            this.commandLocator = commandLocator;
            this.commandProcessor = commandProcessor;
            this.configuration = configuration;
            this.jsonSerializer = jsonSerializer;
            this.jobsDataService = jobsDataService;
            this.schedulerService = schedulerService;
        }

        public async Task Process(CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            Job currentJob = null;
            while (!token.IsCancellationRequested)
            {
                stopwatch.Start();
                try
                {
                    var activeJobs = await jobsDataService.GetActiveJobs();
                    foreach (var job in activeJobs)
                    {
                        currentJob = job;
                        if (CircuitBreakers.IsBroken(job.GetJobIdentifier()))
                        {
                            Logger.Error($"{job.GetJobIdentifier()} has a broken circuit breaker");
                            continue;
                        }

                        var schedule = jsonSerializer.Deserialize<Schedule>(job.schedule);
                        if (schedule.start >= DateTime.UtcNow)
                        {
                            continue;
                        }

                        var nextEvent = schedulerService.GetNextEvent(schedule, job.last_executed_at);
                        Logger.Trace($"Next execution for {job.command_name}:{job.id} at {nextEvent}");
                        if (nextEvent != null && nextEvent < clock.UtcNow)
                        {
                            Logger.Trace($"Executing job {job.command_name}:{job.id}");
                            WrapFireEvent(job);
                        }

                        if (nextEvent == null)
                        {
                            Logger.Trace($"Marking job {job.command_name}:{job.id} as expired");
                            await jobsDataService.MarkJobExpired(job.id);
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.Error($"Error Job {currentJob?.command_name}:{currentJob?.id} Processing: {e.Message}");

                    if (currentJob != null)
                    {
                        CircuitBreakers.RecordBreak(currentJob.GetJobIdentifier());
                    }
                }

                while (stopwatch.ElapsedMilliseconds < configuration.JobProcessThrottleMs())
                {
                    await Task.Delay(100, token);
                }
                stopwatch.Reset();
            }
        }

        private void WrapFireEvent(Job job)
        {
            Task.Run(() => FireEvent(job));
        }

        public async Task FireEvent(Job job)
        {
            var executionId = Guid.NewGuid().ToNakedString();
            try
            {
                Logger.Info($"Beginning job {job.command_name}:{executionId}", job);
                await jobsDataService.BeginExecution(job, executionId);
                var command = commandLocator.Locate(job.command_name);
                var parameters = jsonSerializer.Deserialize<object>(job.parameters);
                var result = await commandProcessor.Process(command, parameters, true);
                await jobsDataService.EndExecution(job, executionId, result);
                Logger.Info($"Completed job {job.command_name}:{executionId}", new {job, result});
            }
            catch (Exception exception)
            {
                Logger.Error($"Job {job.command_name}:{executionId} Failed: {exception.Message}");
                await jobsDataService.ExecutionError(job, executionId, exception);
            }
        }
    }
}
