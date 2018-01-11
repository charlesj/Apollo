using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notebook
{
    public class GetNote : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;
        public int Id { get; set; }

        public GetNote(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            var note = await dataService.GetNote(Id);
            return CommandResult.CreateSuccessResult(note);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Id > 0);
        }

        public override object ExamplePayload()
        {
            return new { id=0};
        }
    }
}
