using System;

namespace osrlib.Dice
{
    /// <summary>
    /// Represents a single Die with a given number of sides.
    /// </summary>
    public class Die
    {
        #region Fields
        private DieType _dieType = DieType.d20;
        private int _minValue = 1;
        private int _rolledValue = 0;
        #endregion

        /// <summary>
        /// Creates a new instance of a Die.
        /// </summary>
        /// <param name="type">The number of sides for the Die. This value must be greater than or equal to 2.</param>
        public Die(DieType type) => _dieType = type;

        /// <summary>
        /// Returns a random value between the MinValue of the Die and its number of sides (i.e. it "rolls" the Die).
        /// </summary>
        /// <returns>int value of the rolled Die.</returns>
        internal int Roll()
        {
            _rolledValue = Utility.Randomizer.GetRandomInt(_minValue, (int)_dieType);

            return _rolledValue;
        }

        /// <summary>
        /// Sets the minimum value for the Die. This value must be equal to or greater than 0. Default: 1.
        /// </summary>
        /// <param name="minValue">The minimum int value of the Die, typically 1.</param>
        internal void SetMinimumValue(int minValue)
        {
            if (minValue >= 0)
                _minValue = minValue;
            else
                throw new ArgumentException("A Die must have a minimum value equal to or greater than 0.", "minValue");
        }

        /// <summary>
        /// Gets the type of die.
        /// </summary>
        public DieType DieType { get { return _dieType; } }

        /// <summary>
        /// Gets the number of sides of the Die.
        /// </summary>
        public int Sides { get { return (int)_dieType; } }
    }
}