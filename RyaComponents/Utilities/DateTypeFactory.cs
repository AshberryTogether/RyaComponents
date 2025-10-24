using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Utilities
{
    internal static class DateTypeFactory
    {
        internal static DateType GenerateDefaultValue<T>()
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(DateOnly):
                    return new DateOnlyDateType(DateOnly.MinValue);
                case Type t when t == typeof(DateOnly?):
                    return new DateOnlyNullableDateType((DateOnly?)DateOnly.MinValue);
                case Type t when t == typeof(DateTime):
                    return new DateTimeDateType(DateTime.MinValue);
                case Type t when t == typeof(DateTime?):
                    return new DateTimeNullableDateType((DateTime?)DateTime.MinValue);
                default:
                    throw new NotSupportedException($"Type {typeof(T).Name} is not supported for DateType creation.");
            }
        }
        internal static bool TryCreateDateTypeInstanceFromObject<T>(T? obj, out DateType result)
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(DateOnly):
                    result = new DateOnlyDateType((DateOnly)(object)obj);
                    return true;
                case Type t when t == typeof(DateOnly?):
                    result = new DateOnlyNullableDateType(obj as DateOnly?);
                    return true;
                case Type t when t == typeof(DateTime):
                    result = new DateTimeDateType((DateTime)(object)obj);
                    return true;
                case Type t when t == typeof(DateTime?):
                    result = new DateTimeNullableDateType(obj as DateTime?);
                    return true;
                default:
                    throw new NotSupportedException($"Type {typeof(T).Name} is not supported for DateType creation.");
            }
        }
        internal static DateType CreateDateTypeInstance<T>()
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(DateOnly):
                    return new DateOnlyDateType();
                case Type t when t == typeof(DateOnly?):
                    return new DateOnlyNullableDateType();
                case Type t when t == typeof(DateTime):
                    return new DateTimeDateType();
                case Type t when t == typeof(DateTime?):
                    return new DateTimeNullableDateType();
                default:
                    throw new NotSupportedException($"Type {typeof(T).Name} is not supported for DateType creation.");
            }
        }
        internal static DateType CreateDateTypeInstance(Type type)
        {
            switch (type)
            {
                case Type t when t == typeof(DateOnly):
                    return new DateOnlyDateType();
                case Type t when t == typeof(DateOnly?):
                    return new DateOnlyNullableDateType();
                case Type t when t == typeof(DateTime):
                    return new DateTimeDateType();
                case Type t when t == typeof(DateTime?):
                    return new DateTimeNullableDateType();
                default:
                    throw new NotSupportedException($"Type {type.Name} is not supported for DateType creation.");
            }
        }
    }
}
