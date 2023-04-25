namespace osrlib.Tests
{
    public class AbilityTests
    {
        [Fact]
        public void AbilityConstructor_SetsTypeAndRollsBaseValue()
        {
            // Arrange
            AbilityType type = AbilityType.Strength;

            // Act
            Ability ability = new Ability(type);

            // Assert
            Assert.Equal(type, ability.Type);
            Assert.True(ability.BaseValue >= 3 && ability.BaseValue <= 18);
        }

        [Fact]
        public void Ability_GetModifier_ReturnsCorrectModifier()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Dexterity)
            {
                BaseValue = 16
            };

            // Act
            int modifier = ability.GetModifier();

            // Assert
            Assert.Equal(2, modifier);
        }

        [Fact]
        public void Ability_RollAbilityScore_SetsBaseValue()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Intelligence);

            // Act
            int rolledScore = ability.RollAbilityScore();

            // Assert
            Assert.Equal(rolledScore, ability.BaseValue);
            Assert.True(rolledScore >= 3 && rolledScore <= 18);
        }

        [Fact]
        public void Ability_Value_CalculatesCorrectly()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Charisma)
            {
                BaseValue = 14,
                ScoreModifiers = new List<Modifier>
                {
                    new Modifier("Item", 2),
                    new Modifier("Spell", -1)
                }
            };

            // Act
            int value = ability.Value;

            // Assert
            Assert.Equal(15, value);
        }

        [Fact]
        public void Ability_ToString_FormatsCorrectly()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Wisdom)
            {
                BaseValue = 12,
                ScoreModifiers = new List<Modifier>
                {
                    new Modifier("Item", 3),
                    new Modifier("Spell", -2)
                }
            };

            // Act
            string abilityString = ability.ToString();

            // Assert
            Assert.Equal("Wisdom: 13 (12 + 1)", abilityString);
        }
    }
}
