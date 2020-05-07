namespace tbrpg.CoreRules
{
    /// <summary>
    /// Specification defining any object that can appear in the game world that is not an <see cref="IGamePiece"/>.
    /// </summary>
    /// <remarks>A class implementing this interface indicates that the object can be wielded
    /// as an active item by an <see cref="IGamePiece"/>, such as a weapon or spell.</remarks>
    public interface IGameItem
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the die code (e.g. "1d8", "2d4") for use when computing damage dealt by this IGameItem.
        /// </summary>
        string DamageDie { get; set; }

        /// <summary>
        /// Returns a random amount of damage based on the <see cref="DamageDie"/>.
        /// </summary>
        /// <returns>The amount of damage rolled.</returns>
        int GetDamageRoll();

        /// <summary>
        /// Specifies the number of <see cref="IGamePiece"/>s that can be targeted when this IGameItem is wielded as a weapon.
        /// </summary>
        int NumberOfAttacks { get; set; }
    }
}
