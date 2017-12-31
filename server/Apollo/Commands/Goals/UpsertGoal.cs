using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.Documents;
using Apollo.Services;

namespace Apollo.Commands.Goals
{
    public class UpsertGoal : AuthenticatedCommand
    {
        private readonly IApolloDocumentStore documentStore;

        public Goal Goal { get; set; }

        public UpsertGoal(ILoginService loginService, IApolloDocumentStore documentStore) : base(loginService)
        {
            this.documentStore = documentStore;
        }

        public override Task<CommandResult> Execute()
        {
            documentStore.Upsert(Goal);
            return Task.FromResult(CommandResult.SuccessfulResult);
        }

        public override Task<bool> IsValid()
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(Goal.Slug));
        }
    }
}