using System;
using Xunit;
using osrlib.CoreRules;
using osrlib.Controllers;
using osrlib.Dice;

namespace osrlib.Tests
{
    public class CoreRulesTests
    {
        [Fact]
        public void InitMajorGameEntities()
        {
            // TODO: This isn't really a "unit test." Should break these out
            // TODO: into their own methods, and perhaps use a playlist to execute
            // TODO: them in order, populating member fields of this test class
            // TODO: and referencing those in downstream tests.

            // Create Encounter
            Encounter encounter = new Encounter
            {
                EncounterParty = PartyGenerator.GetMonsterParty(),
                AutoBattleEnabled = true,
                Position = new GamePosition(10, 10)
            };
            // Create Adventure
            Adventure adventure = new Adventure();

            // Create Dungeon
            Dungeon dungeon = new Dungeon();
            // Add Encounter to Dungeon
            dungeon.Encounters.Add(encounter);
            // Add Dungeon to Adventure

            adventure.AddDungeon(dungeon);
            // Set active Dungeon for Adventure
            adventure.SetActiveDungeon(dungeon);

            // Set active Adventure for GameManager
            GameManager.Instance.SetActiveAdventure(adventure);
            // Set active Party for Adventure
            GameManager.Instance.StartAdventure();

            Assert.NotNull(GameManager.Instance.ActiveAdventure);
        }

        [Fact(Skip = "Test not yet implemented.")]
        public void ExecuteDungeonBattle()
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
        public void InitGameManager()
        {
            // TODO: Get fully initialized GameManager (from InitCoreRulesModel?)
            Assert.NotNull(GameManager.Instance);
        }

        [Fact]
        public void DoEncounterAutoBattle()
        {
            Party playerParty = PartyGenerator.GetPlayerParty();
            Party monsterParty = PartyGenerator.GetMonsterParty();

            Encounter encounter = new Encounter
            {
                EncounterParty = monsterParty,
                AutoBattleEnabled = true
            };
            encounter.EncounterEnded += (object sender, EventArgs e) =>
            {
                Encounter enc = sender as Encounter;
                Assert.True(enc.IsEncounterEnded);
            };

            // Add the adventuring party and start the battle
            encounter.SetAdventuringParty(playerParty);
            encounter.StartEncounter();
        }

        [Fact]
        public void CreateFullyInitializedCharacter()
        {
            // Another "unit test" that's not really a unit test.
            // Using this temporarily (ha!) to debug modifier work.

            DiceRoll roll = new DiceRoll(new DiceHand(1, DieType.d10));

            Being fighter = new Being
            {
                Name = "Cro Mag",
                Defense = roll.RollDice(),
                MaxHitPoints = roll.RollDice() + 10,
                IsTargetable = true
            };
            fighter.RollAbilities();

            Modifier mod = new Modifier { ModifierSource = "Potion of Strength", ModifierValue = 2 };
            fighter.AddAbilityModifier(mod, AbilityType.Strength);

            Weapon sword = new Weapon
            {
                Name = "Long Sword + 1",
                Description = "A finely crafted sword.",
                Type = WeaponType.Melee,
                DamageDie = new DiceHand(1, DieType.d8),
            };
            sword.AttackModifiers.Add(new Modifier { ModifierValue = 1, ModifierSource = sword });
            sword.DamageModifiers.Add(new Modifier { ModifierValue = 1, ModifierSource = sword });

            // Verify the ability collection has the expected abilities
            Assert.Collection(fighter.Abilities,
                ability => Assert.Equal(AbilityType.Strength, ability.Type),
                ability => Assert.Equal(AbilityType.Dexterity, ability.Type),
                ability => Assert.Equal(AbilityType.Constitution, ability.Type),
                ability => Assert.Equal(AbilityType.Intelligence, ability.Type),
                ability => Assert.Equal(AbilityType.Wisdom, ability.Type),
                ability => Assert.Equal(AbilityType.Charisma, ability.Type));
        }
    }
}
