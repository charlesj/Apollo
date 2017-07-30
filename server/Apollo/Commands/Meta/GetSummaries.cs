using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Meta
{
    public class GetSummaries : AuthenticatedCommand
    {
        private readonly IBookmarksDataService bookmarksDataService;
        private readonly IJournalDataService journalDataService;
        private readonly IJobsDataService jobsDataService;
        private readonly ILoginSessionDataService loginSessionDataService;
        private readonly IPersonalHealthService personalHealthService;
        private readonly ITodoItemDataService todoItemDataService;
        private readonly ITodoQueueItemDataService todoQueueItemDataService;

        public GetSummaries(
            IBookmarksDataService bookmarksDataService,
            IJournalDataService journalDataService,
            IJobsDataService jobsDataService,
            ILoginSessionDataService loginSessionDataService,
            ILoginService loginService,
            IPersonalHealthService personalHealthService,
            ITodoItemDataService todoItemDataService,
            ITodoQueueItemDataService todoQueueItemDataService) : base(loginService)
        {
            this.bookmarksDataService = bookmarksDataService;
            this.journalDataService = journalDataService;
            this.jobsDataService = jobsDataService;
            this.loginSessionDataService = loginSessionDataService;
            this.personalHealthService = personalHealthService;
            this.todoItemDataService = todoItemDataService;
            this.todoQueueItemDataService = todoQueueItemDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var values = new Dictionary<string, Func<Task<string>>>();
            values.Add("login sessions", async () => (await loginSessionDataService.GetAllActiveSessions()).Count.ToString());
            values.Add("total bookmarks", async () => (await bookmarksDataService.GetTotal()).ToString());
            values.Add("new bookmarks", async () => (await bookmarksDataService.GetRecentCount()).ToString());
            values.Add("weight change", async () => (await personalHealthService.CalculateRecentlyLostWeight()).ToString(CultureInfo.InvariantCulture));
            values.Add("total weight change", async () => (await personalHealthService.TotalWeightChange()).ToString(CultureInfo.InvariantCulture));
            values.Add("total log entries", async () => (await journalDataService.GetAllJournalEntries()).Count.ToString() );
            values.Add("new log entries", async () => (await journalDataService.GetRecentEntryCount()).ToString() );
            values.Add("to do items", async () => (await todoItemDataService.GetIncompleteItems()).Count.ToString());
            values.Add("do later items", async () => (await todoQueueItemDataService.GetIncompleteItems()).Count.ToString());
            values.Add("active jobs", async () => (await jobsDataService.GetActiveJobs()).Count.ToString());

            var counter = 0;
            var summaries = await Task.WhenAll(values.Select(async v => new {label = v.Key, amount = await v.Value(), id = ++counter}));
            return CommandResult.CreateSuccessResult(summaries);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
