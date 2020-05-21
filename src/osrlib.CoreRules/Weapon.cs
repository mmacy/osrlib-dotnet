using System.Collections.Generic;
using System.Linq;
using osrlib.Dice;

namespace osrlib.CoreRules
{
    /// <summary>
    /// A weapon is a melee or ranged martial weapon, or an offensive spell.
    /// </summary>
    public class Weapon
    {
        /// <summary>
        /// Gets or sets the name of the weapon or spell.
        /// </summary>
        public string Name { get; set; } = "Generic Weapon";

        /// <summary>
        /// Gets or sets the weapon or spell's description.
        /// </summary>
        public string Description { get; set; } = "This is a generic weapon.";

        /// <summary>
        /// Gets or sets the weapon type. An offensive spell is considered a weapon.
        /// </summary>
        public WeaponType Type { get; set; } = WeaponType.Melee;

        /// <summary>
        /// Gets or sets the die rolled when calculating the weapon or spell's damage. Default: <c>1d4</c>.
        /// </summary>
        public DiceHand DamageDie { get; set; } = new DiceHand(1, DieType.d4);

        /// <summary>
        /// Rolls a <see cref="DieType.d20"/>, sums any modifiers, and returns the result.
        /// </summary>
        /// <remarks>
        /// The modifier is typically supplied by <see cref="Ability"/> bonuses/penalties or
        /// enchantments/curses applied to the <see cref="Being"/> wielding the weapon or spell.
        /// </remarks>
        /// <param name="modifier">The bonus or penalty to apply to the roll. Default: <c>0</c>.</param>
        /// <returns>The to-hit roll.</returns>
        public int GetAttackRoll(int modifier = 0)
        {
            DiceRoll toHitRoll = new DiceRoll(new DiceHand(1, DieType.d20));

            // Sum the weapon's to-hit modifiers
            if (AttackModifiers.Any())
            {
                modifier += this.AttackModifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
            }

            toHitRoll.AddModifier(modifier);

            return toHitRoll.RollDice();
        }

        /// <summary>
        /// Rolls the weapon's <see cref="DamageDie"/>, sums any modifiers, and returns the result.
        /// </summary>
        /// <remarks>
        /// The modifier is typically supplied by <see cref="Ability"/> bonuses/penalties or
        /// enchantments/curses applied to the <see cref="Being"/> wielding the weapon or spell.
        /// </remarks>
        /// <param name="modifier">The bonus or penalty to apply to the roll. Default: <c>0</c>.</param>
        /// <returns>The amount of damage rolled.</returns>
        public int GetDamageRoll(int modifier = 0)
        {
            DiceRoll damageRoll = new DiceRoll(this.DamageDie);

            // Sum the weapon's damage modifiers
            if (DamageModifiers.Any())
            {
                modifier += DamageModifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
            }

            damageRoll.AddModifier(modifier);

            return damageRoll.RollDice();
        }

        /// <summary>
        /// Gets or sets the to-hit enchantments (bonuses) or curses (penalties) for the weapon or spell.
        /// </summary>
        /// <remarks>
        /// AttackModifiers and DamageModifiers are typically static for the life of the weapon. For example,
        /// to make a Long Sword +1, add both an AttackModifier and DamageModifier with ModifierValue of <c>1</c>.
        ///
        /// Don't add a Being's <see cref="Ability"/> modifiers to this collection - instead, pass those when you
        /// call <see cref="GetAttackRoll"/>.
        /// </remarks>
        public List<Modifier> AttackModifiers { get; set; } = new List<Modifier>();

        /// <summary>
        /// Gets or sets the damage enchantments (bonuses) or curses (penalties) for the weapon or spell.
        /// </summary>
        /// <remarks>
        /// AttackModifiers and DamageModifiers are typically static for the life of the weapon. For example,
        /// to make a Long Sword +1, add both an AttackModifier and DamageModifier with ModifierValue of <c>1</c>.
        /// Don't add a Being's <see cref="Ability"/> modifiers to this collection - instead, pass those when you
        /// call <see cref="GetDamageRoll"/>.
        /// </remarks>
        public List<Modifier> DamageModifiers { get; set; } = new List<Modifier>();

        /// <summary>
        /// Gets the string representation of the Weapon.
        /// </summary>
        /// <returns>Single-line text representation of the Weapon.</returns>
        public override string ToString() => this.Name;
    }
}
