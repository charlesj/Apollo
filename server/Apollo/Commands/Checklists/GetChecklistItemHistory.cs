using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetChecklistItemHistory : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;

        public int checklist_item_id { get; set; }

        public GetChecklistItemHistory(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(
                await checklistsDataService.GetCompletionHistory(checklist_item_id));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(checklist_item_id > 0);
        }

        public override object ExamplePayload()
        {
            return new { checklist_item_id };
        }
    }
}
