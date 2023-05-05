using osrlib.Core.Engine;
using Xunit.Sdk;

namespace osrlib.Tests
{
    public static class PartyGenerator
    {

        public static Party GetPlayerParty()
        {
            Party playerParty = new Party();
            
            const DieType hitDie = DieType.d20;
            const int def = 15;

            foreach (CharacterClassType characterClassType in Enum.GetValues(typeof(CharacterClassType)))
            {
                Being playerCharacter = GetBeing(characterClassType, hitDie, def);
                playerParty.AddPartyMember(playerCharacter);
            }

            return playerParty;
        }

        public static Party GetMonsterParty()
        {
            Party monsterParty = new Party();

            const DieType hitDie = DieType.d10;
            int defense = 10;

            for (int i = 0; i < 4; i++)
            {
                Being monster = GetBeing(CharacterClassType.Monster, hitDie, defense);
                monster.Name += i.ToString();
                monsterParty.AddPartyMember(monster);
            }

            return monsterParty;
        }

        private static Being GetBeing(CharacterClassType beingType, DieType hitDie, int defense)
        {

            // Init the Being's critical properties
            Being being = new Being(beingType.ToString())
            {
                Class = new CharacterClass(beingType),
                HitPoints = new HitPoints(hitDie),
                Defense = defense
            };

            // Roll the Being's abilities and hit points
            being.RollAbilities();
            Ability constitution = being.GetAbilityByType(AbilityType.Constitution);
            int conModifer = constitution.GetModifierValue();
            being.HitPoints.Roll(conModifer);
            
            return being;
        }
    }
}