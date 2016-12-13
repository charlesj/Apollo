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

    }
}