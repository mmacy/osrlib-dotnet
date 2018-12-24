/*************************************************************************
 * File:        AbilityModifier.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Purpose:     A Modifier is used to adjust another value (e.g. Strength
 *              ability score of 18 gives +3 Modifier to damage rolls).
 * Copyright (c) 2011 Marshall Macy II
 *************************************************************************/

namespace tbrpg.CoreRules
{
    /// <summary>
    /// Represents a modifier from a single source, such as an ability or magical bonus or penalty.
    /// </summary>
    /// <remarks>A Modifier is used to adjust another value. For example, a Strength ability score of 18 gives +3 Modifier to damage rolls.</remarks>
    public class AbilityModifier
    {
        /// <summary>
        /// A reference to the source of the modifier.
        /// </summary>
        public object ModifierSource { get; set; }

        /// <summary>
        /// Gets or sets the amount of the modifier. This value can be a positive or negative number.
        /// </summary>
        public int ModifierValue { get; set; }

        /// <summary>
        /// Gets a value indicating whether the Modifier is configured with both a source and value.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (ModifierSource != null && (ModifierValue >= 0 || ModifierValue <= 0));
            }
        }
    }
}