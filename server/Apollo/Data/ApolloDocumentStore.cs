using System.Linq;
using Marten;
using Marten.Linq;

namespace Apollo.Data
{
    public interface IApolloDocumentStore
    {
        TDocumentType Get<TDocumentType>(string key) where TDocumentType : IDocument;
        void Upsert<TDocumentType>(TDocumentType document) where TDocumentType : IDocument;
        void Delete<TDocumentType>(string key) where TDocumentType : IDocument;
        IQuerySession GetQuerySession();
    }

    public interface IDocument
    {
        string Id { get; }
    }

    public class ApolloDocumentStore : IApolloDocumentStore
    {
        protected DocumentStore documentStore { get; }
        private const string ConnectionStringTemplate = "Host={0};Username={1};Password={2};Database={3}";

        public ApolloDocumentStore(IConfiguration configuration)
        {
            var connectionString = string.Format(
                ConnectionStringTemplate,
                configuration.DatabaseServer(),
                configuration.DatabaseUsername(),
                configuration.DatabasePassword(),
                configuration.DatabaseName());
            documentStore = DocumentStore.For(connectionString);
        }

        public IQuerySession GetQuerySession()
        {
            return documentStore.QuerySession();
        }

        public TDocumentType Get<TDocumentType>(string key) where TDocumentType : IDocument
        {
            using (var session = documentStore.LightweightSession())
            {
                return session.Load<TDocumentType>(key);
            }
        }

        public void Upsert<TDocumentType>(TDocumentType document) where TDocumentType : IDocument
        {
            using (var session = documentStore.LightweightSession())
            {
                session.Store(document);
                session.SaveChanges();
            }
        }

        public void Delete<TDocumentType>(string key) where TDocumentType : IDocument
        {
            using (var session = documentStore.LightweightSession())
            {
                session.Delete<TDocumentType>(key);
                session.SaveChanges();
            }
        }
    }
}
