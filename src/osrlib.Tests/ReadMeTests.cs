using Xunit.Abstractions;

namespace osrlib.Tests
{
    /// <summary>
    /// ***********************************************************************************
    /// WARNING: You MUST backfill any changes to this file to the repo's README.md file!
    /// ***********************************************************************************
    /// The code in these tests back the code snippets in the OSRLib.NET repository README:
    /// https://github.com/mmacy/osrlib-dotnet/blob/main/README.md
    ///
    /// Each section of the README has a matching #region, and the README contains HTML comments
    /// that match match these region names.
    /// </summary>
    public class ReadMeTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ReadMeTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestReadMeSnippets()
        {
            #region SECTION_CREATE_A_CHARACTER
            // Get a ten-sided die ready
            DiceRoll roll = new DiceRoll(new DiceHand(1, DieType.d10));

            // Roll up a fighter-type character
            Being fighter = new Being("Blarg the Destructor")
            {
                Defense = roll.RollDice(),
                MaxHitPoints = roll.RollDice() + 10
            };
            fighter.HitPoints = fighter.MaxHitPoints;
            fighter.RollAbilities();

            // Give Blarg a sweet sword
            Weapon magicSword = new Weapon
            {
                Name = "Long Sword + 1",
                Description = "A finely crafted sword, its blade dimly glows.",
                Type = WeaponType.Melee,
                DamageDie = new DiceHand(1, DieType.d8)
            };
            magicSword.AttackModifiers.Add(new Modifier(magicSword, 1));
            magicSword.DamageModifiers.Add(new Modifier(magicSword, 1));
            fighter.ActiveWeapon = magicSword;

            // Now, add the fighter to the player's party
            Party playerParty = new Party();
            playerParty.AddPartyMember(fighter);
            #endregion

            #region SECTION_STOCK_THE_DUNGEON
            Dungeon dungeon = new Dungeon();

            // Create some monsters for an encounter
            Being goblin1 = new Being("Goblin Chieftain")
            {
                Defense = 10,
                HitPoints = 10,
                MaxHitPoints = 10
            };
            goblin1.RollAbilities();

            Being goblin2 = new Being("Goblin")
            {
                Defense = 5,
                HitPoints = 4,
                MaxHitPoints = 4
            };
            goblin2.RollAbilities();

            // Add the goblins to the monster party
            Party monsterParty = new Party();
            monsterParty.AddPartyMember(goblin1);
            monsterParty.AddPartyMember(goblin2);

            // Add the monsters to an encounter
            Encounter encounter = new Encounter
            {
                EncounterParty = monsterParty,
                Position = new GamePosition(10, 10)
            };

            // Add the encounter to the dungeon
            dungeon.Encounters.Add(encounter);
            #endregion

            #region SECTION_BATTLE_SETUP
            // OSRlib is heavily event-driven and most top-level classes expose public events. Determine
            // when and how to change the state of your game at runtime (prompt for target selection,
            // play a sound when a monster is killed, etc.) by subscribing to events exposed by objects
            // that you create.
            encounter.EncounterStarted += (sender, eventArgs) =>
                {
                    _testOutputHelper.WriteLine($"Encounter has started! Monsters:\r\n{((Encounter)sender).EncounterParty}");
                };

            // Example of subscribing to an event that you might use to update the UI state to notify
            // the player or make some other changes within your application.
            encounter.EncounterEnded += (sender, eventArgs) =>
                {
                    Encounter enc = sender as Encounter;

                    if (enc.AdventuringParty.IsAlive)
                    {
                        Console.WriteLine("Your party has won the battle!");
                    }
                    else if (enc.EncounterParty.IsAlive)
                    {
                        Console.WriteLine("Sorry, your party has been vanquished.");
                    }
                };
            #endregion

            #region SECTION_BATTLE_START
            // Encounters can be set to auto-resolve the battle. This is OPTIONAL! In your game, you
            // typically wouldn't want to set this to true and instead allow your player to select the
            // target of their attack.
            encounter.AutoBattleEnabled = true;

            // Add the adventuring party to the encounter
            encounter.SetAdventuringParty(playerParty);

            // Subscribe to events on the combatants so we can respond to things that happen to them.
            List<Being> combatants = encounter.AdventuringParty.Members.Concat(encounter.EncounterParty.Members).ToList();
            foreach (Being combatant in combatants)
            {
                combatant.SelectedAsTarget += (s, e) =>
                    {
                        Being attackedBeing = s as Being;
                        BeingTargetingEventArgs args = e as BeingTargetingEventArgs;

                        Console.WriteLine($"{e.TargetingBeing} attacks {attackedBeing} with their {e.TargetingBeing.ActiveWeapon}...");
                    };
                combatant.ActionPerformed += (s, e) =>
                    {
                        GameActionEventArgs actionArgs = e as GameActionEventArgs;
                        GameAction action = actionArgs.Action;

                        if (action.Victor.Equals(combatant))
                        {
                            Console.WriteLine($"{combatant} rolled a {action.AttackRoll} and hit for {action.DamageRoll} points of damage.");
                        }
                        else
                        {
                            Console.WriteLine($"{combatant} rolled a {action.AttackRoll} and missed.");
                        }
                    };
                combatant.Killed += (s, e) =>
                    {
                        Console.WriteLine($"{((Being)s).Name} was killed!");
                    };
            }

            // Start the battle. This will fire the EncounterStarted event we subscribed to above, and
            // since we set this encounter to resolve all combat automatically with AutoBattleEnabled,
            // each member of both parties takes turns attacking each other until one side has been
            // defeated.
            encounter.StartEncounter();
            #endregion
        }
    }
}
