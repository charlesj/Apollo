using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetChecklistCompletions : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;

        public int id { get; set; }

        public GetChecklistCompletions(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await checklistsDataService.GetChecklistCompletions(id));
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
