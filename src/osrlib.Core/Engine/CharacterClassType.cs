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
        /// No character class assigned.
        /// </summary>
        None,
        
        /// <summary>
        /// A strong and skilled warrior with expertise in using a variety of weapons and armor.
        /// </summary>
        Fighter,

        /// <summary>
        /// A versatile spellcaster with access to a wide range of arcane spells, but limited in physical combat.
        /// </summary>
        MagicUser,

        /// <summary>
        /// A divine spellcaster, skilled in healing and banishing evil, while also being proficient in combat.
        /// </summary>
        Cleric,

        /// <summary>
        /// A cunning and agile adventurer with abilities in stealth, thievery, and surprise attacks.
        /// </summary>
        Thief,

        /// <summary>
        /// A combination of Fighter and Magic-User, able to wield weapons and cast arcane spells, but with limitations in both areas.
        /// </summary>
        Elf,

        /// <summary>
        /// A combination of Fighter and Thief, skilled in combat and stealth abilities, but limited in both areas.
        /// </summary>
        Halfling,

        /// <summary>
        /// A combination of Fighter and Cleric, proficient in both combat and divine spellcasting, but limited in both areas.
        /// </summary>
        Dwarf,
        
        /// <summary>
        /// A non-player character (NPC).
        /// </summary>
        NPC,
        
        /// <summary>
        /// A creature that is neither player character (PC) nor non-player character (NPC).
        /// </summary>
        Monster
    }
}
