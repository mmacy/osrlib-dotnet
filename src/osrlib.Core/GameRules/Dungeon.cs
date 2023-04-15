namespace osrlib.Core
{
    /// <summary>
    /// The Dungeon contains and manages one or more <see cref="Encounter"/> objects, and
    /// may be associated with other Dungeons.
    /// </summary>
    public class Dungeon
    {
        /// <summary>
        /// Event raised when the <see cref="Party"/> has moved to a <see cref="GamePosition"/>
        /// that is occupied by an <see cref="Encounter"/>.
        /// </summary>
        /// <remarks>To obtain the <see cref="Encounter"/>, check the sender's <see cref="Dungeon.ActiveEncounter"/> property.</remarks>
        public event EventHandler Encountered;

        /// <summary>
        /// Gets or sets the width of the Dungeon. Default: <c>15</c>.
        /// </summary>
        public int Width { get; set; } = 15;

        /// <summary>
        /// Gets or sets the height of the dungeon. Default: <c>15</c>.
        /// </summary>
        public int Height { get; set; } = 15;

        /// <summary>
        /// Gets or sets the starting position of the <see cref="Party"/>. Default: <c>0,0</c>.
        /// </summary>
        public GamePosition StartPosition { get; set; } = new GamePosition(0, 0);

        /// <summary>
        /// Gets or sets the current <see cref="GamePosition"/> of the <see cref="Party"/> within the Dungeon. Default: <c>0,0</c>.
        /// </summary>
        public GamePosition CurrentPosition { get; set; } = new GamePosition(0, 0);

        /// <summary>
        /// Gets or sets the ID of the Dungeon.
        /// </summary>
        public string DungeonId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the next Dungeon in the <see cref="Adventure"/>.
        /// </summary>
        public int NextDungeonId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the previous Dungeon in the <see cref="Adventure"/>.
        /// </summary>
        public int PrevDungeonId { get; set; }

        /// <summary>
        /// Moves the <see cref="Party"/> to the specified position within the Dungeon.
        /// </summary>
        /// <param name="x">The position on the x axis.</param>
        /// <param name="y">The position on the y axis.</param>
        /// <param name="z">The position on the z axis. Default: <c>0</c>.</param>
        /// <returns>The new <see cref="GamePosition"/> of the <see cref="Party"/>.</returns>
        /// <remarks>To determine whether the party moves to a position occupied by an encounter, subscribe to
        /// the <see cref="Encountered"/> event.</remarks>
        public GamePosition MoveParty(int x, int y, int z = 0)
        {
            this.CurrentPosition = new GamePosition(x, y, z);

            // Check Encounters to see if there is an Encounter at this position
            Encounter enc = this.Encounters.Find(e => e.Position.Equals(this.CurrentPosition));

            // If so, call OnEncountered to raise Encountered event
            if (enc != null)
            {
                this.ActiveEncounter = enc;
                OnEncountered();
            }

            return this.CurrentPosition;
        }

        /// <summary>
        /// Raises the event signifying that the <see cref="Party"/> has moved to a <see cref="GamePosition"/>
        /// that is occupied by an <see cref="Encounter"/>.
        /// </summary>
        private void OnEncountered() => Encountered?.Invoke(this, new EventArgs());

        /// <summary>
        /// Gets or sets the active <see cref="Encounter"/>.
        /// </summary>
        public Encounter ActiveEncounter { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Encounter"/>s within the Dungeon.
        /// </summary>
        public List<Encounter> Encounters { get; set; } = new List<Encounter>();
    }
}