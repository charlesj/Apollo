using System.Linq;
using Apollo.Commands.Meta;
using Apollo.CommandSystem;
using Apollo.Server;
using Apollo.ServiceLocator;
using Xunit;

namespace Apollo.Tests.ServiceLocator
{
    public class SimpleInjectorServiceLocatorTests
    {
        [Fact]
        public void CanInstantiate()
        {
            var locator = new SimpleInjectorServiceLocator();
            Assert.NotNull(locator);
        }

        [Fact]
        public void CanGetItself()
        {
            var locator = new SimpleInjectorServiceLocator();
            locator.RegisterServices();
            var located = locator.Get<IServiceLocator>();
            Assert.Same(locator, located);
        }

        [Fact]
        public void CanBuildFromTypeWithNoDependencies()
        {
            var locator = new SimpleInjectorServiceLocator();

            var testCommand = locator.Get(typeof(TestCommand));
            Assert.IsType<TestCommand>(testCommand);
        }

        [Fact]
        public void CanBuildFromTypeWithDependencies()
        {
            var locator = new SimpleInjectorServiceLocator();
            locator.RegisterServices();
            var testCommand = locator.Get(typeof(JsonRpcRequestParser));
            Assert.IsType<JsonRpcRequestParser>(testCommand);
        }

        public class IntegrationTests
        {
            [Fact]
            public void PassesSanityCheck()
            {
                var locator = new SimpleInjectorServiceLocator();
                locator.RegisterServices();

                locator.Test();
            }

            [Fact]
            public void CanBuildAllCommands()
            {
                var locator = new SimpleInjectorServiceLocator();
                locator.RegisterServices();
                
                var type = typeof(ICommand);
                var commandTypes = type.Assembly.GetTypes()
                    .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract);
                foreach (var commandType in commandTypes)
                {
                    var instance = locator.Get(commandType);
                    Assert.NotNull(instance);
                }
            }
        }
    }
}