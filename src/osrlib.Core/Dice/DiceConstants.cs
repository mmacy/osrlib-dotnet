namespace osrlib.Dice
{
    /// <summary>
    /// Provides a collection of error message constants used in dice-related classes.
    /// </summary>
    public static class ErrorConstants
    {
        /// <summary>
        /// Error message for an invalid number of sides for a die. The value should be one of the elements in the <see cref="DieType"/> enumeration.
        /// </summary>
        public const string DiceCountInvalid = $"Invalid number of sides. Must be one of the values in the {nameof(DieType)} enum.";

        /// <summary>
        /// Error message for an incorrect dice notation format. The expected format is NdN, where N is first the number of dice and then the number of sides.
        /// The number of dice must be greater than 0, and the number of sides must be greater than 1.
        /// Example: 3d6
        /// </summary>
        public const string DiceNotationInvalid = "Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides. The number of dice must be greater than 0 and the number of sides must be greater than 1. Example: 3d6";
    }
}