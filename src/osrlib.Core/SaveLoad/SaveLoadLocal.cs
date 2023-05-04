using System.IO;
using Newtonsoft.Json;
using osrlib.Core;
using osrlib.Core.Engine;

namespace osrlib.SaveLoad
{
    /// <summary>
    /// Performs <see cref="Adventure"/> save and load operations to and from a local text file.
    /// </summary>
    public static class SaveLoadLocal
    {
        /// <summary>
        /// Saves the specified <see cref="Adventure"/> to the specified file.
        /// </summary>
        /// <param name="adventure">The adventure to save.</param>
        /// <param name="filePath">The full path to the save file.</param>
        /// <returns>True if the save operation was successful, otherwise false.</returns>
        public static bool Save(Adventure adventure, string filePath)
        {
            try
            {
                using (StreamWriter file = File.CreateText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented };
                    serializer.Serialize(file, adventure);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the <see cref="Adventure"/> contained in the specified file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Adventure Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Adventure adventure = (Adventure)serializer.Deserialize(file, typeof(Adventure));
                    return adventure;
                }
            }
            else
            {
                throw new FileNotFoundException("Adventure file not found: " + filePath);
            }

        }
    }
}
