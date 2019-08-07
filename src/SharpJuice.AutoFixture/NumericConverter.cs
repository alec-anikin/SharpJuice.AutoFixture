using System;

namespace SharpJuice.AutoFixture
{
    internal static class NumericConverter
    {
        public static bool CanConvert(object value, Type toType)
        {
            if (value == null || value.GetType() == toType)
                return true;

            if (value is IConvertible convertible)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Decimal:
                        return toType == typeof(double) || toType == typeof(double?);
                    case TypeCode.Double:
                        return toType == typeof(decimal) || toType == typeof(decimal?);
                    case TypeCode.Int32:
                        return toType == typeof(decimal) ||
                               toType == typeof(decimal?) ||
                               toType == typeof(double) ||
                               toType == typeof(double?);
                }
            }

            return false;
        }

        public static object Convert(object value, Type toType)
        {
            if (value == null)
                return null;

            if (value.GetType() == toType)
                return value;

            if (!CanConvert(value, toType))
                return value;

            if (toType.IsConstructedGenericType && toType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return System.Convert.ChangeType(value, toType.GetGenericArguments()[0]);

            return System.Convert.ChangeType(value, toType);
        }
    }
}