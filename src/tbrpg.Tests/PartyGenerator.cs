using tbrpg.CoreRules;
using System;

namespace tbrpg.Tests
{
    public static class PartyGenerator
    {
        private static int pc_hp = 30;
        private static int pc_df = 15;

        private static int mn_hp = 20;
        private static int mn_df = 10;

        public static Party GetPlayerParty()
        {
            Party party = new Party();

            int hp = pc_hp;
            int def = pc_df;

            party.AddPartyMember(GetBeing("Blarg the Destroyer", hp, def));
            party.AddPartyMember(GetBeing("Draflonduh", hp, def));
            party.AddPartyMember(GetBeing("Vizplag", hp, def));
            party.AddPartyMember(GetBeing("Wangdu", hp, def));
            party.AddPartyMember(GetBeing("Miss Cinnamon", hp, def));
            party.AddPartyMember(GetBeing("Fred Garvin", hp, def));

            return party;
        }

        public static Party GetMonsterParty()
        {
            Party party = new Party();

            for (int i = 0; i < 4; i++)
            {
                Being monster = GetBeing("Monster " + i.ToString(), mn_hp, mn_df);
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
                MaxHitPoints = hp,
                IsTargetable = true
            };

            foreach (Ability ability in being.RollAbilities())
            {
               Console.WriteLine($"{being.Name} - {ability.ToString()}");
            }

            return being;
        }
    }
}