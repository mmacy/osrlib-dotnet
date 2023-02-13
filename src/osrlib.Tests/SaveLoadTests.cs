using System;
using System.IO;
using Xunit;
using osrlib.SaveLoad;
using osrlib.Core;

namespace osrlib.Tests
{

    [Collection("SaveLoadTests")]
    public class SaveLoadTests
    {
        private string _saveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string _saveFile = "tbrpg-adventure.json";

        [Fact, TestPriority(1)]
        public void SaveAdventureFile_ShouldSucceed()
        {
            // Arrange
            Adventure adventure = GetInitializedAdventure();

            // Act
            bool result = SaveLoadLocal.Save(adventure, Path.Combine(_saveDir, _saveFile));

            // Assert
            Assert.True(result);
        }

        [Fact, TestPriority(2)]
        public void LoadAdventureFile_ShouldSucceed()
        {
            // Arrange
            string savePath = Path.Combine(_saveDir, _saveFile);

            // Act
            Adventure loadedAdventure = SaveLoadLocal.Load(savePath);

            // Assert
            Assert.NotNull(loadedAdventure);
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
            adventure.AddDungeon(dungeon);
            adventure.SetActiveDungeon(dungeon);
            adventure.SetActiveParty(PartyGenerator.GetPlayerParty());

            return adventure;
        }
    }
}
