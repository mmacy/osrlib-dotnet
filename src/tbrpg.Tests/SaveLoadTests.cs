using System;
using System.IO;
using Xunit;
using tbrpg.SaveLoad;
using tbrpg.CoreRules;

namespace tbrpg.Tests
{
    public class SaveLoadTests
    {
        private string _saveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private string _saveFile = "tbrpg_adventure.json";

        [Fact]
        public void SaveAdventureToFile()
        {
            Adventure adventure = GetInitializedAdventure();

            Assert.True(SaveLoadLocal.Save(adventure, Path.Combine(_saveDir, _saveFile)));
        }

        [Fact]
        public void LoadAdventureFromFile()
        {
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
            adventure.SetActiveParty(PartyGenerator.GetPlayerParty());
            adventure.AddDungeon(dungeon);
            adventure.SetActiveDungeon(dungeon);

            return adventure;
        }
    }
}
