namespace tbrpg.CoreRules
{
    /// <summary>
    /// Represents a modifier from a single source, such as an ability or magical bonus or penalty.
    /// </summary>
    /// <remarks>A Modifier is used to adjust another value. For example, a Strength ability score of 18 gives +3 Modifier on to-hit and damage rolls.</remarks>
    public class Modifier
    {
        /// <summary>
        /// A reference to the source of the modifier.
        /// </summary>
        public object ModifierSource { get; set; }

        /// <summary>
        /// Gets or sets the amount of the modifier. This value can positive (bonus, enchantment) or negative (penalty, curse).
        /// </summary>
        public int ModifierValue { get; set; }
    }
}