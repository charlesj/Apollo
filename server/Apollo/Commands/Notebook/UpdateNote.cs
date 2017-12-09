using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notebook
{
    public class UpdateNote : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;
        public int id { get; set; }
        public string name { get; set; }
        public string body { get; set; }

        public UpdateNote(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.UpdateNote(id, name, body);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(id > 0 && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(body));
        }

        public override object ExamplePayload()
        {
            return new { id=0, name, body};
        }
    }
}
