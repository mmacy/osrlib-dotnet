/*************************************************************************
 * File:        DiceHand.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2011 Marshall Macy II
 *************************************************************************/

using System;

namespace tbrpg.Dice
{
    /// <summary>
    /// Represents a "handful of dice."
    /// </summary>
    /// <remarks>
    /// The DiceHand is a type representation of a Dice code ("1d4", "2d6") and is the preferred parameter for
    /// use in the creation of a Dice object. Create a DiceHand, add it to a <see cref="DiceRoll"/>, then call its
    /// <see cref="DiceRoll.RollDice"/> method to get the result.
    /// </remarks>
    /// <example>
    /// <code>
    /// DiceHand hand = new DiceHand(1, 20);
    /// DiceRoll roll = new DiceRoll(hand);
    /// int result = roll.RollDice();
    /// </code>
    /// </example>
    public class DiceHand
    {
        /// <summary>
        /// Creates a new instance of DiceHand, appropriate for passing to the Dice and DiceRoll constructor.
        /// </summary>
        /// <param name="count">The number of Dice in the DiceHand - the first value in the '#d#' format (e.g. "1d4", "2d6").</param>
        /// <param name="sides">The number of sides per Die in the DiceHand - the second value in the '#d#' format (e.g. "1d4", "2d6").</param>
        public DiceHand(int count, int sides)
        {
            //Perform some validity checks to ensure the count and sides params are at least 0.
            //Obviously, it would be normal to have at least two sides per die, but we don't
            //want to limit that far as someone might always want a value of 1.

            if (count > 0)
            {
                DieCount = count;

                if (sides > 0)
                {
                    DieSides = sides;
                }
                else
                {
                    throw new ArgumentException("The sides parameter (number of sides per Die) must be equal to or greater than 1.");
                }
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
        public int DieSides { get; set; }
    }
}