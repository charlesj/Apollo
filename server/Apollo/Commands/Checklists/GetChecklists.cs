using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetChecklists : AuthenticatedCommand
    {
        private readonly IChecklistService checklistService;

        public GetChecklists(ILoginService loginService, IChecklistService checklistService) : base(loginService)
        {
            this.checklistService = checklistService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await checklistService.GetAll());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
