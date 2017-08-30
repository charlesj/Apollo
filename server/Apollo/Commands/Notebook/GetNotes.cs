using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notebook
{
    public class GetNotes : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;

        public GetNotes(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var notes = await dataService.GetNoteListing();
            return CommandResult.CreateSuccessResult(notes);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}