using Newtonsoft.Json;

namespace osrlib.Core
{
    /// <summary>
    /// A <see cref="Being"/> has one or more abilities (strength, dexterity, intelligence, etc.) that are
    /// referenced when calculating the success of a <see cref="GameAction"/>.
    /// </summary>
    public class Ability
    {
        [JsonConstructor]
        private Ability() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class with the specified <paramref name="type"/>.
        /// The <see cref="BaseValue"/> is rolled automatically upon initialization.
        /// </summary>
        /// <param name="type">The type of the ability.</param>
        public Ability(AbilityType type)
        {
            Type = type;
            BaseValue = RollAbilityScore();
        }
        
        /// <summary>
        /// Gets or sets the type (strength, dexterity, intelligence, etc.) of this Ability.
        /// </summary>
        [JsonProperty(nameof(Type))]
        public AbilityType Type { get; set; }

        /// <summary>
        /// Gets or sets the base value of this Ability. This is raw "rolled" value set with <see cref="RollAbilityScore"/>,
        /// without any modifiers.
        /// </summary>
        [JsonProperty(nameof(BaseValue))]
        public int BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Modifier"/>s that adjust the score of this Ability.
        /// </summary>
        /// <remarks>These are the modifiers that grant a bonus or impose a penalty on the ability value.
        /// For example, enhancements from potions or penalties from curse-type spells.
        /// </remarks>
        [JsonProperty(nameof(ScoreModifiers))]
        public List<Modifier> ScoreModifiers { get; set; } = new();

        /// <summary>
        /// Gets the bonus granted or penalty imposed by the Ability.
        /// </summary>
        /// <remarks>
        /// This value is based on the Ability's <see cref="Value"/>, which is the sum of its <see cref="BaseValue"/>
        /// and <see cref="ScoreModifiers"/>, if any.
        /// For example, for an Ability score of 18, this method returns 3 (that is, a +3). Use this method when
        /// adjusting dice rolls typically affected by ability scores, for example attack and damage rolls.
        /// </remarks>
        /// <returns>The bonus or penalty value of the Ability.</returns>
        public int GetModifier()
        {
            return BaseValue switch
            {
                int val when Value >= 20 =>  4,
                int val when Value >= 18 =>  3,
                int val when Value >= 16 =>  2,
                int val when Value >= 13 =>  1,
                int val when Value >=  9 =>  0,
                int val when Value >=  6 => -1,
                int val when Value >=  4 => -2,
                int val when Value >=  3 => -3,
                _                        =>  0,
            };
        }

        /// <summary>
        /// Rolls and sets the <see cref="BaseValue"/> of the Ability.
        /// </summary>
        /// <returns>The rolled score.</returns>
        public int RollAbilityScore()
        {
            // Roll the base value
            DiceRoll roll = new DiceRoll(new DiceHand(3, DieType.d6));
            this.BaseValue = roll.RollDice();

            return this.BaseValue;
        }

        /// <summary>
        /// Gets the total value of the Ability, the sum of its <see cref="BaseValue"/> and <see cref="ScoreModifiers"/>, if any.
        /// </summary>
        public int Value
        {
            get
            {
                int mods = 0;
                if (this.ScoreModifiers.Any())
                {
                    mods += ScoreModifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
                }

                return BaseValue + mods;
            }
        }

        /// <summary>
        /// Gets a string representation of the Ability. For example, <c>Strength: 18 (17 + 1)</c>.
        /// </summary>
        /// <returns>Single-line text representation of the Ability.</returns>
        public override string ToString()
        {
            int mods = 0;

            if (ScoreModifiers.Any())
            {
                mods += ScoreModifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
            }

            char modChar = mods >= 0 ? '+' : '-';

            // Format and return the string. Example: "Strength: 18 (17 + 1)"
            return $"{Type}: {Value} ({BaseValue} {modChar} {mods})";
        }
    }
}