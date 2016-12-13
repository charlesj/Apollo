namespace Apollo.ServiceLocator
{
    public interface IServiceLocator
    {
        TService Get<TService>() where TService : class;
    }
}