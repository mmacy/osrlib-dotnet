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
        /// Initializes a new instance of the <see cref="CharacterClass"/> class with the CharacterClassType.None type.
        /// </summary>
        public CharacterClass() : this(CharacterClassType.None)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterClass"/> class with the specified character class type.
        /// </summary>
        /// <param name="classType">The character class type.</param>
        public CharacterClass(CharacterClassType classType)
        {
            ClassType = classType;
        }

        /// <summary>
        /// Gets or sets the character class type. Default value is the first value of the CharacterClassType enum.
        /// </summary>
        public CharacterClassType ClassType { get; set; }

        /// <summary>
        /// Gets or sets the hit die type for the character class. Default value is DieType.d4.
        /// </summary>
        public DieType HitDieType { get; set; } = DieType.d4;

        /// <summary>
        /// Gets or sets the level of the character class. Default value is 1.
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Gets or sets the experience points attained for this character class. Default value is 0.
        /// </summary>
        public int ExperiencePoints { get; set; } = 0;

        /// <summary>
        /// Gets or sets the experience points needed to level up. Default value is 1500.
        /// </summary>
        public int ExperiencePointsNeeded { get; set; } = 1500;

        /// <summary>
        /// Returns a string representation of the character class type with appropriately placed spaces.
        /// </summary>
        /// <returns>A string representation of the character class type.</returns>
        public override string ToString()
        {
            string className = Enum.GetName(typeof(CharacterClassType), ClassType) ?? "";

            // Add spaces before each uppercase letter except the first one
            return Regex.Replace(className, "(?<!^)([A-Z])", " $1");
        }

    }
}
