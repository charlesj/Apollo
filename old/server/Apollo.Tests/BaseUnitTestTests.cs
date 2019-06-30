using System;
using Xunit;
using Moq;

namespace Apollo.Tests
{
    public class BaseUnitTestTests
    {
        public class ClassUnderTest
        {
            [Fact]
            public void SystemUnderTest_AvailableUponInstantiation()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                Assert.NotNull(sut.ClassUnderTest);
            }
        }

        public class Build
        {
            [Fact]
            public void TypeWithMultipleConstructors_ThrowsException()
            {
                Assert.Throws<InvalidOperationException>(() => new BaseUnitTest<TypeWithMultipleConstructors>());
            }

            [Fact]
            public void CanBuildType_WithNoDependencies()
            {
                var sut = new BaseUnitTest<TestableTypeNoDependencies>();
                var built = sut.Build();
                Assert.NotNull(sut.ClassUnderTest);
            }

            [Fact]
            public void CanBuildType_WithInterfaceDependencies()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                var built = sut.Build();
                Assert.NotNull(sut.ClassUnderTest);
            }

            [Fact]
            public void CanBuildType_WithAbstractDependencies()
            {
                var sut = new BaseUnitTest<TypeWithAbstractDependcies>();
                var built = sut.Build();
                Assert.NotNull(sut.ClassUnderTest);
            }

            [Fact]
            public void ThrowsException_WhenBuildingClassWithInappropriateDependencies()
            {
                Assert.Throws<InvalidOperationException>(() => new BaseUnitTest<TypeWithInappropriateDependencies>());
            }
        }

        public class MockMethod
        {

            [Fact]
            public void CanGetInstance()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                var bar = sut.Mock<IBarService>();
            }

            [Fact]
            public void Instances_AreSingletons()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                var first = sut.Mock<IBarService>();
                var second = sut.Mock<IBarService>();
                Assert.Equal(first, second);
            }

            [Fact]
            public void Instances_AreSameAsSystemUnderTest()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                var first = sut.Mock<IFooService>();
                var second = sut.Build().FooService;
                Assert.Equal(first.Object, second);
            }

            [Fact]
            public void CanGet_TheMock()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                var mock = sut.Mock<IFooService>();
                Assert.NotNull(mock);
                Assert.IsType<Mock<IFooService>>(mock);
            }

            [Fact]
            public void CanSetup_TheMock()
            {
                var sut = new BaseUnitTest<TypeWithInterfaceDependencies>();
                var testObject = sut.Build();

                var initial = testObject.FooService.Name();

                var mock = sut.Mock<IFooService>();
                var expected = "hello";
                mock.Setup(foo => foo.Name()).Returns(expected);

                var result = testObject.FooService.Name();
                Assert.NotEqual(expected, initial);
                Assert.Equal(expected, result);
            }
        }

        public class TypeWithInappropriateDependencies
        {
            public TypeWithInappropriateDependencies(string inappropriate)
            { }
        }

        public class TypeWithInterfaceDependencies
        {
            public TypeWithInterfaceDependencies(IFooService fooService, IBarService barService)
            {
                this.FooService = fooService;
            }

            public IFooService FooService { get; private set; }
        }

        public class TypeWithAbstractDependcies
        {
            public TypeWithAbstractDependcies(TestAbstractClass tac)
            { }
        }

        public abstract class TestAbstractClass
        { }

        public class TypeWithMultipleConstructors
        {
            public TypeWithMultipleConstructors()
            {

            }

            public TypeWithMultipleConstructors(IFooService fooService)
            {

            }
        }

        public class TestableTypeNoDependencies
        {
        }

        public interface IFooService
        {
            string Name();
        }

        public interface IBarService
        {
        }
    }
}
