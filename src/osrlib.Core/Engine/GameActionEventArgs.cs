namespace osrlib.Core.Engine
{
    /// <summary>
    /// Provides information for events associated with a <see cref="GameAction"/>.
    /// </summary>
    public class GameActionEventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="GameAction"/> associated with the event.
        /// </summary>
        public GameAction Action { get; set; }
    }
}
