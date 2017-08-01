using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;
using Apollo.Utilities;

namespace Apollo.Data
{
    public interface IJobsDataService
    {
        Task<IReadOnlyList<Job>> GetActiveJobs();
        Task BeginExecution(Job job, string executionId);
        Task EndExecution(Job job, string executionId, CommandResult result);
        Task ExecutionError(Job job, string executionId, Exception exception);
        Task AddJob(string commandName, object parameters, Schedule schedule);
        Task CancelJob(int jobId);
        Task<IReadOnlyList<JobExecution>> GetJobHistory(int jobId);
        Task MarkJobExpired(int jobId);
        Task<IReadOnlyList<Job>> GetExpiredJobs();
    }

    public class JobsDataService : BaseDataService, IJobsDataService
    {
        public const string GetActiveJobsSql = "select * from jobs where expired_at is null";
        public const string GetExpiredJobsSql = "select * from jobs where expired_at is not null";

        public const string BeingExecutionSql = "insert into job_history(job_id, execution_id, executed_at) " +
                                                "values (@jobId, @executionId, current_timestamp)";

        public const string UpdateLastExecutedSql = "update jobs set last_executed_at=current_timestamp " +
                                                    "where id=@jobId";

        public const string EndExecutionSql = "update job_history set results=@results, " +
                                              "result_type=@resultType, " +
                                              "execution_ended=current_timestamp " +
                                              "where execution_id=@executionId";

        public const string AddJobSql = "insert into jobs (command_name, parameters, schedule, created_at) " +
                                        "values (@commandName, @parameters, @schedule, current_timestamp)";

        public const string CancelJobSql = "update jobs set expired_at=current_timestamp where id=@jobId";

        public const string GetJobHistorySql = "select * from job_history where job_id=@jobId";

        public const string ExpireJobSql = "update jobs set expired_at=current_timestamp where id=@jobId";

        private readonly IJsonSerializer serializer;

        public JobsDataService(IConnectionFactory connectionFactory, IJsonSerializer serializer) : base(connectionFactory)
        {
            this.serializer = serializer;
        }

        public async Task<IReadOnlyList<Job>> GetActiveJobs()
        {
            return await QueryAsync<Job>(GetActiveJobsSql);
        }

        public async Task BeginExecution(Job job, string executionId)
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(UpdateLastExecutedSql, new {jobId = job.id});
                    connection.Execute(BeingExecutionSql, new {jobId = job.id, executionId});
                    transaction.Commit();
                }
            }
        }

        public async Task EndExecution(Job job, string executionId, CommandResult result)
        {
            var serializedResult = serializer.Serialize(result);
            await Execute(EndExecutionSql,
                new {results = serializedResult, resultType = Enum.GetName(typeof(CommandResultType), result.ResultStatus), executionId});
        }

        public async Task ExecutionError(Job job, string executionId, Exception exception)
        {
            var serializedResult = serializer.Serialize(new {message = exception.Message});
            await Execute(EndExecutionSql,
                new {results = serializedResult, resultType = "Error", executionId});
        }

        public async Task AddJob(string commandName, object parameters, Schedule schedule)
        {
            var paramsSerialized = serializer.Serialize(parameters);
            var scheduleSerialized = serializer.Serialize(schedule);

            await Execute(AddJobSql, new {commandName, parameters = paramsSerialized, schedule = scheduleSerialized});
        }

        public async Task CancelJob(int jobId)
        {
            await Execute(CancelJobSql, new { jobId });
        }

        public async Task<IReadOnlyList<JobExecution>> GetJobHistory(int jobId)
        {
            return await QueryAsync<JobExecution>(GetJobHistorySql, new {jobId});
        }

        public async Task MarkJobExpired(int jobId)
        {
            await Execute(ExpireJobSql, new {jobId});
        }

        public async Task<IReadOnlyList<Job>> GetExpiredJobs()
        {
            return await QueryAsync<Job>(GetExpiredJobsSql);
        }
    }

    public class Job
    {
        public int id { get; set; }
        public string command_name { get; set; }
        public string parameters { get; set; }
        public string schedule { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? last_executed_at { get; set; }
        public DateTime? expired_at { get; set; }

        public string GetJobIdentifier()
        {
            return $"{command_name}:{id}";
        }
    }

    public class JobExecution
    {
        public int id { get; set; }
        public int job_id { get; set; }
        public string execution_id { get; set; }
        public DateTime executed_at { get; set; }
        public DateTime? execution_ended { get; set; }
        public string results { get; set; }
        public string result_type { get; set; }
    }
}
