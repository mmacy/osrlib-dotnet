using System;
using Xunit;
using tbrpg.Dice;

namespace tbrpg.Tests
{
    public class DiceTests
    {
        /// <summary>
        /// Ensures that the <see cref="DiceRoll"/> always returns values within the desired range.
        /// </summary>
        [Fact]
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
                Assert.True(result >= numDie);
                Assert.True(result <= numDie * numSides);
            }

            numDie = 2;
            numSides = 10;
            hand = new DiceHand(numDie, numSides);
            roll = new DiceRoll(hand);
            for (int i = 0; i < numRolls; i++)
            {
                result = roll.RollDice();
                Assert.True(result >= numDie);
                Assert.True(result <= numDie * numSides);
            }
        }

        [Fact]
        public void InvalidDiceRollThrowsException()
        {
            int numDie = 1;
            int numSides = 0; // A die needs at least 1 side

            Exception ex = Assert.Throws<ArgumentException>(() => new DiceHand(numDie, numSides));
        }

        [Fact]
        public void DiceRollWithEmptyDieCodeTextConstructorThrowsException()
        {
            Exception ex = Assert.Throws<ArgumentNullException>(() => new DiceRoll(""));
        }

        [Fact]
        public void DiceRollWithInvalidDieCodeTextConstructorThrowsException()
        {
            Exception ex = Assert.Throws<ArgumentException>(() => new DiceRoll("BLARG!"));
        }
    }
}
