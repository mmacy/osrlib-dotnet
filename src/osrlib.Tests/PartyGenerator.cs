using osrlib.Core.Engine;

namespace osrlib.Tests
{
    public static class PartyGenerator
    {
        public static Party GetPlayerParty()
        {
            const int hp = 30;
            const int def = 15;

            Party party = new Party();

            foreach (CharacterClassType characterClassType in Enum.GetValues(typeof(CharacterClassType)))
            {
                CharacterClass characterClass = new()
                {
                    Class = characterClassType,
                };

                Being playerCharacter = GetBeing(characterClass.ToString(), hp, def);
                playerCharacter.Class = characterClass;
                party.AddPartyMember(playerCharacter);
            }

            return party;
        }

        public static Party GetMonsterParty()
        {
            Party party = new Party();

            int hp = 20;
            int defense = 10;

            for (int i = 0; i < 4; i++)
            {
                Being monster = GetBeing("Monster " + i.ToString(), hp, defense);
                party.AddPartyMember(monster);
            }

            return party;
        }

        private static Being GetBeing(string name, int hp, int defense)
        {
 
            Being being = new Being(name)
            {
                Defense = defense,
                HitPoints = hp,
                MaxHitPoints = hp
            };
            being.RollAbilities();

            return being;
        }
    }
}