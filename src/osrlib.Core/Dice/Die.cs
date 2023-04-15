namespace osrlib.Dice
{
    /// <summary>
    /// Represents a single die with a given number of sides.
    /// </summary>
    public class Die
    {
        private int _minValue = 1;
        private int _rolledValue = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Die"/> class with the specified number of sides.
        /// </summary>
        /// <param name="type">The number of sides for the die. This value must be greater than or equal to 2.</param>
        public Die(DieType type) => DieType = type;

        /// <summary>
        /// Rolls the die and returns a random value between the minimum value and the number of sides.
        /// </summary>
        /// <returns>An integer value representing the result of the die roll.</returns>
        internal int Roll()
        {
            _rolledValue = Utility.Randomizer.GetRandomInt(_minValue, (int)DieType);

            return _rolledValue;
        }

        /// <summary>
        /// Sets the minimum value for the die. This value must be equal to or greater than 0. Default: 1.
        /// </summary>
        /// <param name="minValue">The minimum integer value of the die, typically 1.</param>
        /// <exception cref="ArgumentException">Thrown when the provided minimum value is less than 0.</exception>
        internal void SetMinimumValue(int minValue)
        {
            if (minValue >= 0)
                _minValue = minValue;
            else
                throw new ArgumentException("A die must have a minimum value equal to or greater than 0.", nameof(minValue));
        }

        /// <summary>
        /// Gets the type of the die.
        /// </summary>
        public DieType DieType { get; }

        /// <summary>
        /// Gets the number of sides of the die.
        /// </summary>
        public int Sides => (int)DieType;
    }
}
