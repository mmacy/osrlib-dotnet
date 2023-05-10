namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents a modifier from a single source, such as an ability or magical bonus or penalty.
    /// </summary>
    /// <remarks>
    /// Use a modifier to adjust another value. For example, a Strength ability score of 18 could add a +3
    /// modifier on to-hit and damage rolls.
    /// </remarks>
    /// <example>
    /// Create a modifier for a +1 bonus to attack and damage rolls from a magic sword:
    /// <code>
    /// Weapon magicSword = new Weapon("Long Sword + 1", "A finely crafted sword, its blade dimly glows.",
    ///                                 WeaponType.Melee, new DiceHand(1, DieType.d8));
    /// Modifier attackModifier = new Modifier(ModifierType.Enchantment, 1);
    /// Modifier damageModifier = new Modifier(ModifierType.Enchantment, 1);
    /// magicSword.AttackModifiers.Add(attackModifier);
    /// magicSword.DamageModifiers.Add(damageModifier);
    /// </code>
    /// </example>
    public record Modifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modifier"/> record with the specified type and value.
        /// </summary>
        /// <param name="type">The type (its nature or source) of the modifier.</param>
        /// <param name="value">The amount of the modifier, which can be positive or negative.</param>
        public Modifier(ModifierType type, int value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Gets the nature or source of the modifier.
        /// </summary>
        /// <remarks>Use the ModifierType to remove all Curses from a Being, for example.</remarks>
        public ModifierType Type { get; init; }

        /// <summary>
        /// Gets or sets the amount of the modifier. This value can positive (bonus, enchantment) or negative (penalty, curse).
        /// </summary>
        public int Value { get; init; }
    }
}