using System.Collections.Generic;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class AddCompletedChecklist : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;

        public List<ChecklistCompletionItem> items { get; set; }
        public int checklist_id { get; set; }
        public string notes { get; set; }

        public AddCompletedChecklist(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var checklistCompletion = new ChecklistCompletion {checklist_id = checklist_id, notes = notes};

            var id = await checklistsDataService.UpsertChecklistCompletion(checklistCompletion);
            foreach (var completionItem in items)
            {
                completionItem.checklist_completion_id = id;
                await checklistsDataService.UpsertChecklistCompletionItem(completionItem);
            }

            return CommandResult.CreateSuccessResult(new {id});
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(notes != null && checklist_id > 0);
        }
    }
}
