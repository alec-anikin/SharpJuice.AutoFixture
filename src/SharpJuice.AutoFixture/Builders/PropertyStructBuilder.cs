using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture.Builders
{
	public sealed class PropertyStructBuilder<T> : ISpecimenBuilder where T : struct
	{
		private readonly string _propertyName;
		private readonly T _value;

		public PropertyStructBuilder(string propertyName, T value)
		{
			_propertyName = propertyName;
			_value = value;
		}

		public object Create(object request, ISpecimenContext context)
		{
			var pi = request as PropertyInfo;
			if (pi != null &&
			    (pi.PropertyType == typeof(T) || pi.PropertyType == typeof(T?)) &&
			    pi.Name == _propertyName)
			{
				return _value;
			}

			return new NoSpecimen();
		}
	}
}