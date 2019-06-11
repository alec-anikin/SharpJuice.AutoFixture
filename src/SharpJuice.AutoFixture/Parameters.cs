using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
{
    public sealed class Parameters
    {
        private readonly IDictionary<string, object> _parameters;

        public Parameters(object parameters)
        {
            _parameters = ToDictionary(parameters);
        }

        public object GetParameterValue(ParameterInfo info, object defaultValue)
        {
            return _parameters.TryGetValue(info.Name, out var specifiedValue)
                ? specifiedValue
                : defaultValue;
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
                if (parameter.Value == null)
                {
                    if (methodParameter.ParameterType.IsValueType)
                        throw new InvalidOperationException($"Parameter {methodParameter.Name} can't be null.");

                    ++matchedScore;
                    continue;
                }

                var parameterType = parameter.Value.GetType();

                if (methodParameter.ParameterType == parameterType ||
                    methodParameter.ParameterType.IsAssignableFrom(parameterType))
                {
                    ++matchedScore;
                    continue;
                }

                try
                {
                    Convert.ChangeType(parameter.Value, methodParameter.ParameterType);

                    if (methodParameter.ParameterType != typeof(string))
                        matchedScore += 0.01m;
                }
                catch
                {
                    return 0;
                }
            }

            return matchedScore;
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
    }
}