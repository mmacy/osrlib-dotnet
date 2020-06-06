using System.Collections.Generic;
using System.Linq;
using osrlib.Dice;

namespace osrlib.Core
{
    /// <summary>
    /// A Being has one or more Abilities. An Ability is used when performing checks to
    /// test success of a GameAction as well as to modify DiceRolls via the Ability's Modifiers.
    /// </summary>
    public class Ability
    {
        /// <summary>
        /// Gets or sets the type of this Ability.
        /// </summary>
        public AbilityType Type { get; set; }

        /// <summary>
        /// Gets or sets the base value of this Ability.
        /// </summary>
        public int BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Modifier"/>s that adjust the score of this Ability. These are
        /// the modifiers that grant a bonus or a impose a penalty on the modifier value. For example, enhancements
        /// from potions or penalties from curse-type spells.
        /// </summary>
        public List<Modifier> ScoreModifiers { get; set; } = new List<Modifier>();

        /// <summary>
        /// Gets the bonus granted or penalty imposed by the Ability.
        /// </summary>
        /// <remarks>
        /// This value is based on the Ability's <see cref="BaseValue"/> and its <see cref="ScoreModifiers"/>.
        /// For example, for an Ability score of 18, this method returns 3 (that is, a +3). Use this method when
        /// adjusting dice rolls typically affected by ability scores, for example attack (to-hit) and damage rolls.
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
        /// Rolls and sets the score for this ability. This method does not modify the <see cref="ScoreModifiers"/>
        /// collection.
        /// </summary>
        /// <returns>The rolled score, not including <see cref="ScoreModifiers"/>.</returns>
        public int RollAbilityScore()
        {
            // Roll the base value
            DiceRoll roll = new DiceRoll(new DiceHand(3, DieType.d6));
            this.BaseValue = roll.RollDice();

            return this.BaseValue;
        }

        /// <summary>
        /// Gets the total value of this ability - the BaseValue plus its <see cref="ScoreModifiers"/>.
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
        /// Gets the string representation of the Ability.
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