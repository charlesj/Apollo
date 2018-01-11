using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notes
{
    public class UpsertNote : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;
        public int id { get; set; }
        public string name { get; set; }
        public string body { get; set; }

        public UpsertNote(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var note = await dataService.UpsertNote(new Note{id=id, name=name, body=body});
            return CommandResult.CreateSuccessResult(note);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(body));
        }

        public override object ExamplePayload()
        {
            return new { id=0, name, body};
        }
    }
}
