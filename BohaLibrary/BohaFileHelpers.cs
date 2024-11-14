using Newtonsoft.Json;

namespace BohaLibrary
{
    internal static class BohaFileHelpers
    {
        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader? reader = null;

            try
            {
                FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                reader = new StreamReader(fs);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents) ?? new();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"File error - {e.Message}");
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}