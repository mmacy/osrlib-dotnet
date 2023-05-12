using System.IO;
using Newtonsoft.Json;
using osrlib.Core.Engine;

namespace osrlib.Core.Rules;

/// <summary>
/// Provides an implementation of the <see cref="IRulesStorage"/> interface for loading and saving rules data like experience points requirements from/to a JSON file.
/// </summary>
public class JsonRulesStorage : IRulesStorage
{
    private readonly string _jsonFilePath;
    
    public class CharacterClassData
    {
        public List<int> ExperiencePoints { get; set; }
        public Dictionary<SavingThrowType, int> SavingThrows { get; set; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonRulesStorage"/> class with the specified JSON file path.
    /// </summary>
    /// <param name="jsonFilePath">The path to the JSON file containing experience points requirements data.</param>
    /// <example>
    /// <code>
    /// var jsonFilePath = "experience_points_requirements.json";
    /// var jsonRulesStorage = new JsonRulesStorage(jsonFilePath);
    /// </code>
    /// </example>
    public JsonRulesStorage(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }

    /// <summary>
    /// Loads experience points requirements for each character class from the JSON file.
    /// </summary>
    /// <returns>A dictionary containing character class types as keys and lists of integers representing experience points requirements as values.</returns>
    /// <example>
    /// <code>
    /// var jsonFilePath = "experience_points_requirements.json";
    /// var jsonRulesStorage = new JsonRulesStorage(jsonFilePath);
    ///
    /// // Load experience points requirements
    /// var experiencePointsRequirements = jsonRulesStorage.LoadExperiencePointsRequirements();
    /// </code>
    /// </example>
    public Dictionary<CharacterClassType, List<int>> LoadExperiencePointsRequirements()
    {
        var data = LoadData();
        return data.ToDictionary(x => x.Key, x => x.Value.ExperiencePoints);
    }

    /// <summary>
    /// Saves experience points requirements for each character class to the JSON file.
    /// </summary>
    /// <param name="experiencePointsRequirements">A dictionary containing character class types as keys and lists of integers representing experience points requirements as values.</param>
    /// <example>
    /// <code>
    /// var jsonFilePath = "experience_points_requirements.json";
    /// var jsonRulesStorage = new JsonRulesStorage(jsonFilePath);
    ///
    /// // Load experience points requirements
    /// var experiencePointsRequirements = jsonRulesStorage.LoadExperiencePointsRequirements();
    ///
    /// // Save experience points requirements after modifying them
    /// experiencePointsRequirements[CharacterClassType.Cleric][1] = 1600;
    /// jsonRulesStorage.SaveExperiencePointsRequirements(experiencePointsRequirements);
    /// </code>
    /// </example>
    public void SaveExperiencePointsRequirements(Dictionary<CharacterClassType, List<int>> experiencePointsRequirements)
    {
        var data = LoadData();
        foreach (var item in experiencePointsRequirements)
        {
            data[item.Key].ExperiencePoints = item.Value;
        }
        SaveData(data);
    }
    
    public Dictionary<CharacterClassType, Dictionary<SavingThrowType, int>> LoadSavingThrowValues()
    {
        var data = LoadData();
        return data.ToDictionary(x => x.Key, x => x.Value.SavingThrows);
    }


    public void SaveSavingThrowValues(Dictionary<CharacterClassType, Dictionary<SavingThrowType, int>> savingThrowValues)
    {
        var data = LoadData();
        foreach (var item in savingThrowValues)
        {
            data[item.Key].SavingThrows = item.Value;
        }
        SaveData(data);
    }
    
    private Dictionary<CharacterClassType, CharacterClassData> LoadData()
    {
        try
        {
            var json = File.ReadAllText(_jsonFilePath);
            var data = JsonConvert.DeserializeObject<Dictionary<CharacterClassType, CharacterClassData>>(json);
            return data ?? new Dictionary<CharacterClassType, CharacterClassData>();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An I/O error occurred while reading the file: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"An error occurred while deserializing the JSON data: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access to the file is denied: {ex.Message}");
        }

        return new Dictionary<CharacterClassType, CharacterClassData>();
    }
    
    private void SaveData(Dictionary<CharacterClassType, CharacterClassData> data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_jsonFilePath, json);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An I/O error occurred while writing the file: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"An error occurred while serializing the JSON data: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access to the file is denied: {ex.Message}");
        }
    }
}