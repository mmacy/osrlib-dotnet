namespace osrlib.Core
{
    /// <summary>
    /// Represents a modifier from a single source, such as an ability or magical bonus or penalty.
    /// </summary>
    /// <remarks>A Modifier is used to adjust another value. For example, a Strength ability score of 18 could add a +3
    /// modifier on to-hit and damage rolls.</remarks>
    /// <example>
    /// Create a modifier for a +1 bonus to attack and damage rolls from a magic sword:
    /// <code>
    /// Weapon magicSword = new Weapon("Long Sword + 1", "A finely crafted sword, its blade dimly glows.",
    ///                                 WeaponType.Melee, new DiceHand(1, DieType.d8));
    /// Modifier attackModifier = new Modifier(magicSword, 1);
    /// Modifier damageModifier = new Modifier(magicSword, 1);
    /// magicSword.AttackModifiers.Add(attackModifier);
    /// magicSword.DamageModifiers.Add(damageModifier);
    /// </code>
    /// </example>
    public class Modifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modifier"/> class with the specified source and value.
        /// </summary>
        /// <param name="modifierSource">The source of the modifier.</param>
        /// <param name="modifierValue">The amount of the modifier, which can be positive or negative.</param>
        public Modifier(object modifierSource, int modifierValue)
        {
            ModifierSource = modifierSource;
            ModifierValue = modifierValue;
        }
        
        /// <summary>
        /// A reference to the source of the modifier.
        /// </summary>
        public object ModifierSource { get; set; }

        /// <summary>
        /// Gets or sets the amount of the modifier. This value can positive (bonus, enchantment) or negative (penalty, curse).
        /// </summary>
        public int ModifierValue { get; init; }
    }
}