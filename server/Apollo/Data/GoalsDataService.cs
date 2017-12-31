using System.Collections.Generic;
using System.Linq;
using Apollo.Data.Documents;

namespace Apollo.Data
{
    public interface IGoalsDataService
    {
        IReadOnlyList<Goal> GetGoals();
    }

    public class GoalsDataService : IGoalsDataService
    {
        private readonly IApolloDocumentStore documentStore;

        public GoalsDataService(IApolloDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public IReadOnlyList<Goal> GetGoals()
        {
            using (var querySession = documentStore.GetQuerySession())
            {
                var goals = querySession.Query<Goal>().ToList();
                return goals;
            }
        }
    }
}
