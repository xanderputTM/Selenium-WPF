using System;
using System.IO;
using System.Text.Json;

namespace Selenium_WPF
{
    internal class Utils
    {
        public static void writeToJson(object? data, String filename)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(@$"C:\Users\Xander\source\repos\Selenium WPF\files\{filename}", jsonString);
        }

    }
}
