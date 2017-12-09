using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notebook
{
    public class AddNote : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;
        public string name { get; set; }
        public string body { get; set; }

        public AddNote(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.CreateNote(name, body);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(body));
        }

        public override object ExamplePayload()
        {
            return new { name, body};
        }
    }
}
