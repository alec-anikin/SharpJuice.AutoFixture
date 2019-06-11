using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
{
    public sealed class BestFitConstructorQuery : IMethodQuery
    {
        private readonly Parameters _parameters;

        public BestFitConstructorQuery(Parameters parameters)
        {
            _parameters = parameters;
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var constructors = type.GetTypeInfo().GetConstructors()
                .Select(c => (info: c, parameters: c.GetParameters()))
                .Where(c => c.parameters.All(p => p.ParameterType != type))
                .OrderBy(c => c.parameters.Length)
                .Select(c => new ConstructorMethod(c.info));

            var bestFit = constructors.Select(method => (method, score: _parameters.Match(method)))
                .Where(i => i.score > 0)
                .OrderByDescending(i => i.score)
                .Select(i => i.method)
                .ToArray();

            if (bestFit.Length == 0)
                throw new InvalidOperationException($"There is no best fit constructor of {type}.");

            return bestFit;
        }
    }
}