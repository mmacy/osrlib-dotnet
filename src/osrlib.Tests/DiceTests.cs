using System;
using Xunit;
using osrlib.Dice;

namespace osrlib.Tests
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


        [Fact]
        public void SanitizeDiceNotation_ValidInput_ReturnsExpectedResult()
        {
            // Arrange
            string diceNotation = " 2d6 ";

            // Act
            string result = DiceUtility.SanitizeDiceNotation(diceNotation);

            // Assert
            Assert.Equal("2d6", result);
        }

        [Fact]
        public void SanitizeDiceNotation_SingleDieFormat_ReturnsExpectedResult()
        {
            // Arrange
            string diceNotation = "d6";

            // Act
            string result = DiceUtility.SanitizeDiceNotation(diceNotation);

            // Assert
            Assert.Equal("1d6", result);
        }

        [Theory]
        [InlineData("2d")]
        [InlineData("2d6d8")]
        [InlineData("2d6d")]
        [InlineData("2 ")]
        [InlineData("^&$#^&#$&*(#@)~")]
        public void SanitizeDiceNotation_InvalidInput_ThrowsArgumentException(string diceNotation)
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => DiceUtility.SanitizeDiceNotation(diceNotation));
        }

    }
}
