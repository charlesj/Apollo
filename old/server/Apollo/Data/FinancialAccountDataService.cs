using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{

    public interface IFinancialAccountDataService
    {
        Task<IReadOnlyList<FinancialAccount>> GetAccounts();
        Task<FinancialAccount> UpsertAccount(FinancialAccount account);
        Task DeleteAccount(int accountId);
        Task<IReadOnlyList<FinancialTransaction>> GetTransactions(int accountId);
        Task<FinancialTransaction> UpsertTransaction(FinancialTransaction transaction);
        Task DeleteTransaction(int transactionId);
    }

    public class FinancialAccountDataService: BaseDataService, IFinancialAccountDataService
    {
        public FinancialAccountDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public Task<IReadOnlyList<FinancialAccount>> GetAccounts()
        {
            return QueryAsync<FinancialAccount>("select * from financial_accounts");
        }

        public async Task<FinancialAccount> UpsertAccount(FinancialAccount account)
        {
            var id = account.id;
            if (id == default(int))
            {
                id = await InsertAndReturnId(
                    "insert into financial_accounts(name, type, description, created_at, updated_at) " +
                    "values (@name, @type, @description, current_timestamp, current_timestamp) " +
                    "returning id",
                    account);
            }
            else
            {
                await Execute(
                    "update financial_accounts set name=@name, type=@type, description=@description, " +
                    "updated_at=current_timestamp where id=@id",
                    account);
            }

            return (await QueryAsync<FinancialAccount>(
                    "select * from financial_accounts where id=@id",
                    new {id})
                ).Single();
        }

        public async Task DeleteAccount(int accountId)
        {
            await Execute("update financial_accounts set deleted_at=current_timestamp where id=@id",
                new {id = accountId});
        }

        public Task<IReadOnlyList<FinancialTransaction>> GetTransactions(int accountId)
        {
            return QueryAsync<FinancialTransaction>("select * from financial_transactions where account_id=@accountId",
                new {accountId});
        }

        public async Task<FinancialTransaction> UpsertTransaction(FinancialTransaction transaction)
        {
            var id = transaction.id;
            if (id == default(int))
            {
                id = await InsertAndReturnId(
                    "insert into financial_transactions(account_id, tags, occurred_at, amount, " +
                    "name, notes, created_at, updated_at) values (@account_id, @tags, " +
                    "@occurred_at, @amount, @name, @notes, current_timestamp, current_timestamp) returning id",
                    transaction);
            }
            else
            {
                await Execute(
                    "update financial_transactions set account_id=@account_id, " +
                    "tags=@tags, occurred_at=@occurred_at, amount=@amount, " +
                    "name=@name, notes=@notes, updated_at=current_timestamp where id=@id",
                    transaction);
            }

            return (await QueryAsync<FinancialTransaction>(
                "select * from financial_transactions where id=@id",
                new {id})
            ).Single();
        }

        public async Task DeleteTransaction(int transactionId)
        {
            await Execute("update financial_transactions set deleted_at=current_timestamp where id=@id",
                new {id = transactionId});
        }
    }

    public class FinancialAccount
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class FinancialTransaction
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public DateTime occurred_at { get; set; }
        public Decimal amount { get; set; }
        public string name { get; set; }
        public string[] tags { get; set; }
        public string notes { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
