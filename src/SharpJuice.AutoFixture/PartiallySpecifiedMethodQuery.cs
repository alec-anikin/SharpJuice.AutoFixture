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

        public IEnumerable<IMethod> SelectMethods(Type type) =>
            _baseQuery.SelectMethods(type).Select(m => new PartiallySpecifiedParametersMethod(m, _parameters));


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

            public IEnumerable<ParameterInfo> Parameters => _method.Parameters;

            public object Invoke(IEnumerable<object> parameters)
            {
                return _method.Invoke(Parameters.Zip(
                    parameters,
                    (param, value) => NumericConverter.Convert(
                        _specifiedParameters.GetParameterValue(param, value),
                        param.ParameterType)));
            }
        }
    }
}