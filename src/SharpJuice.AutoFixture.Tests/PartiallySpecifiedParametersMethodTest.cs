using System;
using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Xunit;

namespace SharpJuice.AutoFixture.Tests
{
	public class PartiallySpecifiedParametersMethodTest
	{
		[Fact]
		public void SpecifyingParameters_ParametersWithSameNamesInjected()
		{
			var fixture = new Fixture();
			var parameters = new {X = 100500, somestring = "Red Fox"};

			fixture.Customize<Sut>(
				c => c.FromFactory(new MethodInvoker(new PartiallySpecifiedMethodQuery(new GreedyConstructorQuery(), parameters))));
			
			var sut = fixture.Create<Sut>();

			sut.SomeString.Should().Be(parameters.somestring);
			sut.X.Should().Be(parameters.X);
			sut.Y.Should().NotBe(parameters.X);
			sut.Z.Should().NotBe(parameters.somestring);
		}

		[Fact]
		public void SpecifyingWrongParameters_ExceptionThrows()
		{
			var fixture = new Fixture();
			var parameters = new { X = 100500, UnknownString = "Red Fox" };

			fixture.Customize<Sut>(
				c => c.FromFactory(new MethodInvoker(new PartiallySpecifiedMethodQuery(new ModestConstructorQuery(), parameters))));

			var sutAct = new Action(() => fixture.Create<Sut>());

			sutAct.Should().Throw<ObjectCreationException>();
		}

		
		public class Sut
		{
			public Sut(int x, int y, string z, string someString)
			{
				X = x;
				Y = y;
				Z = z;
				SomeString = someString;
			}

			public int X { get; }

			public int Y { get; }

			public string Z { get;  }

			public string SomeString { get;  }
		}
	}	
}
