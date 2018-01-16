using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class DeleteCompletedChecklist : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;
        public int id { get; set; }

        public DeleteCompletedChecklist(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistsDataService.DeleteChecklistCompletion(id);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(id > 0);
        }

        public override object ExamplePayload()
        {
            return new { id };
        }
    }
}
