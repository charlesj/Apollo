using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class UpsertChecklistCompletionItem : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;
        public ChecklistCompletionItem Item { get; set; }

        public UpsertChecklistCompletionItem(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistsDataService.UpsertChecklistCompletionItem(Item);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Item.checklist_completion_id > 0 && Item.checklist_item_id > 0);
        }
    }
}
