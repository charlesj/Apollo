using System;

namespace Apollo.ServiceLocator
{
    public interface IServiceLocator
    {
        TService Get<TService>() where TService : class;

        object Get(Type type);

        void RegisterServices();
    }
}
