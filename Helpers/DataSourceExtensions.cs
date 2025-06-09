using System.Reflection;
using DevExtreme.AspNet.Data;

namespace Backend.Helpers
{
    public static class DataSourceLoadOptionsExtensions
    {
        public static void SetOption(this DataSourceLoadOptionsBase options, string key, string value)
        {
            var property = options.GetType().GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite)
            {
                try
                {
                    object? convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(options, convertedValue);
                }
                catch
                {
                    // Ignore invalid  or log if needed
                }
            }
        }
    }
}
