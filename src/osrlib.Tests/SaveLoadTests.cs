﻿using System.IO;
using osrlib.Core.Engine;
using osrlib.SaveLoad;
using Xunit.Abstractions;

namespace osrlib.Tests
{

    [Collection("SaveLoadTests")]
    [TestCaseOrderer("osrlib.Tests.PriorityOrderer", "osrlib.Tests")]
    public class SaveLoadTests //: IDisposable
    {
        private static readonly string _saveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string _saveFile = "tbrpg-adventure.json";
        private static readonly string _savePath = Path.Combine(_saveDir, _saveFile);

        private readonly ITestOutputHelper _testOutputHelper;
        public SaveLoadTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact, TestPriority(1)]
        public void SaveAdventureFile_ShouldSucceed()
        {
            // Arrange
            Adventure adventure = GetInitializedAdventure();

            // Act
            bool fileWritten = SaveLoadLocal.Save(adventure, _savePath);

            if (fileWritten)
            {
                _testOutputHelper.WriteLine($"File successfully written to " + _savePath);
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

        private static Adventure GetInitializedAdventure()
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
