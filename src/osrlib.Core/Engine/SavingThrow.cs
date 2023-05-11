namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents a saving throw, which is the chance that a special attack may be avoided or
    /// will have less than the normal effect.
    /// </summary>
    public class SavingThrow
    {
        /// <summary>
        /// Gets or initializes the type of saving throw. Default is SavingThrowType.None.
        /// </summary>
        public SavingThrowType Type { get; init; } = SavingThrowType.DeathRayOrPoison;

        /// <summary>
        /// Gets or initializes the saving throw score for a Being. Saving throw success or failure is
        /// determined by rolling a 20-sided die (d20) and comparing the result to the saving throw Score.
        /// If the die roll is equal to or greater than the Score, the saving throw is successful.
        /// Default is 20.
        /// </summary>
        public int Score { get; set; } = 20;

        private DiceRoll _roll { get; init; } = new DiceRoll(new DiceHand(1, DieType.d20));

        /// <summary>
        /// Rolls a 20-sided die (d20) and returns a bool indicating whether the save was successful.
        /// </summary>
        /// <returns><c>true</c> if the save was successful, <c>false</c> otherwise.</returns>
        public bool Roll()
        {
            int result = _roll.RollDice();
            return result >= Score;
        }
    }
}
