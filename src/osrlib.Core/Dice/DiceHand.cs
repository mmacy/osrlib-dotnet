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
    /// call its <see cref="DiceRoll.RollDice()"/> method to get the result.
    /// </remarks>
    public class DiceHand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiceHand"/> class using the specified number of dice and sides.
        /// </summary>
        /// <param name="count">The number of dice in the DiceHand.</param>
        /// <param name="sides">The number of sides of each die in the DiceHand.</param>
        /// <example>
        /// The following example demonstrates how to create a new instance of the <see cref="DiceHand"/> class using two 6-sided dice and pass it to the DiceRoll constructor:
        /// <code>
        /// DiceHand diceHand = new DiceHand(2, DieType.d6);
        /// DiceRoll diceRoll = new DiceRoll(diceHand);
        /// </code>
        /// </example>
        public DiceHand(int count, DieType sides)
        {
            // Perform some validity checks to ensure the count and sides params are at least 0.

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
        /// Initializes a new instance of the <see cref="DiceHand"/> class using the specified dice notation.
        /// </summary>
        /// <param name="diceNotation">The dice notation string, in the format "NdN", where N is a positive integer.</param>
        /// <example>
        /// The following example demonstrates how to create a new instance of the <see cref="DiceHand"/> class using the dice notation string "2d6" and pass it to the DiceRoll constructor:
        /// <code>
        /// DiceHand diceHand = new DiceHand("2d6");
        /// DiceRoll diceRoll = new DiceRoll(diceHand);
        /// </code>
        /// </example>
        public DiceHand(string diceNotation)
        {
            try
            {
                // Sanitize the dice notation
                diceNotation = DiceUtility.SanitizeDiceNotation(diceNotation);
            }
            catch (ArgumentException)
            {
                // Rethrow the exception if it was thrown by SanitizeDiceNotation
                throw new ArgumentException("Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.");
            }

            // Split the dice notation into the number of dice and the number of sides
            string[] parts = diceNotation.Split('d');
            int count = int.Parse(parts[0]);
            int sides = int.Parse(parts[1]);

            if (count <= 0)
            {
                // Must have at least one (1) die to roll.
                throw new ArgumentException("Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.");
            }

            // Set the properties
            DieCount = count;
            DieSides = (DieType)sides;
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