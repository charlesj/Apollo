using System.Linq;
using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Goals
{
    public class GetGoals : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;

        public bool IncludeCompleted { get; set; }

        public GetGoals(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override Task<CommandResult> Execute()
        {
            var query = documentStore.Query<Goal>().Where(g => !IncludeCompleted && g.Completed == false);
            return Task.FromResult(CommandResult.CreateSuccessResult(query.ToArray()));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }
    }
}
