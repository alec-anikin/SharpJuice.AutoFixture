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
        private readonly Parameters _parameters;

        public PartiallySpecifiedMethodQuery(IMethodQuery baseQuery, Parameters parameters)
        {
            _baseQuery = baseQuery;
            _parameters = parameters;
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            return _baseQuery.SelectMethods(type)
                .Select(m => new PartiallySpecifiedParametersMethod(m, _parameters));
        }
        
        private sealed class PartiallySpecifiedParametersMethod : IMethod
        {
            private readonly IMethod _method;
            private readonly Parameters _specifiedParameters;

            public PartiallySpecifiedParametersMethod(IMethod method, Parameters specifiedParameters)
            {
                var unknownParameters = specifiedParameters.GetUnknownParameters(method);

                if (unknownParameters.Length != 0)
                    throw new ArgumentException($"Unknown parameters specified: {string.Join(",", unknownParameters)}",
                        nameof(specifiedParameters));

                _method = method;
                _specifiedParameters = specifiedParameters;
            }

            public IEnumerable<ParameterInfo> Parameters =>
                _method.Parameters.Where(p => !_specifiedParameters.Contains(p));

            public object Invoke(IEnumerable<object> parameters) =>
                _method.Invoke(GetParameters(parameters));

            private IEnumerable<object> GetParameters(IEnumerable<object> generatedParameters)
            {
                var queue = new Queue<object>(generatedParameters);

                foreach (var parameter in _method.Parameters)
                {
                    if (_specifiedParameters.Contains(parameter))
                        yield return NumericConverter.Convert(
                            _specifiedParameters.GetParameterValue(parameter),
                            parameter.ParameterType);
                    else
                        yield return queue.Dequeue();
                }
            }
        }
    }
}