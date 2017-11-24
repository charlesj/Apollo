using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class UpsertChecklistCompletion : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;
        public ChecklistCompletion Item { get; set; }

        public UpsertChecklistCompletion(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistsDataService.UpsertChecklistCompletion(Item);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Item.notes != null && Item.checklist_id > 0);
        }
    }
}
