using System.Threading.Tasks;
using Apollo.Data;
using Moq;

namespace Apollo.Tests.Data
{
    public class BaseDataUnitTest<TClassUnderTest> : BaseUnitTest<TClassUnderTest>
    {
        protected Mock<ITestableDbConnection> connection;

        public BaseDataUnitTest()
        {
            connection = Mock<ITestableDbConnection>();

            Mock<IDbConnectionFactory>()
                .Setup(s => s.GetConnection())
                .Returns(Task.FromResult(connection.Object));
        }
    }
}