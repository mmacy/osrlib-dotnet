using System.IO;
using Newtonsoft.Json;

namespace osrlib.Core.Rules;

/// <summary>
/// Provides an implementation of the <see cref="IRulesStorage"/> interface for loading and saving rules data like experience points requirements from/to a JSON file.
/// </summary>
public class JsonRulesStorage : IRulesStorage
{
    private readonly string _jsonFilePath;

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
        try
        {
            var json = File.ReadAllText(_jsonFilePath);
            var experiencePointsRequirements = JsonConvert.DeserializeObject<Dictionary<CharacterClassType, List<int>>>(json);
            return experiencePointsRequirements;
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

        return new Dictionary<CharacterClassType, List<int>>();
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
        try
        {
            var json = JsonConvert.SerializeObject(experiencePointsRequirements, Formatting.Indented);
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