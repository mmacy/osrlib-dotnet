using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tbrpg.CoreRules;
using tbrpg.Controllers;

namespace tbrpg.Tests
{
    [TestClass]
    public class CoreRulesTests
    {
        [TestMethod]
        [TestCategory("CoreRules")]
        public void InitMajorGameEntities()
        {
            // TODO: This is not exactly a "unit test." Should break these out
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

            Assert.IsNotNull(GameManager.Instance.ActiveAdventure);
        }

        [TestMethod]
        [TestCategory("CoreRules")]
        public void ExecuteAdventureBattle()
        {
            // Get fully initialized GameManager (from InitGameSystemModel?)
            // Get Encounter from active Adventure, active Dungeon
            // Set active Encounter (on Dungeon? on GameManager?)
            // Set autobattle ON (this is on the Encounter, but where best to set?)
            // Add active Adventure party to active Encounter
            // Perform battle

            Assert.Inconclusive("Test not yet implemented.");
        }

        [TestMethod]
        [TestCategory("CoreRules")]
        public void InitGameManager()
        {
            // TODO: Get fully initialized GameManager (from InitCoreRulesModel?)
            Assert.IsNotNull(GameManager.Instance);
        }

        [TestMethod]
        [TestCategory("CoreRules")]
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
                Assert.IsTrue(enc.IsEncounterEnded);
            };

            // Add the adventuring party and specify AutoBattle = true
            encounter.SetAdventuringParty(playerParty, true);
        }
    }
}
