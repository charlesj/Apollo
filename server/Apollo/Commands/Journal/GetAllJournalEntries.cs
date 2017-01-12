using System.Threading.Tasks;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Data.ResultModels;
using Dapper;

namespace Apollo.Commands.Journal
{
    public class GetAllJournalEntries : ICommand
    {
        private readonly IDbConnectionFactory connectionFactory;

        public GetAllJournalEntries(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<CommandResult> Execute()
        {
            using (var connection = await connectionFactory.GetConnection())
            {
                var results = await connection.QueryAsync<JournalEntry>("select * from journal");
                return new CommandResult{ ResultStatus = CommandResultType.Success, Result = results };
            }
        }

        public Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }
}