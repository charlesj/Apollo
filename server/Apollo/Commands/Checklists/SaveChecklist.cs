using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Services;

namespace Apollo.Commands.Checklists
{
    public class SaveChecklist : AuthenticatedCommand
    {
        private readonly IChecklistService checklistService;
        public Checklist Checklist { get; set; }

        public SaveChecklist(ILoginService loginService, IChecklistService checklistService) : base(loginService)
        {
            this.checklistService = checklistService;
        }

        public override async Task<CommandResult> Execute()
        {
            var saved = await checklistService.SaveChecklist(Checklist);
            return CommandResult.CreateSuccessResult(saved);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Checklist.name.Length > 0 && Checklist.description != null);
        }

        public override object ExamplePayload()
        {
            return new { Checklist = new Checklist()};
        }
    }
}
