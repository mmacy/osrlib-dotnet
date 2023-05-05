using osrlib.Core.Engine;

namespace osrlib.Core
{
    /// <summary>
    /// Specifies the type of an <see cref="Ability"/>.
    /// </summary>
    public enum AbilityType
    {
        ///<summary>Power, force.</summary>
        Strength,
        ///<summary>Nimbleness, balance.</summary>
        Dexterity,
        ///<summary>Hardiness, resilience.</summary>
        Constitution,
        ///<summary>Intellectual aptitude.</summary>
        Intelligence,
        ///<summary>Worldly experience, street smarts.</summary>
        Wisdom,
        ///<summary>Likeability, persuasiveness.</summary>
        Charisma
    }
}