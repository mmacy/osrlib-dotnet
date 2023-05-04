namespace osrlib.Core.Engine
{
    /// <summary>
    /// Represents hit points for a character or monster, including base hit points, modifiers, and current wounds.
    /// </summary>
    public class HitPoints
    {
        /// <summary>
        /// Creates a new instance of HitPoints with the specified base value.
        /// </summary>
        /// <param name="baseHitPoints">The base number of hit points.</param>
        public HitPoints(int baseHitPoints)
        {
            Base = baseHitPoints;
        }

        /// <summary>
        /// Gets or sets the base hit points for the character or monster.
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Gets or sets the collection of modifiers affecting the character or monster's hit points.
        /// </summary>
        public List<Modifier> Modifiers { get; init; } = new();

        /// <summary>
        /// Gets or sets the number of hit points lost due to wounds on the character or monster.
        /// </summary>
        public int Wounds { get; set; }

        /// <summary>
        /// Gets the maximum hit points by adding all the modifiers to the base.
        /// </summary>
        public int Maximum => Base + Modifiers.Sum(m => m.ModifierValue);
        
        /// <summary>
        /// Gets the current hit points by adding all the modifiers to the base and then subtracting the wounds.
        /// </summary>
        public int Current => Base + Modifiers.Sum(m => m.ModifierValue) - Wounds;
    }
}
