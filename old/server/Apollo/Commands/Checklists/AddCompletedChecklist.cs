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
            var completedChecklist = await checklistsDataService.SaveCompletedChecklist(checklist_id, notes, items);
            return CommandResult.CreateSuccessResult(completedChecklist);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(notes != null && checklist_id > 0);
        }

        public override object ExamplePayload()
        {
            return new { checklist_id=0, notes, items=new List<ChecklistCompletionItem>{new ChecklistCompletionItem(), new ChecklistCompletionItem()}};
        }
    }
}
