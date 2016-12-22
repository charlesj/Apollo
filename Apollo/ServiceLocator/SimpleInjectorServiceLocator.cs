using Apollo.Runtime;
using Apollo.Server;
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

            // utility binding
            container.Register<IClock, Clock>(Lifestyle.Singleton);

            // runtime bindings
            container.Register<IRuntime, SimpleLoopRuntime>();
            container.Register<IRuntimeContext, RuntimeContext>(Lifestyle.Singleton);

            // server bindings
            container.Register<IHttpServer, HttpServer>();
            container.Register<IJsonRPCProcessor, JsonRPCProcessor>();
        }

        public TService Get<TService>() where TService : class
        {
            return this.container.GetInstance<TService>();
        }
    }
}