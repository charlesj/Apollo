using System;
using Apollo.CommandSystem;
using Apollo.Data;
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
            container.Register<IServiceLocator>(() => this, Lifestyle.Singleton);
            container.Register<IConfiguration, Configuration>(Lifestyle.Singleton);

            // command system bindings
            container.Register<ICommandHydrator, CommandHydrator>(Lifestyle.Singleton);
            container.Register<ICommandLocator, CommandLocator>(Lifestyle.Singleton);
            container.Register<ICommandProcessor, CommandProcessor>(Lifestyle.Singleton);

            // data bindings
            container.Register<IDbConnectionFactory, ConnectionFactory>();
            container.Register<IJournalDataService, JournalDataService>();

            // runtime bindings
            container.Register<IRuntime, SimpleLoopRuntime>();
            container.Register<IRuntimeContext, RuntimeContext>(Lifestyle.Singleton);

            // server bindings
            container.Register<IHttpServer, HttpServer>();
            container.Register<IJsonRpcCommandTranslator, JsonRpcCommandTranslator>();
            container.Register<IJsonRpcHttpConverter, JsonRpcHttpConverter>();
            container.Register<IJsonRpcRequestLogger, JsonRpcRequestLogger>();
            container.Register<IJsonRpcRequestParser, JsonRpcRequestParser>();
            container.Register<IJsonRpcProcessor, JsonRpcProcessor>();

            // utility binding
            container.Register<IClock, Clock>(Lifestyle.Singleton);
            container.Register<IEnvironmentReader, EnvironmentReader>(Lifestyle.Singleton);
            container.Register<IJsonSerializer, ApolloJsonSerializer>(Lifestyle.Singleton);
        }

        public TService Get<TService>() where TService : class
        {
            return this.container.GetInstance<TService>();
        }

        public object Get(Type type)
        {
            return this.container.GetInstance(type);
        }

        public void Test()
        {
            this.container.Verify(VerificationOption.VerifyAndDiagnose);
        }
    }
}