using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class UpsertChecklistItem : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;
        public ChecklistItem Item { get; set; }

        public UpsertChecklistItem(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistsDataService.UpsertChecklistItem(Item);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Item.name.Length > 0 && Item.description != null &&
                                   Item.type != null);
        }
    }
}
