namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents hit points for a character or monster, including base hit points, modifiers, and current wounds.
    /// </summary>
    public class HitPoints
    {
        private readonly DiceRoll _hitDieRoll;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HitPoints"/> class with the specified hit die.
        /// </summary>
        /// <remarks>
        /// The base hit points are not rolled when using this constructor. You should call the <see cref="Roll"/>
        /// method immediately after instantiation.
        /// </remarks>
        /// <param name="hitDie">The die type used when rolling hit points.</param>
        public HitPoints(DieType hitDie)
        {
            HitDie = hitDie;
            _hitDieRoll = new DiceRoll(new DiceHand(1, hitDie));
        }

        /// <summary>
        /// Gets or sets the base hit points for the character or monster.
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Gets the hit die (HD) used for rolling hit points.
        /// </summary>
        public DieType HitDie { get; init; }

        /// <summary>
        /// Gets or sets the collection of modifiers affecting the character or monster's hit points.
        /// </summary>
        /// <remarks>
        /// Use these modifiers to affect the hit points temporarily, such as for enchantments or curses.
        /// These modifiers are not added to the result when you call the <see cref="Roll"/> method. To
        /// add a modifier value like a Constitution ability modifier when rolling hit points, pass
        /// its literal value to the <see cref="Roll"/> method instead of adding it to this collection.
        /// </remarks>
        public List<Modifier> Modifiers { get; init; } = new();

        /// <summary>
        /// Gets or sets the number of hit points lost due to wounds on the character or monster.
        /// Default is 0.
        /// </summary>
        public int Wounds { get; set; } = 0;

        /// <summary>
        /// Gets the maximum hit points by adding all the modifiers to the base.
        /// </summary>
        public int Maximum => Base + Modifiers.Sum(m => m.Value);

        /// <summary>
        /// Gets the current hit points by adding all the modifiers to the base and then subtracting the wounds.
        /// </summary>
        public int Current => Maximum - Wounds;

        /// <summary>
        /// Rolls the hit die and applies the optional modifier, then adds the result to the base hit points.
        /// </summary>
        /// <remarks>
        /// Calling the Roll method permanently updates the Base value of the hit points. You should
        /// call this method only on occasions that warrant doing so, such as when first creating a character
        /// or when a character levels up.
        ///
        /// Only the result of the hit point roll (plus the modifier, if specified) is returned by this method.
        /// The returned value doesn't include the HitPoint's Base value or any modifiers in its Modifiers collection.
        /// </remarks>
        /// <param name="modifier">An optional modifier to add to the rolled hit points.</param>
        /// <returns>
        /// The result of the hit point roll including the modifier, if specified.
        /// </returns>
        public int Roll(int modifier = 0)
        {
            int hpRoll = _hitDieRoll.RollDice() + modifier;
            
            // Ensure the Base value never goes below 1
            Base = Math.Max(Base + hpRoll, 1);

            return hpRoll;
        }
        
        /// <summary>
        /// Returns a string representation of the hit points in the format "Current/Maximum".
        /// </summary>
        /// <returns>A string representation of the hit points.</returns>
        public override string ToString()
        {
            return $"{Current}/{Maximum}";
        }
    }
}
