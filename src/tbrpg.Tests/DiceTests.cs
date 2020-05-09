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
            DieType dieType = DieType.d20;
            DiceHand hand = new DiceHand(numDie, dieType);
            DiceRoll roll = new DiceRoll(hand);
            int result;
            for (int i = 0; i < numRolls; i++)
            {
                result = roll.RollDice();
                Assert.True(result >= numDie);
                Assert.True(result <= numDie * (int)dieType);
            }

            numDie = 2;
            dieType = DieType.d10;
            hand = new DiceHand(numDie, dieType);
            roll = new DiceRoll(hand);
            for (int i = 0; i < numRolls; i++)
            {
                result = roll.RollDice();
                Assert.True(result >= numDie);
                Assert.True(result <= numDie * (int)dieType);
            }
        }

        [Fact]
        public void InvalidDiceRollThrowsException()
        {
            int numDie = 0;
            DieType dieType = DieType.d1;

            Exception ex = Assert.Throws<ArgumentException>(() => new DiceHand(numDie, dieType));
        }
    }
}
