namespace osrlib.Core
{
    /// <summary>
    /// Represents a human player. A User can own one or more <see cref="Party"/> or <see cref="Adventure"/>.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the Id of the User.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the User.
        /// </summary>
        public string Name { get; set; }
    }
}
