using System;

namespace AN.Integration.Database.Query
{
    public static class DbValue
    {
        public static object Convert(object value)
        {
            object dbValue = value switch
            {
                null => "null", 
                string s => $"'{s}'",
                int i => i,
                Guid g => $"'{g}'",
                DateTime dt => dt.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                _ => throw new Exception($"Type {value.GetType().Name} is not supported")
            };

            return dbValue;
        }
    }
}