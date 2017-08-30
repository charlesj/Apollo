using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Services;

namespace Apollo.Commands.Notebook
{
    public class UpdateNote : AuthenticatedCommand
    {
        private readonly INotebookDataService dataService;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        
        public UpdateNote(ILoginService loginService, INotebookDataService dataService) : base(loginService)
        {
            this.dataService = dataService;
        }

        public override async Task<CommandResult> Execute()
        {
            await dataService.UpdateNote(Id, Name, Body);
            return CommandResult.SuccessfulResult;
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(Id > 0 && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Body));
        }
    }
}