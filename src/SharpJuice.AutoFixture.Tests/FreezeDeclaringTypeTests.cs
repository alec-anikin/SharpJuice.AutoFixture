using AutoFixture;
using FluentAssertions;
using Xunit;

namespace SharpJuice.AutoFixture.Tests
{
    public sealed class FreezeDeclaringTypeTests
    {
        private readonly IFixture _fixture;

        public FreezeDeclaringTypeTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void FreezingParameter_ReturnsFrozenInstance()
        {
            var p = _fixture.FreezeParameter<RedType, Parameter>();

            var instance = _fixture.Create<RedType>();
            var anotherInstance = _fixture.Create<RedType>();

            instance.P.Should().BeSameAs(p);
            anotherInstance.P.Should().BeSameAs(p);

            instance.Y.Should().NotBe(anotherInstance.Y);
        }

        [Fact]
        public void FreezingParameter_FrozenOnlyForSpecifiedType()
        {
            var p = _fixture.FreezeParameter<RedType, Parameter>();

            var red = _fixture.Create<RedType>();
            var yellow = _fixture.Create<YellowType>();

            red.P.Should().BeSameAs(p);

            yellow.Other.Should().NotBeSameAs(p);
            yellow.Other.Should().NotBeEquivalentTo(p);
        }

        [Fact]
        public void FreezingParameterFor2Types_FrozenForBoth()
        {
            var redP = _fixture.FreezeParameter<RedType, Parameter>();
            var yellowP = _fixture.FreezeParameter<YellowType, Parameter>();

            var red = _fixture.Create<RedType>();
            var yellow = _fixture.Create<YellowType>();

            redP.Should().NotBeEquivalentTo(yellowP);

            red.P.Should().BeSameAs(redP);
            yellow.Other.Should().BeSameAs(yellowP);
        }

        public class Parameter
        {
            public int X { get; }
            public string Y { get; }

            public Parameter(int x, string y)
            {
                X = x;
                Y = y;
            }
        }

        public sealed class RedType
        {
            public Parameter P { get; }
            public int Y { get; }

            public RedType(Parameter p, int y)
            {
                P = p;
                Y = y;
            }
        }

        public sealed class YellowType
        {
            public string A { get; }
            public Parameter Other { get; }

            public YellowType(string a, Parameter other)
            {
                A = a;
                Other = other;
            }
        }
    }
}
