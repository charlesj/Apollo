using Apollo.ServiceLocator;

namespace Apollo
{
    public class Kernel
    {
        public IServiceLocator Boot(BootOptions defaults)
        {
            var serviceLocator = new SimpleInjectorServiceLocator();
            serviceLocator.RegisterServices();
            return serviceLocator.Get<IServiceLocator>();
        }
    }
}