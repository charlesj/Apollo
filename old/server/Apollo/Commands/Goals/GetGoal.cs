using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Goals
{
    public class GetGoal : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;

        public string Id { get; set; }

        public GetGoal(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override Task<CommandResult> Execute()
        {
            var goal = documentStore.Get<Goal>(Id);
            return Task.FromResult(CommandResult.CreateSuccessResult(goal));
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Id));
        }
    }
}