using osrlib.CoreRules;
using System;

namespace osrlib.Tests
{
    public static class PartyGenerator
    {
        public static Party GetPlayerParty()
        {
            int hp = 30;
            int def = 15;

            Party party = new Party();

            party.AddPartyMember(GetBeing("Blarg the Destroyer", hp, def));
            party.AddPartyMember(GetBeing("Killarvo", hp, def));
            party.AddPartyMember(GetBeing("Vizplag", hp, def));
            party.AddPartyMember(GetBeing("Winglar", hp, def));
            party.AddPartyMember(GetBeing("Binzo", hp, def));
            party.AddPartyMember(GetBeing("Ceelak", hp, def));

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

            Being being = new Being
            {
                Name = name,
                Defense = defense,
                HitPoints = hp,
                MaxHitPoints = hp
            };
            being.RollAbilities();

            return being;
        }
    }
}