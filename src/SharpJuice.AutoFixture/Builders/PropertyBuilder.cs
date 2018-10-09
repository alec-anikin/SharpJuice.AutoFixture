using System.Reflection;
using AutoFixture.Kernel;

namespace SharpJuice.AutoFixture.Builders
{
	public sealed class PropertyBuilder<TProperty> : ISpecimenBuilder
	{
		private readonly string _propertyName;
		private readonly TProperty _value;

		public PropertyBuilder(string propertyName, TProperty value)
		{
			_propertyName = propertyName.ToUpper();
			_value = value;
		}

		public object Create(object request, ISpecimenContext context)
		{
			var pi = request as PropertyInfo;

			if (pi != null &&
			    pi.PropertyType == typeof(TProperty) &&
			    pi.Name.ToUpper() == _propertyName)
			{
				return _value;
			}

			return new NoSpecimen();
		}
	}
}