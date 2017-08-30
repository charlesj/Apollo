using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notebook
{
    public class AddNote : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;
        public string Name { get; set; }
        public string Body { get; set; }
        
        public AddNote(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.CreateNote(Name, Body);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Body));
        }
    }
}