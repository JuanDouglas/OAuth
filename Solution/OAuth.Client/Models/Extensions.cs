using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client.Models
{
    public static class Extensions
    {
        public static void AllPropertiesLocalDate(this object obj)
        {
            Type objType = obj.GetType();

            foreach (PropertyInfo property in objType.GetProperties())
            {
                if (property.PropertyType.FullName == typeof(DateTime).FullName)
                {
                    if (property.CanWrite)
                    {
                        DateTime dateTime = (DateTime)property.GetValue(obj);
                        property.SetValue(obj, dateTime.ToLocalTime());
                    }
                }
            }

        }
    }
}
