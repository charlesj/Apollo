namespace Apollo.Data
{
    public abstract class BaseDataService
    {
        protected readonly IDbConnectionFactory connectionFactory;

        protected BaseDataService(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
    }
}