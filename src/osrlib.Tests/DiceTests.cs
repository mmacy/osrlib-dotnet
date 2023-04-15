namespace osrlib.Tests
{
    public class DiceTests
    {
        [Fact]
        public void DiceHand_IntConstructor_ReturnsExpectedResult()
        {
            // Arrange
            int count = 2;
            DieType sides = DieType.d6;

            // Act
            var diceHand = new DiceHand(count, sides);

            // Assert
            Assert.Equal(count, diceHand.DieCount);
            Assert.Equal(sides, diceHand.DieSides);
        }

        [Fact]
        public void DiceHand_StringConstructor_ReturnsExpectedResult()
        {
            // Arrange
            string diceNotation = "2d6";

            // Act
            var diceHand = new DiceHand(diceNotation);

            // Assert
            Assert.Equal(2, diceHand.DieCount);
            Assert.Equal(DieType.d6, diceHand.DieSides);
        }

        [Theory]
        [InlineData(0, "The count parameter (number of dice) must be equal to or greater than 1.")]
        [InlineData(-2, "The count parameter (number of dice) must be equal to or greater than 1.")]
        public void DiceHand_IntConstructor_ThrowsArgumentException_WhenCountIsInvalid(int count, string expectedMessage)
        {
            // Arrange
            DieType sides = DieType.d6;

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new DiceHand(count, sides));

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("d0", "Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.")]
        [InlineData("2d0", "Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.")]
        [InlineData("2d",  "Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.")]
        [InlineData("abc", "Incorrect dice notation format. Use NdN, where N is first the number of dice and then the number of sides.")]
        public void DiceHand_StringConstructor_ThrowsArgumentException_WhenDiceNotationIsInvalid(string diceNotation, string expectedMessage)
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => new DiceHand(diceNotation));

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        /// <summary>
        /// Ensures that the <see cref="DiceRoll"/> always returns values within the desired range.
        /// </summary>
        [Fact]
        public void DiceRolls_ShouldAlwaysBeWithinBounds()
        {
            // Arrange
            int numRolls = 1000;

            // Test for numDie = 1, dieType = d20
            int numDie = 1;
            DieType dieType = DieType.d20;
            DiceHand hand = new DiceHand(numDie, dieType);
            DiceRoll roll = new DiceRoll(hand);

            // Act and Assert
            for (int i = 0; i < numRolls; i++)
            {
                int result = roll.RollDice();
                Assert.InRange(result, numDie, numDie * (int)dieType);
            }

            // Test for numDie = 2, dieType = d10
            numDie = 2;
            dieType = DieType.d10;
            hand = new DiceHand(numDie, dieType);
            roll = new DiceRoll(hand);

            // Act and Assert
            for (int i = 0; i < numRolls; i++)
            {
                int result = roll.RollDice();
                Assert.InRange(result, numDie, numDie * (int)dieType);
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

        [Theory]
        [InlineData("2d")]
        [InlineData("2d6d8")]
        [InlineData("2d6d")]
        [InlineData("2 ")]
        [InlineData("^&$#^&#$&*(#@)~")]
        [InlineData("02d6")]
        public void SanitizeDiceNotation_InvalidInput_ThrowsArgumentException(string diceNotation)
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => DiceUtility.SanitizeDiceNotation(diceNotation));
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

    }
}
