/*************************************************************************
 * File:        DiceHand.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2011 Marshall Macy II
 *************************************************************************/

using System;

namespace osrlib.Dice
{
    /// <summary>
    /// Represents a "handful of dice."
    /// </summary>
    /// <remarks>
    /// The DiceHand is a type representation of the well-known NdN format for a handful of
    /// dice. For example, "1d20", "3d6", or "4d8." It is the preferred parameter for use in
    /// the creation of a DiceRoll. Create a DiceHand, add it to a <see cref="DiceRoll"/>, then
    /// call its <see cref="DiceRoll.RollDice"/> method to get the result.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Roll one twenty-sided die
    /// DiceHand hand = new DiceHand(1, DieType.d20);
    /// DiceRoll roll = new DiceRoll(hand);
    /// int result = roll.RollDice();
    /// </code>
    /// </example>
    public class DiceHand
    {
        /// <summary>
        /// Creates a new instance of DiceHand, appropriate for passing to the Dice and DiceRoll constructors.
        /// </summary>
        /// <param name="count">The number of Dice in the DiceHand - the first value in the '#d#' format (the '3' in 3d6).</param>
        /// <param name="sides">The number of sides per Die in the DiceHand.</param>
        public DiceHand(int count, DieType sides)
        {
            //Perform some validity checks to ensure the count and sides params are at least 0.

            if (count > 0)
            {
                DieCount = count;
                DieSides = sides;
            }
            else
            {
                throw new ArgumentException("The count parameter (number of dice) must be equal to or greater than 1.");
            }
        }

        /// <summary>
        /// Gets or sets the number of dice in the DiceHand.
        /// </summary>
        public int DieCount { get; set; }

        /// <summary>
        /// Gets or sets the number of sides of each die in the DiceHand.
        /// </summary>
        public DieType DieSides { get; set; }
    }
}