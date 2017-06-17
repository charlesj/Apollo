using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMoq;
using Moq;
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

        public Mock<TMockType> Mock<TMockType>() where TMockType : class
        {
            return this.Mocker.GetMock<TMockType>();
        }

        public string GetAnonymousString(object obj, string propName)
        {
            var property = obj.GetType().GetProperties().Single(p => p.Name == propName);
            return property.GetValue(obj) as string;
        }

        public TVal GetAnon<TVal>(object obj, string propName)
        {
            var property = obj.GetType().GetProperties().Single(p => p.Name == propName);
            return (TVal)property.GetValue(obj);
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