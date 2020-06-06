using System;
using System.IO;
using Xunit;
using osrlib.SaveLoad;
using osrlib.Core;

namespace osrlib.Tests
{
    public class SaveLoadTests
    {
        private string _saveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string _saveFile = "tbrpg-adventure.json";

        [Fact]
        public void SaveAndLoadAdventureFile()
        {
            // Since xUnit runs all tests in parallel, we can't guarantee that the save-to-file test would be
            // completed prior to the load-from-file test, so run them both here so that the load-from-file has
            // a file to load.

            Adventure adventure = GetInitializedAdventure();
            Assert.True(SaveLoadLocal.Save(adventure, Path.Combine(_saveDir, _saveFile)));

            Adventure loadedAdventure = SaveLoadLocal.Load(Path.Combine(_saveDir, _saveFile));
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
