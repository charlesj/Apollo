using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetChecklistCompletionLog : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;


        public GetChecklistCompletionLog(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await checklistsDataService.GetChecklistCompletionLog());
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
