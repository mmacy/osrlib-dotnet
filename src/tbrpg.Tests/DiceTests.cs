using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tbrpg.Dice;

namespace tbrpg.Tests
{
    [TestClass]
    public class DiceTests
    {
        /// <summary>
        /// Ensures that the <see cref="DiceRoll"/> always returns values within the desired range.
        /// </summary>
        [TestMethod][TestCategory("Dice")]
        public void DiceRollsAlwaysWithinBounds()
        {
            int numRolls = 1000;

            int numDie = 1;
            int numSides = 20;
            DiceHand hand = new DiceHand(numDie, numSides);
            DiceRoll roll = new DiceRoll(hand);
            int result;
            for (int i = 0; i < numRolls; i++)
            {
                result = roll.RollDice();
                Assert.IsTrue(result >= numDie);
                Assert.IsTrue(result <= numDie * numSides);
            }

            numDie = 2;
            numSides = 10;
            hand = new DiceHand(numDie, numSides);
            roll = new DiceRoll(hand);
            for (int i = 0; i < numRolls; i++)
            {
                result = roll.RollDice();
                Assert.IsTrue(result >= numDie);
                Assert.IsTrue(result <= numDie * numSides);
            }
        }

        [TestMethod][TestCategory("Dice")]
        [ExpectedException(typeof(ArgumentException), "Invalid DiceHand specified, but exception not thrown.")]
        public void InvalidDiceRollThrowsException()
        {
            int numDie = 1;
            int numSides = 1; // Can't have a single-sided die
            DiceHand hand = new DiceHand(numDie, numSides);
            DiceRoll roll = new DiceRoll(hand);
        }

        [TestMethod][TestCategory("Dice")]
        [ExpectedException(typeof(ArgumentNullException), "Invalid DiceRoll constructor specified, but exception not thrown.")]
        public void DiceRollWithEmptyDieCodeTextConstructorThrowsException()
        {
            DiceRoll roll = new DiceRoll("");
        }

        [TestMethod][TestCategory("Dice")]
        [ExpectedException(typeof(ArgumentException), "Invalid DiceRoll constructor specified, but exception not thrown.")]
        public void DiceRollWithInvalidDieCodeTextConstructorThrowsException()
        {
            DiceRoll roll = new DiceRoll("BLARGH!");
        }
    }
}
