﻿using System;
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
        private readonly IFeedDataService feedDataService;
        private readonly IJournalDataService journalDataService;
        private readonly IJobsDataService jobsDataService;
        private readonly ILoginSessionDataService loginSessionDataService;
        private readonly INotebookDataService notebookDataService;
        private readonly IPersonalHealthService personalHealthService;
        private readonly IBoardDataService boardDataService;
        private readonly IChecklistsDataService checklistsDataService;

        public GetSummaries(
            IBookmarksDataService bookmarksDataService,
            IFeedDataService feedDataService,
            IJournalDataService journalDataService,
            IJobsDataService jobsDataService,
            ILoginSessionDataService loginSessionDataService,
            ILoginService loginService,
            INotebookDataService notebookDataService,
            IPersonalHealthService personalHealthService,
            IBoardDataService boardDataService,
            IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.bookmarksDataService = bookmarksDataService;
            this.feedDataService = feedDataService;
            this.journalDataService = journalDataService;
            this.jobsDataService = jobsDataService;
            this.loginSessionDataService = loginSessionDataService;
            this.notebookDataService = notebookDataService;
            this.personalHealthService = personalHealthService;
            this.boardDataService = boardDataService;
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var values = new Dictionary<string, Func<Task<string>>>();
            values.Add("login sessions", async () => (await loginSessionDataService.GetAllActiveSessions()).Count.ToString());
            values.Add("bookmarks", async () => (await bookmarksDataService.GetTotal()).ToString());
            values.Add("new bookmarks", async () => (await bookmarksDataService.GetRecentCount()).ToString());
            values.Add("weight change (week)", async () => (await personalHealthService.CalculateRecentlyLostWeight()).ToString(CultureInfo.InvariantCulture));
            values.Add("weight change", async () => (await personalHealthService.TotalWeightChange()).ToString(CultureInfo.InvariantCulture));
            values.Add("log entries", async () => (await journalDataService.GetCount()).ToString() );
            values.Add("new log entries", async () => (await journalDataService.GetRecentEntryCount()).ToString() );
            values.Add("active jobs", async () => (await jobsDataService.GetActiveJobs()).Count.ToString());
            values.Add("feeds", async () => (await feedDataService.GetFeeds()).Count.ToString());
            values.Add("unread items", async () => (await feedDataService.GetFeeds()).Sum(i => i.unread_count).ToString());
            values.Add("notes", async() => (await notebookDataService.GetNoteCount()).ToString());
            values.Add("boards", async() => (await boardDataService.GetBoardCount()).ToString());
            values.Add("incomplete board items", async() => (await boardDataService.GetIncompleteItemCount()).ToString());
            values.Add("recently added board items", async() => (await boardDataService.GetRecentlyAddedItemCount()).ToString());
            values.Add("recently completed board items", async() => (await boardDataService.GetRecentlyCompletedItemCount()).ToString());
            values.Add("checklists", async () => (await checklistsDataService.GetChecklists()).Count.ToString());
            values.Add("completed checklists", async () => (await checklistsDataService.GetChecklistCompletionLog()).Count.ToString());
            var counter = 0;
            try
            {
                var summaries = await Task.WhenAll(values.Select(async v =>
                {
                    try
                    {
                        return new {label = v.Key, amount = await v.Value(), id = ++counter};
                    }
                    catch(Exception)
                    {
                        return null;
                    }
                }));
                return CommandResult.CreateSuccessResult(summaries.Where(s => s!= null));
            }
            catch (Exception e)
            {
                Logger.Error("Could not build summaries");
                Logger.Error(e.Message);

                var summary = new {label = "Errors Building Summaries", amount = "1"};
                return CommandResult.CreateSuccessResult(new List<object> {summary});
            }
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
