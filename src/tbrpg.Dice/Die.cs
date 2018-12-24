/*************************************************************************
 * File:        Die.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2011 Marshall Macy II
 *************************************************************************/

using System;

namespace tbrpg.Dice
{
    /// <summary>
    /// Represents a single Die, appropriate for adding to a DiceRoll.
    /// </summary>
    public class Die
    {
        #region Fields
        private int _sides = 0;
        private int _minValue = 1; //All Die should have default minVal of 1
        private int _rolledValue = 0;
        private bool _wasRolled;
        #endregion

        /// <summary>
        /// Creates a new instance of a Die.
        /// </summary>
        /// <param name="sides">The number of sides for the Die. This value must be greater than or equal to 2.</param>
        public Die(int sides)
        {
            if (sides >= 2)
                _sides = sides;
            else
                throw new ArgumentException("A Die cannot have less than 2 sides.", "sides");
        }

        #region Internal Methods
        /// <summary>
        /// Given a valid Die code string, returns a Die object with the attributes specified by the Die code.
        /// A Die code is a string such as "1d4" or "2d6", where the first number specifies the number of dice,
        /// each of which containing the number of sides specified by the second number (i.e. "1d4" is one
        /// four-sided die, "2d6" is two six-sided dice). Note that Die.Parse() returns only the first Die object
        /// if supplied a Die code specifying multiple Dice. To return multiple Dice, see Dice.Parse().
        /// </summary>
        /// <param name="dieCode">A string such as "1d4" or "2d6", where the first number specifies the number of dice,
        /// each of which containing the number of sides specified by the second number (i.e. "1d4" is one
        /// four-sided die, "2d6" is two six-sided dice).</param>
        /// <returns>The first Die object containing the specified attributes of the Die code. Only one Die object is
        /// returned regardless of the number of Dice specified in the Die code.</returns>
        internal Die Parse(string dieCode)
        {
            if (!String.IsNullOrEmpty(dieCode))
            {
                int dieCount = 0;
                int dieSides = 0;

                //Attempt to populate the number of Dice and the number of sides
                if (Dice.TryParseDieCode(dieCode, out dieCount, out dieSides))
                {
                    //Since this is Die.Parse, we only want to return one Die object, regardless
                    //of whether multiple Dice were specified in the Die code.
                    return new Die(dieSides);
                }
            }

            //If we fall through to here, we've not obtained a valid Die object.
            return null;
        }

        /// <summary>
        /// Returns a random value between the MinValue of the Die and its number of sides (i.e. it "rolls" the Die).
        /// </summary>
        /// <returns>int value of the rolled Die.</returns>
        internal int Roll()
        {

            _wasRolled = true;

            _rolledValue = Utility.Randomizer.GetRandomInt(_minValue, _sides);

            return _rolledValue;
        }

        /// <summary>
        /// Sets the minimum value for the Die. This value must be equal to or greater than 0. The default is 1.
        /// </summary>
        /// <param name="minValue">The minimum int value of the Die, typically 1.</param>
        internal void SetMinimumValue(int minValue)
        {
            if (minValue >= 0)
                _minValue = minValue;
            else
                throw new ArgumentException("A Die must have a minimum value equal to or greater than 0.", "minValue");
        }
        #endregion

        /// <summary>
        /// Gets the number of sides of the Die.
        /// </summary>
        public int Sides { get { return _sides; } }
    }
}