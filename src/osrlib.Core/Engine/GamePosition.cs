namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents an object's location within a <see cref="Dungeon"/>.
    /// </summary>
    public struct GamePosition
    {
        /// <summary>
        /// Creates a new instance of GamePosition with the specified coordinates.
        /// </summary>
        /// <param name="x">The position on the X axis.</param>
        /// <param name="y">The position on the y axis.</param>
        /// <param name="z">The position on the z axis.</param>
        public GamePosition(int x, int y, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// The position on the x axis.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The position on the y axis.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// The position on the z axis.
        /// </summary>
        public int Z { get; set; }
    }
}
