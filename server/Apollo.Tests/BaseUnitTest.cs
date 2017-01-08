using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMoq;
using Moq.Language.Flow;

namespace Apollo.Tests
{
    public abstract class BaseUnitTest<TClassUnderTest>
    {
        protected TClassUnderTest ClassUnderTest { get; private set; }
        protected AutoMoqer Mocker { get; private set; }

        protected BaseUnitTest()
        {
            this.Mocker = new AutoMoqer();
            this.ClassUnderTest = Mocker.Create<TClassUnderTest>();
        }
    }

    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
            params TResult[] results) where T : class  {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}