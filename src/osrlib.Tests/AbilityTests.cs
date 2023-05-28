namespace osrlib.Tests
{
    public class AbilityTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public AbilityTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        [Fact]
        public void AbilityConstructor_SetsTypeAndRollsBaseValue()
        {
            // Arrange
            AbilityType type = AbilityType.Strength;

            // Act
            Ability ability = new Ability(type);

            // Assert
            Assert.Equal(type, ability.Type);
            Assert.True(ability.Base >= 3 && ability.Base <= 18);
        }

        [Fact]
        public void Ability_GetModifier_ReturnsCorrectModifier()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Dexterity)
            {
                Base = 16
            };

            // Act
            int modifier = ability.GetModifierValue();

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
            Assert.Equal(rolledScore, ability.Base);
            Assert.True(rolledScore >= 3 && rolledScore <= 18);
        }

        [Fact]
        public void Ability_Value_CalculatesCorrectly()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Charisma)
            {
                Base = 14,
                ScoreModifiers = new List<Modifier>
                {
                    new Modifier(ModifierType.Enchantment, 2),
                    new Modifier(ModifierType.Curse, -1)
                }
            };

            // Act
            int value = ability.Score;

            // Assert
            Assert.Equal(15, value);
        }

        [Fact]
        public void Ability_ToString_FormatsCorrectly()
        {
            // Arrange
            Ability ability = new Ability(AbilityType.Wisdom)
            {
                Base = 12,
                ScoreModifiers = new List<Modifier>
                {
                    new Modifier(ModifierType.Potion, 3),
                    new Modifier(ModifierType.Curse, -2)
                }
            };

            // Act
            string abilityString = ability.ToString();

            // Assert
            Assert.Equal("Wisdom: 13 (12 + 1)", abilityString);
        }
    }
}
