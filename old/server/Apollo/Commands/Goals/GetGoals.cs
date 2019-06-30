using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Goals
{
    public class GetGoals : AuthenticatedCommand
    {
        private readonly IGoalsDataService goalsDataService;

        public GetGoals(ILoginService loginService, IGoalsDataService goalsDataService) : base(loginService)
        {
            this.goalsDataService = goalsDataService;
        }

        public override Task<CommandResult> Execute()
        {
            return Task.FromResult(CommandResult.CreateSuccessResult(goalsDataService.GetGoals()));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
