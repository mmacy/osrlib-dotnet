namespace osrlib.Core.Engine
{
    /// <summary>
    /// Specifies the character class types.
    /// </summary>
    /// <remarks>
    /// Class types aren't restricted to player character classes and include NPC and monster types.
    /// </remarks>
    public enum CharacterClassType
    {
        /// <summary>
        /// A strong, skilled warrior with expertise in martial weapons and armor.
        /// </summary>
        Fighter,

        /// <summary>
        /// A caster of powerful arcane spells, but limited in physical combat.
        /// </summary>
        MagicUser,

        /// <summary>
        /// A divine spellcaster, skilled in healing and "buffing," in addition to melee combat.
        /// </summary>
        Cleric,

        /// <summary>
        /// A cunning and agile adventurer with abilities in stealth, thievery, and surprise attacks.
        /// </summary>
        Thief,

        /// <summary>
        /// A combination of Fighter and Magic User, able to wield weapons and cast arcane spells, but with some restrictions in both.
        /// </summary>
        Elf,

        /// <summary>
        /// A combination of Fighter and Thief, skilled in combat and stealth abilities, but limited in both areas.
        /// </summary>
        Halfling,

        /// <summary>
        /// A combination of Fighter and Cleric with skills incombat and divine spellcasting (at higher levels), but with some restrictions in boths.
        /// </summary>
        Dwarf,
        
        /// <summary>
        /// A non-player character (NPC).
        /// </summary>
        NPC,
        
        /// <summary>
        /// A living or undead entity that's neither player character (PC) nor non-player character (NPC).
        /// </summary>
        Monster
    }
}
