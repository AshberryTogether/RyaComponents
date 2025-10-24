using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace RyaComponents.Utilities
{
    public static class DataBindingHelper
    {
        public static object? GetRecordProperty(object? record, string propertyName)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record is DataRowView row)
            {
                return row[propertyName];
            }

            PropertyInfo? info = record.GetType().GetProperty(propertyName);
            if (info is null)
            {
                throw new ArgumentNullException($"`{record.GetType()}` does not have a property called `{propertyName}`");
            }
            return info.GetValue(record);
        }

        public static object? GetRecordFromBoundDataSource(object? dataSource, int index)
        {
            if (dataSource is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().ElementAt(index);
            }
            if (dataSource is IListSource listSource)
            {
                return listSource.GetList()[index];
            }
            return null;
        }

        public static int GetCountFromBoundObject(object? boundObject)
        {
            if (boundObject is ICollection collection)
            {
                return collection.Count;
            }
            if (boundObject is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Count();
            }
            if (boundObject is IListSource listSource)
            {
                return listSource.GetList().Count;
            }
            return -1;
        }
    }
}
