using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;

namespace Apollo.Services
{
    public interface IChecklistService
    {
        Task<Checklist> SaveChecklist(Checklist checklist);
        Task<IReadOnlyList<Checklist>> GetAll();
        Task DeleteChecklist(int id);
    }

    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistsDataService dataService;

        public ChecklistService(IChecklistsDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<Checklist> SaveChecklist(Checklist checklist)
        {
            var models = checklist.ToModels();
            Logger.Info("updating checklist", checklist);
            var updatedChecklist = await dataService.UpsertChecklist(models.Item1);
            Logger.Info("updated checklist", updatedChecklist);
            var currentItems = await dataService.GetChecklistItems(updatedChecklist.id);
            var deletableItems = currentItems.Where(curr => !models.Item2.Any(item => item.id == curr.id));
            foreach (var deletableItem in deletableItems)
            {
                Logger.Info("deleting item", deletableItem);
                await dataService.DeleteChecklistItem(deletableItem.id);
            }

            var items = await Task.WhenAll(models.Item2.Select(item =>
            {
                item.checklist_id = updatedChecklist.id;
                return dataService.UpsertChecklistItem(item);
            }));

            Logger.Info("updated items", items);
            return new Checklist(updatedChecklist, items);
        }

        public async Task<IReadOnlyList<Checklist>> GetAll()
        {
            var checklists = await dataService.GetChecklists();
            var checklistItems = await dataService.GetAllChecklistItems();
            return checklists.Select(checklist =>
                new Checklist(checklist, checklistItems.Where(item => item.checklist_id == checklist.id))).ToList();
        }

        public async Task DeleteChecklist(int id)
        {
            await dataService.DeleteChecklistItems(id);
            await dataService.DeleteChecklist(id);
        }
    }

    public class Checklist
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public List<ChecklistItem> items { get; set; }

        public Checklist()
        {
            this.items = new List<ChecklistItem>();
        }

        public Checklist(Data.Checklist model, IEnumerable<Data.ChecklistItem> items)
        {
            id = model.id;
            name = model.name;
            description = model.description;
            createdAt = model.created_at;
            updatedAt = model.updated_at;
            this.items = items.Select(item => new ChecklistItem(item)).ToList();
        }

        public Tuple<Data.Checklist, List<Data.ChecklistItem>> ToModels()
        {
            var checklistModel = new Data.Checklist
            {
                id = id,
                name = name,
                type="unset",
                description = description,
                created_at = createdAt,
                updated_at = updatedAt
            };

            var checklistItems = items.Select(item => new Data.ChecklistItem
            {
                id = item.id,
                name = item.name,
                type = item.type,
                description = item.description,
                created_at = item.createdAt,
                updated_at = item.updatedAt,
            }).ToList();

            return new Tuple<Data.Checklist, List<Data.ChecklistItem>>(checklistModel, checklistItems);
        }
    }

    public class ChecklistItem
    {
        public ChecklistItem()
        {
        }

        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public ChecklistItem(Data.ChecklistItem item)
        {
            id = item.id;
            name = item.name;
            type = item.type;
            description = item.description;
            createdAt = item.created_at;
            updatedAt = item.updated_at;
        }
    }
}
