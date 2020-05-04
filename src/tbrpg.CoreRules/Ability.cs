using System.Collections.Generic;
using System.Linq;

namespace tbrpg.CoreRules
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
        /// Gets or sets the collection of <see cref="AbilityModifier"/>s for this Ability.
        /// </summary>
        public List<AbilityModifier> Modifiers { get; set; }

        /// <summary>
        /// Gets the total value of this ability - the BaseValue plus its <see cref="Modifiers"/>.
        /// </summary>
        public int Value => BaseValue + Modifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
    }
}