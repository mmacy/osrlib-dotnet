namespace osrlib.Core.Rules;

/// <summary>
/// Provides an interface for loading and saving rules data, such as experience points requirements, from/to a storage format.
/// </summary>
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
public interface IRulesStorage
{
    /// <summary>
    /// Loads experience points requirements for each character class from the storage format.
    /// </summary>
    /// <returns>A dictionary containing character class types as keys and lists of integers representing experience points requirements as values.</returns>
    Dictionary<CharacterClassType, List<int>> LoadExperiencePointsRequirements();

    /// <summary>
    /// Saves experience points requirements for each character class to the storage format.
    /// </summary>
    /// <param name="experiencePointsRequirements">A dictionary containing character class types as keys and lists of integers representing experience points requirements as values.</param>
    void SaveExperiencePointsRequirements(Dictionary<CharacterClassType, List<int>> experiencePointsRequirements);
}