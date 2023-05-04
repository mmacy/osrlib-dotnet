using osrlib.Core.Engine;

namespace osrlib.Tests
{
    public static class PartyGenerator
    {
        public static Party GetPlayerParty()
        {
            const DieType hitDie = DieType.d20;
            const int def = 15;

            Party party = new Party();

            foreach (CharacterClassType characterClassType in Enum.GetValues(typeof(CharacterClassType)))
            {
                CharacterClass characterClass = new CharacterClass(characterClassType);
                Being playerCharacter = GetBeing(characterClass.Class, hitDie, def);
                playerCharacter.Class = characterClass;
                party.AddPartyMember(playerCharacter);
            }

            return party;
        }

        public static Party GetMonsterParty()
        {
            Party party = new Party();

            const DieType hitDie = DieType.d10;
            int defense = 10;

            for (int i = 0; i < 4; i++)
            {
                Being monster = GetBeing(CharacterClassType.Monster, hitDie, defense);
                monster.Class = new CharacterClass(CharacterClassType.Monster);
                party.AddPartyMember(monster);
            }

            return party;
        }

        private static Being GetBeing(CharacterClassType beingType, DieType hitDie, int defense)
        {
 
            Being being = new Being(beingType.ToString())
            {
                Class = new CharacterClass(beingType),
                Defense = defense,
                HitPoints = new HitPoints(hitDie)
            };
            being.RollAbilities();
            
            // Roll the Being's hit points and include the the Constitution modifier, if any
            Ability constitution = being.GetAbilityByType(AbilityType.Constitution);
            Modifier conModifer = new Modifier(constitution, constitution.GetModifierValue());
            being.HitPoints = new HitPoints(being.Class.HitDie);
            being.HitPoints.Roll(conModifer.ModifierValue);
            
            return being;
        }

        /// <summary>
        /// Gets the Constitution modifier value from the given set of abilities.
        /// </summary>
        /// <param name="being">The Being whose Constitution modifier should be returned.</param>
        /// <returns>A integer between -3 and 3 (inclusive of 0), or 0 if the Being's Abilities collection is empty.</returns>
        internal static int GetConstitutionModifierValue(Being being)
        {
            Ability constitution = being.GetAbilityByType(AbilityType.Constitution);

            if (constitution != null)
            {
                return constitution.GetModifierValue();
            }
            else
            {
                return 0;
            }
            
        }
    }
}