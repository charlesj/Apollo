using System;
using Apollo.CommandSystem;
using Apollo.Data;
using Apollo.Jobs;
using Apollo.Runtime;
using Apollo.Server;
using Apollo.Services;
using Apollo.Utilities;
using SimpleInjector;

namespace Apollo.ServiceLocator
{
    public class SimpleInjectorServiceLocator : IServiceLocator
    {
        private Container container;

        public SimpleInjectorServiceLocator()
        {
            container = new Container();
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
            container.Register<IBookmarksDataService, BookmarksDataService>();
            container.Register<IConnectionFactory, ConnectionFactory>();
            container.Register<IJournalDataService, JournalDataService>();
            container.Register<IJobsDataService, JobsDataService>();
            container.Register<ILoginSessionDataService, LoginSessionDataService>();
            container.Register<IMetricsDataService, MetricsDataService>();
            container.Register<ITodoItemDataService, TodoItemDataService>();
            container.Register<ITodoQueueItemDataService, TodoQueueItemDataService>();
            container.Register<IUserSettignsDataService, UserSettingsDataService>();

            // jobs bindings
            container.Register<IJobProcessor, JobProcessor>();

            // runtime bindings
            container.Register<IRuntime, SimpleLoopRuntime>();
            container.Register<IRuntimeContext, RuntimeContext>(Lifestyle.Singleton);

            // server bindings
            container.Register<IHttpRequestProcessor, HttpRequestProcessor>();
            container.Register<IJsonRpcCommandTranslator, JsonRpcCommandTranslator>();
            container.Register<IJsonRpcHttpConverter, JsonRpcHttpConverter>();
            container.Register<IJsonRpcRequestParser, JsonRpcRequestParser>();
            container.Register<IJsonRpcProcessor, JsonRpcProcessor>();

            // service bindings
            container.Register<IPersonalHealthService, PersonalHealthService>();
            container.Register<ILoginService, LoginService>();
            container.Register<ISchedulerService, SchedulerService>();
            container.Register<IUserSettingsService, UserSettingsService>();

            // utility binding
            container.Register<IBase64Converter, Base64Converter>(Lifestyle.Singleton);
            container.Register<IClock, Clock>(Lifestyle.Singleton);
            container.Register<IEnvironmentReader, EnvironmentReader>(Lifestyle.Singleton);
            container.Register<IJsonSerializer, ApolloJsonSerializer>(Lifestyle.Singleton);
            container.Register<IPasswordHasher, PasswordHasher>(Lifestyle.Singleton);
        }

        public TService Get<TService>() where TService : class
        {
            return container.GetInstance<TService>();
        }

        public object Get(Type type)
        {
            return container.GetInstance(type);
        }

        public void Test()
        {
            container.Verify(VerificationOption.VerifyAndDiagnose);
        }
    }
}
