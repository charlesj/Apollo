using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;

namespace Apollo.Tests
{
    public class MockRegistry
    {
        private Dictionary<Type, object> registry;

        public MockRegistry()
        {
            this.registry = new Dictionary<Type, object>();
        }

        public Mock<TMockType> Get<TMockType>() where TMockType : class
        {
            var type = typeof(TMockType);
            EnsureExists(type);
            return (Mock<TMockType>)this.registry[type];
        }

        public object Get(Type type)
        {
            EnsureExists(type);

            return ((dynamic)this.registry[type]).Object;
        }

        public void EnsureExists(Type type)
        {
            if (!this.registry.ContainsKey(type))
            {
                var genericMock = typeof(Mock<>).MakeGenericType(type);
                var genericConstructor = genericMock.GetConstructors()[0];
                var instance = genericConstructor.Invoke(new object[0]);
                this.registry.Add(type, (dynamic)instance);
            }
        }
    }
}
