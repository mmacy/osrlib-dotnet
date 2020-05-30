using System;
using System.Collections.Generic;

namespace osrlib.Dice
{
    /// <summary>
    /// Represents a collection of <see cref="Die"/> objects.
    /// </summary>
    public class Dice : List<Die>
    {
        /// <summary>
        /// Returns a Dice collection containing the specified number of Die objects each with the number of specified sides.
        /// </summary>
        /// <param name="dieCount">The number of Die objects to be added to the collection.</param>
        /// <param name="dieType">The type (number of sides) for each Die.</param>
        /// <returns>Dice collection.</returns>
        internal static Dice GetDice(int dieCount, DieType dieType)
        {
            Dice dice = new Dice();

            //Get each Die object, each with the number of specified sides.
            for (int i = 0; i < dieCount; i++)
            {
                dice.AddDie(new Die(dieType));
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
        /// Adds the specified DiceHand to this Dice collection.
        /// </summary>
        /// <param name="hand">The Dice.DiceHand to add to this Dice collection.</param>
        internal void AddDice(DiceHand hand)
        {
            base.AddRange(GetDice(hand.DieCount, hand.DieSides));
        }
    }
}