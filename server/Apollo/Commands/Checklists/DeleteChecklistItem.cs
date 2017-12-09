using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class DeleteChecklistItem : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;
        public int id { get; set; }

        public DeleteChecklistItem(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistsDataService.DeleteChecklistItem(id);
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
