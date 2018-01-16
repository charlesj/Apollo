using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IChecklistsDataService
    {
        Task<Checklist> UpsertChecklist(Checklist checklist);
        Task<ChecklistItem> UpsertChecklistItem(ChecklistItem item);
        Task DeleteChecklist(int id);
        Task DeleteChecklistItems(int checklist_id);
        Task DeleteChecklistItem(int id);
        Task<IReadOnlyList<Checklist>> GetChecklists();
        Task<IReadOnlyList<ChecklistItem>> GetChecklistItems(int checklist_id);
        Task<int> UpsertChecklistCompletion(ChecklistCompletion completion);
        Task<ChecklistCompletionItem> UpsertChecklistCompletionItem(ChecklistCompletionItem item);
        Task DeleteChecklistCompletion(int id);
        Task DeleteChecklistCompletionItem(int id);
        Task<IReadOnlyList<ChecklistCompletion>> GetChecklistCompletions(int checklist_id);
        Task<IReadOnlyList<ChecklistCompletionItem>> GetCompletionItems(int checklist_completion_id);
        Task<IReadOnlyList<ChecklistCompletionItem>> GetCompletionHistory(int checklist_item_id);
        Task<IReadOnlyList<ChecklistCompletionLogEntry>> GetChecklistCompletionLog();
        Task<IReadOnlyList<CompletedChecklistItemInfo>> GetCompletedChecklistItemInfo(int completed_checklist_id);

        Task<CompletedChecklist> SaveCompletedChecklist(int checklist_id, string notes, List<ChecklistCompletionItem> items);
        Task<IReadOnlyList<CompletedChecklist>> GetCompletedChecklists(int checklist_id);
        Task<IReadOnlyList<ChecklistItem>> GetAllChecklistItems();
    }

    public class ChecklistsDataService : BaseDataService, IChecklistsDataService
    {
        public ChecklistsDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<Checklist> UpsertChecklist(Checklist checklist)
        {
            var instertSql = "insert into checklists(name, type, description, created_at, updated_at) values " +
                             "(@name, @type, @description, current_timestamp, current_timestamp) returning id";
            var updateSql = "update checklists set name=@name, type=@type, description=@description, " +
                            "updated_at=current_timestamp where id=@id";
            var selectSql = "select * from checklists where id=@id";
            return await Upsert(instertSql, updateSql, selectSql, checklist);
        }

        public async Task<ChecklistItem> UpsertChecklistItem(ChecklistItem item)
        {
            var insertSql = "insert into checklist_items(checklist_id, name, type, description, created_at, " +
                            "updated_at) values (@checklist_id, @name, @type, @description, current_timestamp, " +
                            "current_timestamp) returning id";
            var updateSql = "update checklist_items set name=@name, type=@type, description=@description, " +
                            "updated_at=current_timestamp where id=@id";
            var selectSql = "select * from checklist_items where id=@id";
            return await Upsert(insertSql, updateSql, selectSql, item);
        }

        public async Task DeleteChecklist(int id)
        {
            await Execute("update checklists set deleted_at=current_timestamp where id=@id", new {id});
        }

        public async Task DeleteChecklistItems(int checklist_id)
        {
            await Execute("update checklist_items set deleted_at=current_timestamp where checklist_id=@checklist_id", new {checklist_id});
        }

        public async Task DeleteChecklistItem(int id)
        {
            await Execute("update checklist_items set deleted_at=current_timestamp where id=@id", new {id});;
        }

        public async Task<IReadOnlyList<Checklist>> GetChecklists()
        {
            return await QueryAsync<Checklist>("select * from checklists where deleted_at is null");
        }

        public async Task<IReadOnlyList<ChecklistItem>> GetChecklistItems(int checklist_id)
        {
            return await QueryAsync<ChecklistItem>("select * from checklist_items where deleted_at is null " +
                                                   "and checklist_id=@checklist_id", new {checklist_id});
        }

        public async Task<int> UpsertChecklistCompletion(ChecklistCompletion completion)
        {
            if (completion.id == 0)
            {
                var idResults = await QueryAsync<IdResult>("insert into checklist_completions(checklist_id, notes, created_at, " +
                                           "updated_at) values (@checklist_id, @notes, current_timestamp, " +
                                           "current_timestamp) returning id", completion);
                return idResults.Single().id;
            }

            await Execute("update checklist_completions set notes=@notes, updated_at=current_timestamp " +
                          "where id=@id", completion);

            return completion.id;
        }

        public async Task<ChecklistCompletionItem> UpsertChecklistCompletionItem(ChecklistCompletionItem item)
        {
            var insertSql = "insert into checklist_completion_items(checklist_completion_id, checklist_item_id, " +
                            "completed, created_at, updated_at) values (@checklist_completion_id, " +
                            "@checklist_item_id, @completed, current_timestamp, current_timestamp) returning id";
            var updateSql = "update checklist_completion_items set completed=@completed, updated_at=@updated_at " +
                            "where id=@id";
            var selectSql = "select * from checklist_completion_items where id=@id";

            return await Upsert(insertSql, updateSql, selectSql, item);
        }

        public async Task DeleteChecklistCompletion(int id)
        {
            await Execute("update checklist_completions set deleted_at=current_timestamp where id=@id", new {id});
        }

        public async Task DeleteChecklistCompletionItem(int id)
        {
            await Execute("update checklist_completion_items set deleted_at=current_timestamp where id=@id", new {id});
        }

        public async Task<IReadOnlyList<ChecklistCompletion>> GetChecklistCompletions(int checklist_id)
        {
            return await QueryAsync<ChecklistCompletion>("select * from checklist_completions where " +
                                                         "checklist_id=@checklist_id", new {checklist_id});
        }

        public async Task<ChecklistCompletion> GetChecklistCompletion(int checklist_completion_id)
        {
            var queryResult = await QueryAsync<ChecklistCompletion>("select * from checklist_completions where " +
                                                                    "id=@checklist_completion_id",
                new {checklist_completion_id});

            return queryResult.Single();
        }

        public async Task<IReadOnlyList<ChecklistCompletionItem>> GetCompletionItems(int checklist_completion_id)
        {
            return await QueryAsync<ChecklistCompletionItem>("select * from checklist_completion_items " +
                                                             "where checklist_completion_id=@checklist_completion_id",
                new {checklist_completion_id});
        }

        public async Task<IReadOnlyList<ChecklistCompletionItem>> GetCompletionHistory(int checklist_item_id)
        {
            return await QueryAsync<ChecklistCompletionItem>("select * from checklist_completion_items " +
                                                             "where checklist_item_id=@checklist_item_id",
                new {checklist_item_id});
        }

        public async Task<IReadOnlyList<ChecklistCompletionLogEntry>> GetChecklistCompletionLog()
        {
            return await QueryAsync<ChecklistCompletionLogEntry>("select cc.id as completion_id, c.name as name, " +
                                                                 "cc.notes, cc.created_at as completed_at from " +
                                                                 "checklist_completions cc " +
                                                                 "join checklists c on cc.checklist_id=c.id " +
                                                                 "where cc.deleted_at is null " +
                                                                 "order by cc.created_at desc;");
        }

        public async Task<IReadOnlyList<CompletedChecklistItemInfo>> GetCompletedChecklistItemInfo(int completed_checklist_id)
        {
            return await QueryAsync<CompletedChecklistItemInfo>(
                "select cci.id,cci.completed,cci.checklist_completion_id," +
                "ci.name,ci.type,ci.description,ci.id as source_item_id " +
                "from checklist_completion_items cci join checklist_items " +
                "ci on cci.checklist_item_id=ci.id " +
                "where cci.checklist_completion_id=@completed_checklist_id",
                new {completed_checklist_id});
        }

        public async Task<CompletedChecklist> SaveCompletedChecklist(int checklist_id, string notes, List<ChecklistCompletionItem> items)
        {
            var checklistCompletion = new ChecklistCompletion {checklist_id = checklist_id, notes = notes};

            var id = await this.UpsertChecklistCompletion(checklistCompletion);
            foreach (var completionItem in items)
            {
                completionItem.checklist_completion_id = id;
                await this.UpsertChecklistCompletionItem(completionItem);
            }

            var checklistInfo = await GetChecklistCompletion(id);

            var completedItems = await GetCompletedChecklistItemInfo(id);

            return new CompletedChecklist {Checklist = checklistInfo, Items = completedItems};

        }

        public async Task<IReadOnlyList<CompletedChecklist>> GetCompletedChecklists(int checklist_id)
        {
            var completedChecklistInfo = await GetChecklistCompletions(checklist_id);

            var completedChecklistItems = await QueryAsync<CompletedChecklistItemInfo>(
                "select cci.id,cci.completed,cci.checklist_completion_id," +
                "ci.name,ci.type,ci.description,ci.id as source_item_id " +
                "from checklist_completion_items cci join checklist_items " +
                "ci on cci.checklist_item_id=ci.id");

            var completedChecklists = completedChecklistInfo.Select(checklist => new CompletedChecklist
            {
                Checklist = checklist,
                Items = completedChecklistItems.Where(item => item.checklist_completion_id == checklist.id).ToList()
            }).ToList();

            return completedChecklists;
        }

        public Task<IReadOnlyList<ChecklistItem>> GetAllChecklistItems()
        {
            return QueryAsync<ChecklistItem>("select * from checklist_items where deleted_at is null");
        }
    }

    public class Checklist : ITableModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class ChecklistItem : ITableModel
    {
        public int id { get; set; }
        public int checklist_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class ChecklistCompletion : ITableModel
    {
        public int id { get; set; }
        public int checklist_id { get; set; }
        public string notes { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class ChecklistCompletionItem : ITableModel
    {
        public int id { get; set; }
        public int checklist_completion_id { get; set; }
        public int checklist_item_id { get; set; }
        public int completed { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class CompletedChecklist
    {
        public ChecklistCompletion Checklist { get; set; }
        public IReadOnlyList<CompletedChecklistItemInfo> Items { get; set; }
    }

    public class ChecklistCompletionLogEntry
    {
        public int completion_id { get; set; }
        public string name { get; set; }
        public string notes { get; set; }
        public DateTime completed_at { get; set; }
    }

    public class CompletedChecklistItemInfo : ITableModel
    {
        public int id { get; set; }
        public int completed { get; set; }
        public int checklist_completion_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int source_item_id { get; set; }
    }
}
