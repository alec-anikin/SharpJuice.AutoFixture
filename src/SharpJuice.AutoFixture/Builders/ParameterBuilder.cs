using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture.Builders
{
	public sealed class ParameterBuilder<TParameter> : ISpecimenBuilder
	{
		private readonly string _parameterName;
		private readonly TParameter _value;

		public ParameterBuilder(string parameterName, TParameter value)
		{
			_parameterName = parameterName;
			_value = value;
		}

		public object Create(object request, ISpecimenContext context)
		{
		    if (request is ParameterInfo pi &&
			    pi.ParameterType == typeof(TParameter) &&
			    string.Compare(pi.Name, _parameterName, StringComparison.OrdinalIgnoreCase) == 0)

				return _value;

			return new NoSpecimen();
		}
	}
}