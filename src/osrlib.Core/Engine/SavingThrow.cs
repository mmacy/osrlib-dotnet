namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents a saving throw, which is the chance that a special attack may be avoided or
    /// will have less than the normal effect.
    /// </summary>
    public record SavingThrow
    {
        /// <summary>
        /// Gets or initializes the saving throw type.
        /// </summary>
        public SavingThrowType Type { get; init; }

        /// <summary>
        /// Gets or initializes the chance for a successful saving throw. A successful saving throw is
        /// typically determined by rolling a 20-sided die (d20) and comparing the result to the chance.
        /// If the die roll is equal to or greater than the chance, the saving throw is successful.
        /// </summary>
        public int Chance { get; init; }
    }
}
