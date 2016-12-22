using Apollo.Runtime;
using Apollo.Utilities;
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

            container.Register<IClock, Clock>(Lifestyle.Singleton);
            container.Register<IRuntime, SimpleLoopRuntime>();
            container.Register<IRuntimeContext, RuntimeContext>(Lifestyle.Singleton);
        }

        public TService Get<TService>() where TService : class
        {
            return this.container.GetInstance<TService>();
        }
    }
}