﻿namespace osrlib.Tests
{
    public class CoreRulesTests
    {
        [Fact]
        public void TestInitializingMajorGameEntities()
        {
            // Arrange
            Party monsterParty = PartyGenerator.GetMonsterParty();
            GamePosition position = new GamePosition(10, 10);

            // Act
            Encounter encounter = new Encounter
            {
                EncounterParty = monsterParty,
                AutoBattleEnabled = true,
                Position = position
            };
            Adventure adventure = new Adventure();
            Dungeon dungeon = new Dungeon();
            dungeon.Encounters.Add(encounter);
            adventure.AddDungeon(dungeon);
            adventure.SetActiveDungeon(dungeon);
            GameManager.Instance.SetActiveAdventure(adventure);
            GameManager.Instance.StartAdventure();

            // Assert
            Assert.NotNull(GameManager.Instance.ActiveAdventure);
        }

        [Fact(Skip = "Test not yet implemented.")]
        public void DoDungeonBattle()
        {
            // Get fully initialized GameManager (from InitGameSystemModel?)
            // Set autobattle ON (this is on the Encounter, but where best to set? Probably at the Adventure level - maybe need an AdventureSettings class)
            // Set active Party for Adventure
            // Set active Dungeon for the Adventure
            // Move Party to Encounter location
            // Perform (auto) battle

            Assert.NotNull("Test not yet implemented.");
        }

        [Fact]
        public void TestGameManagerInitialization()
        {
            // TODO: Get fully initialized GameManager (from InitCoreRulesModel?)
            Assert.NotNull(GameManager.Instance);
        }

        [Fact]
        public void TestAutoBattleInEncounter()
        {
            // Arrange
            Party playerParty = PartyGenerator.GetPlayerParty();
            Party monsterParty = PartyGenerator.GetMonsterParty();

            Encounter encounter = new Encounter
            {
                EncounterParty = monsterParty,
                AutoBattleEnabled = true
            };

            bool encounterEndedEventFired = false;
            void HandleEncounterEndedEvent(object sender, EventArgs e)
            {
                Encounter enc = sender as Encounter;
                encounterEndedEventFired = enc.IsEncounterEnded;
            }
            encounter.EncounterEnded += HandleEncounterEndedEvent;

            // Act
            encounter.SetAdventuringParty(playerParty);
            encounter.StartEncounter();

            // Assert
            Assert.True(encounterEndedEventFired);
        }

        [Fact]
        public void TestCreatingFullyInitializedCharacter()
        {
            // Arrange
            DiceHand diceHand = new DiceHand(1, DieType.d10);
            DiceRoll defenseRoll = new DiceRoll(diceHand);
            DiceRoll hitPointsRoll = new DiceRoll(diceHand);
            Being fighter = new Being
            {
                Name = "Cro Mag",
                Defense = defenseRoll.RollDice(),
                MaxHitPoints = hitPointsRoll.RollDice() + 10
            };
            fighter.HitPoints = fighter.MaxHitPoints;

            Modifier mod = new Modifier { ModifierSource = "Potion of Strength", ModifierValue = 2 };
            Weapon sword = new Weapon
            {
                Name = "Long Sword + 1",
                Description = "A finely crafted sword.",
                Type = WeaponType.Melee,
                DamageDie = new DiceHand(1, DieType.d8),
            };
            sword.AttackModifiers.Add(new Modifier { ModifierValue = 1, ModifierSource = sword });
            sword.DamageModifiers.Add(new Modifier { ModifierValue = 1, ModifierSource = sword });
            fighter.ActiveWeapon = sword;

            // Act
            fighter.RollAbilities();
            fighter.AddAbilityModifier(mod, AbilityType.Strength);

            // Assert
            Assert.Equal(6, fighter.Abilities.Count);
            Assert.Equal(AbilityType.Strength, fighter.Abilities[0].Type);
            Assert.Equal(AbilityType.Dexterity, fighter.Abilities[1].Type);
            Assert.Equal(AbilityType.Constitution, fighter.Abilities[2].Type);
            Assert.Equal(AbilityType.Intelligence, fighter.Abilities[3].Type);
            Assert.Equal(AbilityType.Wisdom, fighter.Abilities[4].Type);
            Assert.Equal(AbilityType.Charisma, fighter.Abilities[5].Type);
        }
    }
}
