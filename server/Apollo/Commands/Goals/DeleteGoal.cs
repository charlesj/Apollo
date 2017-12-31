using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Goals
{
    public class DeleteGoal : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;

        public string Id { get; set; }

        public DeleteGoal(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override Task<CommandResult> Execute()
        {
            documentStore.Delete<Goal>(Id);
            return Task.FromResult(CommandResult.SuccessfulResult);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Id));
        }
    }
}