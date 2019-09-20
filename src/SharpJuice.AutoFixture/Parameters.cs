using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
{
    public sealed class Parameters
    {
        private readonly IDictionary<string, (object value, Type type)> _parameters;

        public Parameters(object parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            _parameters = ToDictionary(parameters);
        }

        public bool Contains(ParameterInfo parameter) =>
            _parameters.ContainsKey(parameter.Name);

        public object GetParameterValue(ParameterInfo info)
        {
            return _parameters.TryGetValue(info.Name, out var specifiedValue)
                ? specifiedValue.value
                : throw new InvalidOperationException("Parameter not found");
        }

        public string[] GetUnknownParameters(IMethod method)
        {
            var methodParameters = method.Parameters.Select(p => p.Name);

            return _parameters.Keys
                .Except(methodParameters, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        public decimal Match(IMethod method)
        {
            var matchedByNameParameters = method.Parameters.Join(_parameters,
                    p => p.Name,
                    p => p.Key,
                    (methodParameter, parameter) => (methodParameter, parameter),
                    StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (matchedByNameParameters.Length != _parameters.Count)
                return 0;

            var matchedScore = 1m;

            foreach (var (methodParameter, parameter) in matchedByNameParameters)
            {
                var (value, type) = parameter.Value;
                var methodParameterType = methodParameter.ParameterType;

                if (methodParameterType == type ||
                    methodParameterType.IsAssignableFrom(type))
                {
                    ++matchedScore;
                    continue;
                }

                if (NumericConverter.CanConvert(value, methodParameterType))
                {
                    matchedScore += 0.01m;
                    continue;
                }

                try
                {
                    Convert.ChangeType(value, methodParameterType);

                    if (methodParameterType != typeof(string))
                        matchedScore += 0.001m;
                }
                catch
                {
                    return 0;
                }
            }

            return matchedScore;
        }

        private static Dictionary<string, (object, Type)> ToDictionary(object specifiedParameters)
        {
            return specifiedParameters.GetType()
                .GetProperties()
                .ToDictionary(
                    p => p.Name,
                    p => (p.GetValue(specifiedParameters), p.PropertyType),
                    StringComparer.OrdinalIgnoreCase);
        }
    }
}