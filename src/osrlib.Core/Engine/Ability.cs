namespace osrlib.Core.Engine
{
    /// <summary>
    /// A <see cref="Being"/> has one or more abilities (strength, dexterity, intelligence, etc.) that are
    /// referenced when calculating the success of a <see cref="GameAction"/>.
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class with the specified <paramref name="type"/>.
        /// The <see cref="Base"/> is rolled automatically upon initialization.
        /// </summary>
        /// <param name="type">The type of the ability.</param>
        public Ability(AbilityType type)
        {
            Type = type;
            RollAbilityScore();
        }
        
        /// <summary>
        /// Gets or sets the type (strength, dexterity, intelligence, etc.) of the ability.
        /// </summary>
        public AbilityType Type { get; set; }

        /// <summary>
        /// Gets or sets the base value of the ability. This is raw "rolled" value set with <see cref="RollAbilityScore"/>,
        /// without any modifiers.
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Modifier"/>s that adjust the score of the ability.
        /// </summary>
        /// <remarks>These are the modifiers that grant a bonus or impose a penalty on the ability value.
        /// For example, enhancements from potions or penalties from curse-type spells.
        /// </remarks>
        public List<Modifier> ScoreModifiers { get; set; } = new();

        /// <summary>
        /// Gets the bonus granted or penalty imposed by the ability.
        /// </summary>
        /// <remarks>
        /// This value is based on the ability's <see cref="Score"/>, which is the sum of its <see cref="Base"/>
        /// and <see cref="ScoreModifiers"/>, if any.
        /// For example, for an Ability score of 18, this method returns 3 (that is, a +3). Use this method when
        /// adjusting dice rolls typically affected by ability scores, for example attack and damage rolls.
        /// </remarks>
        /// <returns>The bonus or penalty value of the ability.</returns>
        public int GetModifierValue()
        {
            return Base switch
            {
                int val when Score >= 20 =>  4,
                int val when Score >= 18 =>  3,
                int val when Score >= 16 =>  2,
                int val when Score >= 13 =>  1,
                int val when Score >=  9 =>  0,
                int val when Score >=  6 => -1,
                int val when Score >=  4 => -2,
                int val when Score >=  3 => -3,
                _                        =>  0,
            };
        }

        /// <summary>
        /// Rolls and sets the <see cref="Base"/> of the ability.
        /// </summary>
        /// <returns>The rolled score.</returns>
        public int RollAbilityScore()
        {
            // TODO - Don't hard code the die roll used for generating the ability score.
            // TODO - Add support for discarding the lowest die roll in the DiceHand so we can do "4d6 drop lowest."
            
            // Roll the base value
            DiceRoll roll = new DiceRoll(new DiceHand(3, DieType.d6));
            this.Base = roll.RollDice();

            return this.Base;
        }

        /// <summary>
        /// Gets the total value of the ability, the sum of its <see cref="Base"/> and <see cref="ScoreModifiers"/>, if any.
        /// </summary>
        public int Score
        {
            get
            {
                int mods = 0;
                if (this.ScoreModifiers.Any())
                {
                    mods += ScoreModifiers.Select(m => m.Value).Aggregate((x, y) => x + y);
                }

                return Base + mods;
            }
        }

        /// <summary>
        /// Gets a string representation of the ability. For example, <c>Strength: 18 (17 + 1)</c>.
        /// </summary>
        /// <returns>Single-line text representation of the ability.</returns>
        public override string ToString()
        {
            int mods = 0;

            if (ScoreModifiers.Any())
            {
                mods += ScoreModifiers.Select(m => m.Value).Aggregate((x, y) => x + y);
            }

            char modChar = mods >= 0 ? '+' : '-';

            // Format and return the string. Example: "Strength: 18 (17 + 1)"
            return $"{Type}: {Score} ({Base} {modChar} {mods})";
        }
    }
}