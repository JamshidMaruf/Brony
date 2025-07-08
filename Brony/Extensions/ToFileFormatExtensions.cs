using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Brony.Domain;
using Brony.Models;

namespace Brony.Extensions
{
    internal static class ToFileFormatExtensions
    {
        public static List<string> ToFileFormat<T>(List<T> models)
        {
            List<string> modelsInStringFormat = new List<string>();

            foreach (var model in models)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                List<string> values = new List<string>();

                foreach (var prop in properties)
                {
                    object value = prop.GetValue(model);
                    values.Add(value != null ? value.ToString() : "null");
                }

                string line = string.Join(",", values);
                modelsInStringFormat.Add(line);
            }

            return modelsInStringFormat;
        }
    }
}
