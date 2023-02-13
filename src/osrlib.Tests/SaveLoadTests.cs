using System;
using System.IO;
using Xunit;
using osrlib.SaveLoad;
using osrlib.Core;

namespace osrlib.Tests
{

    [Collection("SaveLoadTests")]
    [TestCaseOrderer("osrlib.Tests.PriorityOrderer", "osrlib.Tests")]
    public class SaveLoadTests //: IDisposable
    {
        private static string _saveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string _saveFile = "tbrpg-adventure.json";
        private static string _savePath = Path.Combine(_saveDir, _saveFile);

        [Fact, TestPriority(1)]
        public void SaveAdventureFile_ShouldSucceed()
        {
            // Arrange
            Adventure adventure = GetInitializedAdventure();

            // Act
            bool fileWritten = SaveLoadLocal.Save(adventure, _savePath);

            if (fileWritten)
            {
                Console.WriteLine($"File successfully written to " + _savePath);
            }

            // Assert
            Assert.True(fileWritten);
        }

        [Fact, TestPriority(2)]
        public void LoadAdventureFile_ShouldSucceed()
        {
            // Act
            Adventure loadedAdventure = SaveLoadLocal.Load(_savePath);

            // Assert
            Assert.NotNull(loadedAdventure);

            // Clean up
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
            }
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
