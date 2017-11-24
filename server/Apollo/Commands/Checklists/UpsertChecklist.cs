using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class UpsertChecklist : AuthenticatedCommand
    {
        private readonly IChecklistsDataService checklistsDataService;
        public Checklist Checklist { get; set; }

        public UpsertChecklist(ILoginService loginService, IChecklistsDataService checklistsDataService) : base(loginService)
        {
            this.checklistsDataService = checklistsDataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await checklistsDataService.UpsertChecklist(Checklist);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Checklist.name.Length > 0 && Checklist.description != null &&
                                   Checklist.type != null);
        }
    }
}
