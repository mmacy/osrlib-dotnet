using System;
using Xunit;
using tbrpg.CoreRules;
using tbrpg.Controllers;

namespace tbrpg.Tests
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
            // Create Dungeon
            Dungeon dungeon = new Dungeon();
            // Add Encounter to Dungeon
            dungeon.Encounters.Add(encounter);
            // Create Adventure
            Adventure adventure = new Adventure();
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
        public void ExecuteAdventureBattle()
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
    }
}
