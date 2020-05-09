namespace tbrpg.CoreRules
{
    /// <summary>
    /// Specifies the type of weapon or whether the weapon is an offensive (attack) spell.
    /// Used in determining which ability modifiers to apply to attack and damage rolls,
    /// as well as whether a target is within attack range.
    /// </summary>
    public enum WeaponType
    {
        /// <summary>
        /// Martial weapon wielded in close combat. Examples: sword, axe, mace, spear.
        /// </summary>
        Melee,
        /// <summary>
        /// Martial weapon with extended attack range. Examples: bow, javelin, dart, throwing axe.
        /// </summary>
        Ranged,
        /// <summary>
        /// Offensive (attack) spell. All offensive spells are considered ranged weapons.
        /// </summary>
        Spell
    }
}