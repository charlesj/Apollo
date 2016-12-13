using Apollo.ServiceLocator;
using Xunit;

namespace Apollo.Tests
{
    public class KernelTests
    {
        public class Boot
        {
            [Fact]
            public void ReturnsFunctinginServiceLocator()
            {
                var kernel = new Kernel();
                var locator = kernel.Boot(BootOptions.Defaults);

                Assert.NotNull(locator);
                var located = locator.Get<IServiceLocator>();
                Assert.NotNull(located);
                Assert.Same(locator, located);
            }
        }
    }
}