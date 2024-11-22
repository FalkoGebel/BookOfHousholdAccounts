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
        internal static T ReadFromJsonFile<T>(string filePath) where T : new()
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

        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        internal static void WriteToJsonFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter? writer = null;

            try
            {
                FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(fs);
                writer.Write(contentsToWriteToFile);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"File error - {e.Message}");
            }
            finally
            {
                writer?.Close();
            }
        }

        internal static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}