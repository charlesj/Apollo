using System.Collections.Generic;
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

        protected void SetupQuery<TResult>(IEnumerable<TResult> result)
        {
            connection.Setup(c => c.QueryAsync<TResult>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.FromResult(result));
        }

        protected void VerifyQuery<TResult>(string query, object obj)
        {
            this.connection.Verify(c =>
                c.QueryAsync<TResult>(query, obj), Times.Once());
        }
    }
}
