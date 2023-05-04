using System;
using System.Text.RegularExpressions;

namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents a character class in the game.
    /// </summary>
    public class CharacterClass
    {
        /// <summary>
        /// Gets or sets the character class type.
        /// </summary>
        public CharacterClassType Class { get; set; }

        /// <summary>
        /// Gets or sets the hit die type for the character class.
        /// </summary>
        public DieType HitDie { get; set; }

        /// <summary>
        /// Gets or sets the level of the character class.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the experience points attained for this character class.
        /// </summary>
        public int ExperiencePoints { get; set; }

        /// <summary>
        /// Gets or sets the experience points needed to level up.
        /// </summary>
        public int ExperiencePointsNeeded { get; set; }

        /// <summary>
        /// Returns a string representation of the character class type with appropriately placed spaces.
        /// </summary>
        /// <returns>A string representation of the character class type.</returns>
        public override string ToString()
        {
            string className = Enum.GetName(typeof(CharacterClassType), Class) ?? "";
    
            // Add spaces before each uppercase letter except the first one
            return Regex.Replace(className, "(?<!^)([A-Z])", " $1");
        }

    }
}
