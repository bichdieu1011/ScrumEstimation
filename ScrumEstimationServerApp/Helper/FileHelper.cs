using Newtonsoft.Json;

namespace ScrumEstimationServerApp.Helper
{
    //public class FileHelper
    //{
    //    private static readonly object _lock = new object();

    //    public static T ReadFile<T>(string filePath)
    //    {
    //        try
    //        {
    //            var json = File.ReadAllText(filePath);
    //            if (!string.IsNullOrEmpty(json))
    //                return JsonConvert.DeserializeObject<T>(json);
    //            return default(T);
    //        }
    //        catch (Exception ex)
    //        {
    //            return default(T);
    //        }
    //    }

    //    public static void WriteFile(string jsonValue, string filePath)
    //    {
    //        try
    //        {
    //            lock (_lock)
    //            {
    //                File.WriteAllText(filePath, jsonValue);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }
    //}
}