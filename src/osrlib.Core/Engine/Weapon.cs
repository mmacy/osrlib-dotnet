namespace osrlib.Core.Engine
{
    /// <summary>
    /// A Weapon is a melee or ranged martial weapon, or an offensive spell.
    /// </summary>
    /// <example>
    /// Create a basic melee weapon
    /// <code>
    /// Weapon dagger = new Weapon("Dagger", "A standard dagger.",
    ///                            WeaponType.Melee, new DiceHand(1, DieType.d4));
    /// </code>
    /// </example>
    /// <example>
    /// Create a magical weapon
    /// <code>
    /// Weapon magicSword = new Weapon("Long Sword + 1", "A finely crafted sword, its blade dimly glows.",
    ///                                WeaponType.Melee, new DiceHand(1, DieType.d8));
    /// Modifier attackModifier = new Modifier(magicSword, 1);
    /// Modifier damageModifier = new Modifier(magicSword, 1);
    /// magicSword.AttackModifiers.Add(attackModifier);
    /// magicSword.DamageModifiers.Add(damageModifier);
    /// </code>
    /// </example>
    /// <example>
    /// Create an offensive spell
    /// <code>
    /// Weapon flameJet = new Weapon("Flame Jet", "A jet of flame issues forth from the caster's hands.",
    ///                              WeaponType.Spell, new DiceHand(1, DieType.d12));
    /// </code>
    /// </example>
    public class Weapon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class with optional parameters.
        /// </summary>
        /// <param name="name">The name of the weapon or spell. Default: "Generic Weapon".</param>
        /// <param name="description">The weapon or spell's description. Default: "This is a generic weapon.".</param>
        /// <param name="type">The weapon type. An offensive spell is considered a weapon. Default: <see cref="WeaponType.Melee"/>.</param>
        /// <param name="damageDie">The die rolled when calculating the weapon or spell's damage. Default: <c>1d4</c>.</param>
        /// <example>
        /// The following example demonstrates how to create a new instance of the <see cref="Weapon"/> class:
        /// <code>
        /// Weapon genericWeapon = new Weapon();
        /// </code>
        /// </example>
        /// <example>
        /// The following example demonstrates how to create a new instance of the <see cref="Weapon"/> class with custom values:
        /// <code>
        /// Weapon customWeapon = new Weapon("Custom Weapon", "This is a custom weapon.", WeaponType.Ranged, new DiceHand(1, DieType.d6));
        /// </code>
        /// </example>
        public Weapon(string name = "Generic Weapon",
            string description = "This is a generic weapon.",
            WeaponType type = WeaponType.Melee,
            DiceHand damageDie = null)
        {
            Name = name;
            Description = description;
            Type = type;
            DamageDie = damageDie ?? new DiceHand(1, DieType.d4);

            AttackModifiers = new List<Modifier>();
            DamageModifiers = new List<Modifier>();
        }

        /// <summary>
        /// Gets or sets the name of the weapon or spell.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the weapon or spell's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the weapon type. An offensive spell is considered a weapon.
        /// </summary>
        public WeaponType Type { get; set; }

        /// <summary>
        /// Gets or sets the die rolled when calculating the weapon or spell's damage. Default: <c>1d4</c>.
        /// </summary>
        public DiceHand DamageDie { get; set; }

        /// <summary>
        /// Rolls a <see cref="DieType.d20"/>, sums any modifiers, and returns the result.
        /// </summary>
        /// <remarks>
        /// The modifier is typically supplied by <see cref="Ability"/> bonuses/penalties or
        /// enchantments/curses applied to the <see cref="Being"/> wielding the weapon or spell.
        /// </remarks>
        /// <param name="modifier">The bonus or penalty to apply to the roll. Default: <c>0</c>.</param>
        /// <returns>The to-hit roll after its <see cref="DiceRoll.RollDice()"/> has been called.</returns>
        public DiceRoll GetAttackRoll(int modifier = 0)
        {
            DiceRoll toHitRoll = new DiceRoll(new DiceHand(1, DieType.d20));

            // Sum the weapon's to-hit modifiers
            if (AttackModifiers.Any())
            {
                modifier += this.AttackModifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
            }

            toHitRoll.AddModifier(modifier);
            toHitRoll.RollDice();

            return toHitRoll;
        }

        /// <summary>
        /// Rolls the weapon's <see cref="DamageDie"/>, sums any modifiers, and returns the result.
        /// </summary>
        /// <remarks>
        /// The modifier is typically supplied by <see cref="Ability"/> bonuses/penalties or
        /// enchantments/curses applied to the <see cref="Being"/> wielding the weapon or spell.
        /// </remarks>
        /// <param name="modifier">The bonus or penalty to apply to the roll. Default: <c>0</c>.</param>
        /// <returns>The damage roll after its <see cref="DiceRoll.RollDice()"/> has been called.</returns>
        public DiceRoll GetDamageRoll(int modifier = 0)
        {
            DiceRoll damageRoll = new DiceRoll(this.DamageDie);

            // Sum the weapon's damage modifiers
            if (DamageModifiers.Any())
            {
                modifier += DamageModifiers.Select(m => m.ModifierValue).Aggregate((x, y) => x + y);
            }

            damageRoll.AddModifier(modifier);
            damageRoll.RollDice();

            return damageRoll;
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