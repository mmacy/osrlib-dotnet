using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tbrpg.CoreRules
{
    /// <summary>
    /// The Adventure contains one or more <see cref="Dungeon"/> and <see cref="Quest"/> objects.
    /// </summary>
    public class Adventure
    {
        /// <summary>
        /// Gets or sets the active player <see cref="Party"/>.
        /// </summary>
        public Party ActiveParty { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Dungeon"/> currently being explored by the player's <see cref="Party"/>.
        /// </summary>
        public string ActiveDungeonId { get; set; }

        /// <summary>
        /// Gets the active <see cref="Dungeon"/>.
        /// </summary>
        public Dungeon ActiveDungeon { get; private set; }

        /// <summary>
        /// Gets or sets the Dungeons in the Adventure.
        /// </summary>
        public List<Dungeon> Dungeons { get; set; } = new List<Dungeon>();

        /// <summary>
        /// Gets or sets the Quests in the Adventure.
        /// </summary>
        public List<Quest> Quests { get; set; } = new List<Quest>();
        
        /// <summary>
        /// Adds the specified <see cref="Dungeon"/> to the Adventure.
        /// </summary>
        /// <param name="dungeon"></param>
        public void AddDungeon(Dungeon dungeon)
        {
            this.Dungeons.Add(dungeon);
        }

        /// <summary>
        /// Sets the active <see cref="Dungeon"/> for the Adventure.
        /// </summary>
        /// <param name="dungeon">The <see cref="Dungeon"/> to set as active.</param>
        public void SetActiveDungeon(Dungeon dungeon)
        {
            this.ActiveDungeon = dungeon;
            this.ActiveDungeonId = dungeon.DungeonId;
        }

        /// <summary>
        /// Sets the active player <see cref="Party"/> for the Adventure. This is the Party that will enter combat with this Adventure's <see cref="Dungeon"/> <see cref="Encounter"/>s.
        /// </summary>
        /// <param name="activeParty">The player's <see cref="Party"/> to set as active.</param>
        public void SetActiveParty(Party activeParty)
        {
            this.ActiveParty = activeParty;
        }
    }
}
