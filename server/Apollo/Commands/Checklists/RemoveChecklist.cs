using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class RemoveChecklist : AuthenticatedCommand
    {
        private readonly IChecklistService checklistService;

        public int id { get; set; }

        public RemoveChecklist(ILoginService loginService, IChecklistService checklistService) : base(loginService)
        {
            this.checklistService = checklistService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistService.DeleteChecklist(id);
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
