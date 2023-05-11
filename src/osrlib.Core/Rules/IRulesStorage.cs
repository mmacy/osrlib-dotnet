using osrlib.Core.Engine;

namespace osrlib.Core.Rules;

/// <summary>
/// Provides an interface for loading and saving rules data, such as experience points requirements, from/to persistent storage.
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
    /// Loads experience points requirements for each character class from persistent storage.
    /// </summary>
    /// <returns>A dictionary containing character class types as keys and lists of integers representing experience points requirements as values.</returns>
    Dictionary<CharacterClassType, List<int>> LoadExperiencePointsRequirements();

    /// <summary>
    /// Saves experience points requirements for each character class to persistent storage.
    /// </summary>
    /// <param name="experiencePointsRequirements">A dictionary containing character class types as keys and lists of integers representing experience points requirements as values.</param>
    void SaveExperiencePointsRequirements(Dictionary<CharacterClassType, List<int>> experiencePointsRequirements);
    
    /// <summary>
    /// Loads saving throw values for each character class from persistent storage.
    /// </summary>
    /// <returns>A dictionary containing character class types as keys and dictionaries representing saving throw values as values. Each inner dictionary contains saving throw types as keys and integers as values.</returns>
    Dictionary<CharacterClassType, Dictionary<SavingThrowType, int>> LoadSavingThrowValues();

    /// <summary>
    /// Saves saving throw values for each character class to persistent storage.
    /// </summary>
    /// <param name="savingThrowValues">A dictionary containing character class types as keys and dictionaries representing saving throw values as values. Each inner dictionary contains saving throw types as keys and integers as values.</param>
    void SaveSavingThrowValues(Dictionary<CharacterClassType, Dictionary<SavingThrowType, int>> savingThrowValues);
}