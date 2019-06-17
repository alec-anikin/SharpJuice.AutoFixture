using System;
using AutoFixture;
using FluentAssertions;
using SharpJuice.AutoFixture.Tests.TestObjects;
using Xunit;

namespace SharpJuice.AutoFixture.Tests
{
    public sealed class BestFitConstructorTests
    {
        private readonly IFixture _fixture;

        public BestFitConstructorTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CreatingSingleCtorClassWithParameters_InstanceCreated()
        {
            var param = new {x = 134, Price = new Money(100, "USD")};
            var square = _fixture.Create<RedSquare>(param);

            square.X.Should().Be(param.x);
            square.Price.Should().Be(param.Price);
            square.Y.Should().NotBe(default);
            square.SomeClass.Should().NotBeNull();
        }

        [Fact]
        public void CreatingSingleCtorClassWithConvertibleParameters_InstanceCreated()
        {
            var param = new { y = 134, Price = new Money(100, "USD") };
            var square = _fixture.Create<RedSquare>(param);

            square.Y.Should().Be(param.y);
            square.Price.Should().Be(param.Price);
        }

        [Fact]
        public void CreatingSingleCtorClassWithNonConvertibleParameters_Throws()
        {
            var param = new {y = 134m, Price = new Money(100, "USD")};
            
            Action act = () => _fixture.Create<RedSquare>(param);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreatingMultiCtorClassWithMatch1_InstanceCreated()
        {
            var param = new { X = 123, y = 23.44445m, teXt = "Nothing to say" };

            var instance = _fixture.Create<MultiCtor>(param);

            instance.X.Should().Be(param.X);
            instance.Y.Should().Be(param.y);
            instance.Text.Should().Be(param.teXt);
            instance.Age.Should().Be(default);
            instance.LongText.Should().Be(default);
        }

        [Fact]
        public void CreatingMultiCtorClassWithMatch2_InstanceCreated()
        {
            var param = new { x = 123, y = 23.44445m, text = 123 };

            var instance = _fixture.Create<MultiCtor>(param);

            instance.X.Should().Be(param.x);
            instance.Y.Should().Be(param.y);
            instance.Text.Should().Be(default);
            instance.Age.Should().Be(default);
            instance.LongText.Should().Be(param.text);
        }

        [Fact]
        public void CreatingMultiCtorClassWithMatch3_InstanceCreated()
        {
            var param = new { y = 23.44445m, text = "123", age = (byte)18 };

            var instance = _fixture.Create<MultiCtor>(param);

            instance.X.Should().Be(default);
            instance.Y.Should().Be(param.y);
            instance.Text.Should().Be(param.text);
            instance.Age.Should().Be(param.age);
            instance.LongText.Should().Be(default);
        }

        [Fact]
        public void CreatingMultiCtorClassWithMatch4_InstanceCreated()
        {
            var param = new { x = DateTime.Now };

            var instance = _fixture.Create<MultiCtor>(param);

            instance.X.Should().Be(default);
            instance.Y.Should().Be(default);
            instance.Text.Should().Be(default);
            instance.Age.Should().Be(default);
            instance.LongText.Should().Be(default);
        }

        [Fact]
        public void CreatingMultiCtorClassWithMultiMatch_ModestConstructorUsed()
        {
            var param = new { text ="222" };

            var instance = _fixture.Create<MultiCtor>(param);

            instance.X.Should().Be(default);
            instance.Y.Should().NotBe(default);
            instance.Text.Should().Be(param.text);
            instance.Age.Should().Be(default);
            instance.LongText.Should().Be(default);
        }

        [Fact]
        public void CreatingMultiCtorClassWithNotMatched_Throws()
        {
            var param = new { x = 134, age = (byte)25 };

            Action act = () => _fixture.Create<MultiCtor>(param);
            act.Should().Throw<InvalidOperationException>();
        }


        [Fact]
        public void CreatingInstanceWithCustomizedConstructor_ReturnsSpecified()
        {
            var price = new Money(2589.54m, "USD");
            _fixture.CustomizeConstructor<RedSquare>(new { price });

            var instance = _fixture.Create<RedSquare>();

            instance.Price.Amount.Should().Be(price.Amount);
            instance.Price.Currency.Should().Be(price.Currency);
        }
    }
}
