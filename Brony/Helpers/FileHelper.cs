using System.Reflection;

namespace Brony.Helpers;

public static class FileHelper
{
    public static string ReadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close(); // Ensure the file exists
        }

        return File.ReadAllText(filePath);
    }

    public static void WriteToFile<T>(string filePath, List<string> content)
    {
        File.WriteAllLines(filePath, content);
    }

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
