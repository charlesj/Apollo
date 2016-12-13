using Apollo.Runtime;
using SimpleInjector;

namespace Apollo.ServiceLocator
{
    public class SimpleInjectorServiceLocator : IServiceLocator
    {
        private Container container;

        public SimpleInjectorServiceLocator()
        {
            this.container = new Container();
        }

        public void RegisterServices()
        {
            container.Register<IServiceLocator>(() => this);
            container.Register<IRuntime, SimpleLoopRuntime>();
        }

        public TService Get<TService>() where TService : class
        {
            return this.container.GetInstance<TService>();
        }
    }
}