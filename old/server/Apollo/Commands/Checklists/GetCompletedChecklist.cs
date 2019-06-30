using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetCompletedChecklist : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;

        public int completed_checklist_id { get; set; }

        public GetCompletedChecklist(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var checklistInfo = (await checklistsDataService.GetChecklistCompletionLog())
                                .Single(cc => cc.completion_id == completed_checklist_id);

            var items = await checklistsDataService.GetCompletedChecklistItemInfo(completed_checklist_id);

            return CommandResult.CreateSuccessResult(new {checklistInfo, items});
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(completed_checklist_id > 0);
        }

        public override object ExamplePayload()
        {
            return new { completed_checklist_id };
        }
    }
}
