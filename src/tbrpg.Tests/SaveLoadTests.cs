using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tbrpg.SaveLoad;
using tbrpg.CoreRules;

namespace tbrpg.Tests
{
    [TestClass]
    public class SaveLoadTests
    {
        private string _saveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string _saveFile = "tbrpg_adventure.json";

        [TestMethod]
        [TestCategory("ExternalSystems")]
        public void SaveAdventureToFile()
        {
            Adventure adventure = GetInitializedAdventure();

            Assert.IsTrue(SaveLoadLocal.Save(adventure, Path.Combine(_saveDir, _saveFile)));
        }

        [TestMethod]
        [TestCategory("ExternalSystems")]
        public void LoadAdventureFromFile()
        {
            Adventure loadedAdventure = SaveLoadLocal.Load(Path.Combine(_saveDir, _saveFile));

            Assert.IsNotNull(loadedAdventure);
        }
        
        private Adventure GetInitializedAdventure()
        {
            Encounter encounter = new Encounter
            {
                EncounterParty = PartyGenerator.GetMonsterParty(),
                AutoBattleEnabled = true,
                Position = new GamePosition(10, 10)
            };

            Dungeon dungeon = new Dungeon();
            dungeon.Encounters.Add(encounter);

            Adventure adventure = new Adventure();
            adventure.SetActiveParty(PartyGenerator.GetPlayerParty());
            adventure.AddDungeon(dungeon);
            adventure.SetActiveDungeon(dungeon);

            return adventure;
        }
    }
}
