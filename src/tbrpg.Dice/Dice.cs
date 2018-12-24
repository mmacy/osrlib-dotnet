/*************************************************************************
 * File:        Dice.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2011 Marshall Macy II
 *************************************************************************/

using System;
using System.Collections.Generic;

namespace tbrpg.Dice
{
    /// <summary>
    /// Represents a collection of Die objects, suitable for usage with DiceRoll.
    /// </summary>
    public class Dice : List<Die>
    {
        /// <summary>
        /// Returns a Dice collection containing the specified number of Die objects each with the number of specified sides.
        /// </summary>
        /// <param name="dieCount">The number of Die objects to be added to the collection.</param>
        /// <param name="dieSides">The number of sides for each Die.</param>
        /// <returns>Dice collection.</returns>
        internal static Dice GetDice(int dieCount, int dieSides)
        {
            Dice dice = new Dice();

            //Get each Die object, each with the number of specified sides.
            for (int i = 0; i < dieCount; i++)
            {
                //Since this is Die.Parse, we only want to return one Die object, regardless
                //of whether multiple Dice were specified in the Die code.
                dice.AddDie(new Die(dieSides));
            }

            if (dice.Count > 0)
                return dice;
            else
                return null;
        }

        /// <summary>
        /// Adds the specified Die to this Dice collection.
        /// </summary>
        /// <param name="die">The Die to add to the collection.</param>
        internal void AddDie(Die die)
        {
            base.Add(die);
        }

        /// <summary>
        /// Adds the specified Dice to this Dice collection.
        /// </summary>
        /// <param name="dice">The Dice collection to add to this Dice collection.</param>
        internal void AddDice(Dice dice)
        {
            base.AddRange(dice);
        }

        /// <summary>
        /// Adds the Dice represented by the specified die code (e.g. "1d4", "2d6") to the Dice collection.
        /// </summary>
        /// <param name="dieCode">The die code string representing the number of dice and the number of sides of each die in the DiceRoll, e.g. "1d4", "2d6".</param>
        internal void AddDice(string dieCode)
        {
            base.AddRange(Parse(dieCode));
        }

        /// <summary>
        /// Adds the specified DiceHand to this Dice collection.
        /// </summary>
        /// <param name="hand">The Dice.DiceHand to add to this Dice collection.</param>
        internal void AddDice(DiceHand hand)
        {
            base.AddRange(GetDice(hand.DieCount, hand.DieSides));
        }

        /// <summary>
        /// Given a valid Die code string, returns a Dice collection of Die objects with the attributes specified
        /// by the Die code. A Die code is a string such as "1d4" or "2d6", where the first number specifies the
        /// number of dice, each of which containing the number of sides specified by the second number (i.e.
        /// "1d4" is one four-sided die, "2d6" is two six-sided dice).
        /// </summary>
        /// <param name="dieCode">A string such as "1d4" or "2d6", where the first number specifies the number of dice,
        /// each of which containing the number of sides specified by the second number (i.e. "1d4" is one
        /// four-sided die, "2d6" is two six-sided dice).</param>
        /// <returns>Dice collection containing Die objects of the specified attributes of the Die code.</returns>
        public static Dice Parse(string dieCode)
        {
            if (!String.IsNullOrEmpty(dieCode))
            {
                int dieCount = 0;
                int dieSides = 0;

                //Attempt to populate the number of Dice and the number of sides
                if (Dice.TryParseDieCode(dieCode, out dieCount, out dieSides))
                {
                    return GetDice(dieCount, dieSides);
                }
            }

            //If we fall through to here, we've not obtained a valid Die object.
            return null;
        }

        /// <summary>
        /// Attempts to parse the supplied Die code by splitting it into its constituent parts (number of dice,
        /// number of sides per die) and populating the specified out parameters for each.
        /// </summary>
        /// <param name="dieCode">The string containing the Die code.</param>
        /// <param name="dieCount">The Die count parameter to be populated.</param>
        /// <param name="dieSides">The number of sides per Die parameter to be populated.</param>
        /// <returns>Value indicating whether the Die code was parsed successfully. A value of true indicates two
        /// values - both greater than 0 - were obtained.</returns>
        internal static bool TryParseDieCode(string dieCode, out int dieCount, out int dieSides)
        {
            dieCount = 0;
            dieSides = 0;

            if (!String.IsNullOrEmpty(dieCode))
            {
                //Use the 'd' in the Die code as the delimiter
                string[] dieCodeElements = dieCode.Trim().ToLower().Split('d');

                //Check to ensure we only have two values to examine
                if (dieCodeElements.Length == 2)
                {
                    //Attempt to populate the values, verifying the elements contain integers before continuing
                    if (int.TryParse(dieCodeElements[0], out dieCount) && int.TryParse(dieCodeElements[1], out dieSides))
                    {
                        //Ensure the values are greater than zero
                        if (dieCount > 0 && dieSides > 0)
                            return true;
                    }
                }
            }

            //Fall-through to here indicates failure to parse the Die code, or
            //values less than 0 obtained.
            return false;
        }
    }
}