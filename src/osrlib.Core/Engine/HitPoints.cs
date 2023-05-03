using System.Collections.Generic;
using System.Linq;
using osrlib.Core;

namespace osrlib.Domain
{
    /// <summary>
    /// Represents hit points for a character or creature, including base hit points, modifiers, and current wounds.
    /// </summary>
    public class HitPoints
    {
        /// <summary>
        /// Gets or sets the base hit points for the character or creature.
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Gets or sets the collection of modifiers affecting the character or creature's hit points.
        /// </summary>
        public List<Modifier> Modifiers { get; init; } = new();

        /// <summary>
        /// Gets or sets the number of hit points lost due to wounds on the character or creature.
        /// </summary>
        public int Wounds { get; set; }

        /// <summary>
        /// Gets the current hit points by adding all the modifiers to the base and then subtracting the wounds.
        /// </summary>
        public int Current => Base + Modifiers.Sum(m => m.ModifierValue) - Wounds;
    }
}
