using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Data
{
    public interface IChecklistsDataService
    {
        Task UpsertChecklist(Checklist checklist);
        Task UpsertChecklistItem(ChecklistItem item);
        Task DeleteChecklist(int id);
        Task DeleteChecklistItem(int id);
        Task<IReadOnlyList<Checklist>> GetChecklists();
        Task<IReadOnlyList<ChecklistItem>> GetChecklistItems(int checklist_id);
        Task<int> UpsertChecklistCompletion(ChecklistCompletion completion);
        Task UpsertChecklistCompletionItem(ChecklistCompletionItem item);
        Task DeleteChecklistCompletion(int id);
        Task DeleteChecklistCompletionItem(int id);
        Task<IReadOnlyList<ChecklistCompletion>> GetChecklistCompletions(int checklist_id);
        Task<IReadOnlyList<ChecklistCompletionItem>> GetCompletionItems(int checklist_completion_id);
        Task<IReadOnlyList<ChecklistCompletionItem>> GetCompletionHistory(int checklist_item_id);
        Task<IReadOnlyList<ChecklistCompletionLogEntry>> GetChecklistCompletionLog();
        Task<IReadOnlyList<CompletedChecklistItemInfo>> GetCompletedChecklistItemInfo(int completed_checklist_id);
    }

    public class ChecklistsDataService : BaseDataService, IChecklistsDataService
    {
        public ChecklistsDataService(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task UpsertChecklist(Checklist checklist)
        {
            if (checklist.id == 0)
            {
                await Execute("insert into checklists(name, type, description, created_at, updated_at) values " +
                              "(@name, @type, @description, current_timestamp, current_timestamp)", checklist);
            }
            else
            {
                await Execute("update checklists set name=@name, type=@type, description=@description, " +
                              "updated_at=current_timestamp where id=@id", checklist);
            }
        }

        public async Task UpsertChecklistItem(ChecklistItem item)
        {
            if (item.id == 0)
            {
                await Execute("insert into checklist_items(checklist_id, name, type, description, created_at, " +
                              "updated_at) values (@checklist_id, @name, @type, @description, current_timestamp, " +
                              "current_timestamp)", item);
            }
            else
            {
                await Execute("update checklist_items set name=@name, type=@type, description=@description, " +
                              "updated_at=current_timestamp where id=@id", item);
            }
        }

        public async Task DeleteChecklist(int id)
        {
            await Execute("update checklists set deleted_at=current_timestamp where id=@id", new {id});
        }

        public async Task DeleteChecklistItem(int id)
        {
            await Execute("update checklist_items set deleted_at=current_timestamp where id=@id", new {id});
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

        public async Task UpsertChecklistCompletionItem(ChecklistCompletionItem item)
        {
            if (item.id == 0)
            {
                await Execute("insert into checklist_completion_items(checklist_completion_id, checklist_item_id, " +
                              "completed, created_at, updated_at) values (@checklist_completion_id, " +
                              "@checklist_item_id, @completed, current_timestamp, current_timestamp)", item);
            }
            else
            {
                await Execute("update checklist_completion_items set completed=@completed, updated_at=@updated_at " +
                              "where id=@id", item);
            }
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
    }

    public class Checklist
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class ChecklistItem
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

    public class ChecklistCompletion
    {
        public int id { get; set; }
        public int checklist_id { get; set; }
        public string notes { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class ChecklistCompletionItem
    {
        public int id { get; set; }
        public int checklist_completion_id { get; set; }
        public int checklist_item_id { get; set; }
        public int completed { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

    public class ChecklistCompletionLogEntry
    {
        public int completion_id { get; set; }
        public string name { get; set; }
        public string notes { get; set; }
        public DateTime completed_at { get; set; }
    }

    public class CompletedChecklistItemInfo
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
