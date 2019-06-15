using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture
{
    public sealed class DeclaringTypeParameterSpecification : IRequestSpecification
    {
        private readonly Type _declaringType;
        private readonly Type _parameterType;

        public DeclaringTypeParameterSpecification(Type declaringType, Type parameterType)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _parameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
        }

        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request is ParameterInfo parameterInfo)
            {
                return _declaringType == parameterInfo.Member.DeclaringType &&
                       _parameterType == parameterInfo.ParameterType;
            }

            return false;
        }
    }
}