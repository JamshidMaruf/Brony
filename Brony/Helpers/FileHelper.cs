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

   
}
