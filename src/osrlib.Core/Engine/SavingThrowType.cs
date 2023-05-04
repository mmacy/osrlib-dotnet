namespace osrlib.Core.Engine
{
    /// <summary>
    /// Specifies the saving throw types.
    /// </summary>
    public enum SavingThrowType
    {
        ///<summary>Resistance to death magic and poison effects.</summary>
        DeathRayOrPoison,

        ///<summary>Resistance to the effects of magical wands.</summary>
        MagicWands,

        ///<summary>Resistance to paralysis and petrification effects, including turn to stone.</summary>
        ParalysisOrTurnToStone,

        ///<summary>Resistance to breath weapon attacks, such as those from dragons or other creatures.</summary>
        DragonBreath,

        ///<summary>Resistance to the effects of magical rods, staves, and spells.</summary>
        RodsStavesOrSpells
    }
}
