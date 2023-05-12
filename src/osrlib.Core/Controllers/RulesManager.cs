using osrlib.Core.Engine;
using osrlib.Core.Rules;

namespace osrlib.Controllers
{
    /// <summary>
    /// The RulesManager singleton handles ruleset loading and management.
    /// </summary>
    /// <remarks>The RulesManager singleton should be accessed only via the <see cref="RulesManager.Instance"/> property.</remarks>
    public class RulesManager
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="RulesManager"/>.
        /// </summary>
        public static RulesManager Instance { get; } = new RulesManager();

        private Dictionary<CharacterClassType, JsonRulesStorage.CharacterClassData> characterClassDataDictionary;

        /// <summary>
        /// Gets the character class data for the given character class type.
        /// </summary>
        public JsonRulesStorage.CharacterClassData this[CharacterClassType classType]
        {
            get
            {
                LoadCharacterClassDataIfNeeded();
                return characterClassDataDictionary[classType];
            }
        }

        /// <summary>
        /// Creates a new instance of the RulesManager class.
        /// </summary>
        /// <remarks>Private constructor to enforce singleton usage via the <see cref="RulesManager.Instance"/> property.</remarks>
        private RulesManager() { }

        private void LoadCharacterClassDataIfNeeded()
        {
            if (characterClassDataDictionary == null)
            {
                var jsonFilePath = "character_class_data.json";
                var jsonRulesStorage = new JsonRulesStorage(jsonFilePath);

                // Load all character class data
                //characterClassDataDictionary = jsonRulesStorage.LoadCharacterClassData();
                var thing = jsonRulesStorage.LoadSavingThrowValues();
            }
        }
    }
}
