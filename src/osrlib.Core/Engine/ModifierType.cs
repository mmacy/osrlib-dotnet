namespace osrlib.Core.Engine
{
    /// <summary>
    /// Indicates the source or nature of a modifier.
    /// </summary>
    public enum ModifierType
    {
        /// <summary>
        /// Indicates the modifier originates from an enchantment like a bonus granted by a magic item.
        /// </summary>
        Enchantment,

        /// <summary>
        /// Indicates the modifier originates from a curse like a penalty imposed by a cursed item or a harmful spell.
        /// </summary>
        Curse,

        /// <summary>
        /// Indicates the modifier originates from an ability like a bonus or penalty based on a character's ability score.
        /// </summary>
        Ability,

        /// <summary>
        /// Indicates the modifier originates from a racial trait like a bonus or penalty inherent to a character's race.
        /// </summary>
        Racial,

        /// <summary>
        /// Indicates the modifier originates from a spell like a bonus or penalty granted by a beneficial or harmful incantation.
        /// </summary>
        Spell,

        /// <summary>
        /// Indicates the modifier originates from a scroll like a bonus or penalty granted by a magical effect activated by a scroll.
        /// </summary>
        Scroll,
        
        /// <summary>
        /// Indicates the modifier originates from a potion like a bonus or penalty granted by a magical effect activated by consuming a potion.
        /// </summary>
        Potion
    }
}
