using System;
﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace Apollo.Tests
{
    public class BaseUnitTest<TClassUnderTest>
    {
        protected MockRegistry MockRegistry { get; set; }
        public TClassUnderTest ClassUnderTest { get; private set; }

        public BaseUnitTest()
        {
            this.MockRegistry = new MockRegistry();
            this.ClassUnderTest = this.Build();
        }

        public Mock<TMockType> Mock<TMockType>() where TMockType : class
        {
            return MockRegistry.Get<TMockType>();
        }

        public TClassUnderTest Build()
        {
            var constructorParams = new List<object>();
            var constructors = (typeof(TClassUnderTest)).GetConstructors();
            if (constructors.Length > 1)
                throw new InvalidOperationException("Cannot Test Types with multiple constructors - Code Smell");

            var constructor = constructors[0];
            var parameters = constructor.GetParameters();
            foreach (var parameter in parameters)
            {
                var parametType = parameter.ParameterType;
                if (!parametType.GetTypeInfo().IsAbstract && !parametType.GetTypeInfo().IsInterface)
                    throw new InvalidOperationException("Use more appropriate types (interface or abstract) for your dependencies");

                constructorParams.Add(MockRegistry.Get(parameter.ParameterType));
            }

            var instance = constructor.Invoke(constructorParams.ToArray());
            return (TClassUnderTest)instance;
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
