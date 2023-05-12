using System;
using System.Linq;
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
        public CharacterClass() : this(CharacterClassType.Fighter)
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
        /// Gets or sets the experience points earned for this character class. Default value is 0.
        /// </summary>
        public int ExperiencePoints { get; set; } = 0;
        
        /// <summary>
        /// Gets the experience points needed for the character to level up.
        /// </summary>
        /// <remarks>
        /// The number of experience points required to level up is based on the
        /// <see cref="ExperiencePointsRequirements"/> and <see cref="Level"/> of the character.
        /// 
        /// If the character's <see cref="Level"/> is within the bounds of the <see cref="ExperiencePointsRequirements"/>
        /// array, the corresponding value from the array is used.
        /// 
        /// If the character's <see cref="Level"/> is beyond the bounds of the <see cref="ExperiencePointsRequirements"/>
        /// array, the requirement is calculated as 1.33 times the last defined value in the array. This calculation is
        /// repeated for each level beyond the last-defined. For example, if the last defined requirement is 750000 and
        /// the character's level is two levels beyond the last defined one, the requirement would be (750000 * 1.33) * 1.33.
        /// 
        /// This provides a linear progression for levels beyond those defined in the <see cref="ExperiencePointsRequirements"/>
        /// array. The calculated requirement is rounded down to the nearest whole number.
        /// </remarks>
        public int ExperiencePointsNeeded
        {
            get
            {
                if (ExperiencePointsRequirements == null || ExperiencePointsRequirements.Length == 0)
                {
                    return 0; // Default value
                }

                if (Level <= ExperiencePointsRequirements.Length)
                {
                    // Calculate the requirement based on the defined XP requirements.
                    return ExperiencePointsRequirements[Level - 1]; //TODO: Move this to a RulesManager lookup
                }
                else
                {
                    // Calculate the requirement as 1.33 times the last-defined requirement, repeated for
                    // each level beyond the last defined value in the XP requirements array.
                    double requirement = ExperiencePointsRequirements[^1];
                    for (int i = ExperiencePointsRequirements.Length; i < Level; i++)
                    {
                        requirement *= 1.33;
                    }

                    // NOTE: Casting to int rounds the XP requirement value down to the next whole number.
                    return (int)requirement;
                }
            }
        }

        
        /// <summary>
        /// Gets the experience point progression for the character class.
        /// </summary>
        public int[] ExperiencePointsRequirements { get; set; } = //TODO: Move this to a RulesManager lookup
            { 0, 2000, 4000, 8000, 16000, 32000, 64000, 125000, 250000, 500000, 750000 };
        
        /// <summary>
        /// Gets or sets the saving throws for the character class.
        /// </summary>
        public List<SavingThrow> SavingThrows { get; set; } = new();

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
