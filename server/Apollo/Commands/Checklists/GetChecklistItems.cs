using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class GetChecklistItems : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;

        public int id { get; set; }

        public GetChecklistItems(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            return CommandResult.CreateSuccessResult(await checklistsDataService.GetChecklistItems(id));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(id > 0);
        }
    }
}
