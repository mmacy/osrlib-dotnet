namespace osrlib.Core.Engine
{
    /// <summary>
    /// Specifies the character class types.
    /// </summary>
    /// <remarks>
    /// Class types aren't restricted to player character classes and include NPC and monster types.
    /// </remarks>
    /// <example>
    /// The CharacterClassType enum values correspond to the default hit die (HD) for each character type so you can
    /// determine a character's hit die based on their class, like so:
    ///
    /// <code>
    /// playerCharacter.Class.ClassType = CharacterClassType.Fighter;
    /// playerCharacter.Class.HitDieType = (DieType)(int)playerCharacter.Class.ClassType;
    /// </code>
    /// </example>
    public enum CharacterClassType
    {
        /// <summary>
        /// No character class assigned.
        /// </summary>
        None = DieType.d1,
        
        /// <summary>
        /// A strong and skilled warrior with expertise in using a variety of weapons and armor.
        /// </summary>
        Fighter = DieType.d8,

        /// <summary>
        /// A versatile spellcaster with access to a wide range of arcane spells, but limited in physical combat.
        /// </summary>
        MagicUser = DieType.d4,

        /// <summary>
        /// A divine spellcaster, skilled in healing and banishing evil, while also being proficient in combat.
        /// </summary>
        Cleric = DieType.d6,

        /// <summary>
        /// A cunning and agile adventurer with abilities in stealth, thievery, and surprise attacks.
        /// </summary>
        Thief = DieType.d4,

        /// <summary>
        /// A combination of Fighter and Magic-User, able to wield weapons and cast arcane spells, but with limitations in both areas.
        /// </summary>
        Elf = DieType.d6,

        /// <summary>
        /// A combination of Fighter and Thief, skilled in combat and stealth abilities, but limited in both areas.
        /// </summary>
        Halfling = DieType.d6,

        /// <summary>
        /// A combination of Fighter and Cleric, proficient in both combat and divine spellcasting, but limited in both areas.
        /// </summary>
        Dwarf = DieType.d8,
        
        /// <summary>
        /// A non-player character (NPC).
        /// </summary>
        NPC = DieType.d1,
        
        /// <summary>
        /// A creature that is neither player character (PC) nor non-player character (NPC).
        /// </summary>
        Monster = DieType.d4
    }
}
