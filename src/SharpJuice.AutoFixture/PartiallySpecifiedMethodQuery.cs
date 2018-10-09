using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
{
	public sealed class PartiallySpecifiedMethodQuery : IMethodQuery
	{
		private readonly IMethodQuery _baseQuery;
		private readonly object _parameters;

		public PartiallySpecifiedMethodQuery(IMethodQuery baseQuery, object parameters)
		{
			_baseQuery = baseQuery;
			_parameters = parameters;
		}

		public IEnumerable<IMethod> SelectMethods(Type type) =>
			_baseQuery.SelectMethods(type).Select(m => new PartiallySpecifiedParametersMethod(m, _parameters));


		private sealed class PartiallySpecifiedParametersMethod : IMethod
		{
			private readonly IMethod _method;
			private readonly IDictionary<string, object> _specifiedParameters;

			public PartiallySpecifiedParametersMethod(IMethod method, object specifiedParameters)
			{
				var parameters = ToDictionary(specifiedParameters);

				var unknownParameters = GetUnknownParameters(method, parameters);
				if (unknownParameters.Any())
					throw new ArgumentException($"Unknown parameters specified: {string.Join(",", unknownParameters)}", nameof(specifiedParameters));
	
				_method = method;
				_specifiedParameters = parameters;
			}

			public IEnumerable<ParameterInfo> Parameters => _method.Parameters;

			public object Invoke(IEnumerable<object> parameters)
			{
				return _method.Invoke(Parameters.Zip(parameters, ReplaceParameterValue));
			}

			private static Dictionary<string, object> ToDictionary(object specifiedParameters)
			{
				return specifiedParameters.GetType()
					.GetProperties()
					.ToDictionary(
						p => p.Name, 
						p => p.GetValue(specifiedParameters), 
						StringComparer.OrdinalIgnoreCase);
			}

			private static string[] GetUnknownParameters(IMethod method, Dictionary<string, object> parameters)
			{
				var methodParameters = method.Parameters.Select(p => p.Name);
				var unknownParameters = parameters.Keys.Except(methodParameters, StringComparer.OrdinalIgnoreCase).ToArray();
				return unknownParameters;
			}

			private object ReplaceParameterValue(ParameterInfo info, object originalValue)
			{
			    return _specifiedParameters.TryGetValue(info.Name, out var specifiedValue) 
			        ? specifiedValue 
			        : originalValue;
			}
		}
	}
}