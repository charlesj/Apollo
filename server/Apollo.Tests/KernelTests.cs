using System.Threading.Tasks;
using Apollo.Data;
using Apollo.ServiceLocator;
using Xunit;

namespace Apollo.Tests
{
    public class KernelTests
    {
        public class Boot : BaseUnitTest<Kernel>
        {
            [Fact]
            public void ReturnsFunctinginServiceLocator()
            {
                var testConfiguration = Mock<IConfiguration>();
                testConfiguration.Setup(c => c.IsValid()).Returns(true);

                var docBootstrapper = Mock<IDocumentStoreBoostrapper>();
                docBootstrapper.Setup(d => d.Bootstrap())
                    .Returns(Task.CompletedTask);

                var testLocator = Mock<IServiceLocator>();

                testLocator.Setup(l => l.Get<IConfiguration>())
                    .Returns(testConfiguration.Object);

                testLocator.Setup(l => l.Get<IServiceLocator>())
                    .Returns(testLocator.Object);

                testLocator.Setup(l => l.Get<IDocumentStoreBoostrapper>())
                    .Returns(docBootstrapper.Object);

                var kernel = new Kernel();
                var locator = kernel.Boot(BootOptions.Defaults, testLocator.Object);

                Assert.NotNull(locator);
                var located = locator.Get<IServiceLocator>();
                Assert.NotNull(located);
                Assert.Same(locator, located);
            }
        }
    }
}
