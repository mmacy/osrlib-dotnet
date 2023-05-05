using osrlib.Core.Engine;
using Xunit.Abstractions;

namespace osrlib.Tests
{
    /// <summary>
    /// Tests the functionality of the <see cref="Encounter.PerformStep"/> method is used to
    /// progress the battle instead of using <c>Encounter.AutoBattleEnabled = true</c>.
    /// </summary>
    public class SteppedBattleTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SteppedBattleTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestSteppedBattle()
        {
            #region CHARACTER_SETUP

            // Get our dice hands ready
            DiceRoll roll1d10 = new DiceRoll(new DiceHand(1, DieType.d10));
            DiceRoll roll1d6 = new DiceRoll(new DiceHand(1, DieType.d6));

            // Roll up a fighter-type character
            Being fighter = new Being("Blarg the Destructor")
            {
                Class = new(CharacterClassType.Fighter),
                Defense = roll1d10.RollDice() + 5,
                HitPoints = new(DieType.d8)
            };
            fighter.RollAbilities();
            Ability constitution = fighter.GetAbilityByType(AbilityType.Constitution);
            fighter.HitPoints.Roll(constitution.GetModifierValue());

            // Give Blarg a sweet sword
            Weapon magicSword = new Weapon
            {
                Name = "Long Sword + 1",
                Description = "A finely crafted sword, its blade dimly glows.",
                Type = WeaponType.Melee,
                DamageDie = new(1, DieType.d8)
            };
            magicSword.AttackModifiers.Add(new Modifier(magicSword, 1));
            magicSword.DamageModifiers.Add(new Modifier(magicSword, 1));
            fighter.ActiveWeapon = magicSword;
            
            PrintCharacter(fighter);
            _testOutputHelper.WriteLine("---------------");

            // Roll up a wizard-type character
            Being wizard = new Being("Merlin")
            {
                Class = new(CharacterClassType.MagicUser),
                Defense = roll1d6.RollDice(),
                HitPoints = new(DieType.d4)
            };
            wizard.RollAbilities();
            Ability constitutionWiz = wizard.GetAbilityByType(AbilityType.Constitution);
            wizard.HitPoints.Roll(constitutionWiz.GetModifierValue());

            // Give Merlin a sweet fireball spell
            Weapon fireball = new Weapon
            {
                Name = "Fireball",
                Description = "Launches a flaming ball that explodes upon impact.",
                Type = WeaponType.Spell,
                DamageDie = new(1, DieType.d12),
            };
            wizard.ActiveWeapon = fireball;
            
            PrintCharacter(wizard);
            _testOutputHelper.WriteLine("---------------");
            
            // Now, add the characters to the player's party
            Party playerParty = new Party();
            playerParty.AddPartyMember(fighter);
            playerParty.AddPartyMember(wizard);

            // Subscribe to the the character's PotentialTargetsAdded event. When the encounter is
            // stepped with PerformStep(), the next attacker in the queue has their potential targets
            // collection filled, at which time they should select target(s).
            foreach (Being character in playerParty.Members)
            {
                character.PotentialTargetsAdded += (s, e) =>
                {
                    // This is typically where you would request player input in your UI to select
                    // target(s) to attack. Here, however, we'll just print out the targets that were
                    // added to the character's potential target collection.

                    Being character = s as Being;
                    _testOutputHelper.WriteLine(
                        $"Potential targets added for {character.Name} - ready for selecting targets with Being.SelectTarget().");

                    foreach (Being target in character.PotentialTargets)
                    {
                        _testOutputHelper.WriteLine($".... {target}");
                    }
                };
            }

            #endregion

            #region DUNGEON_SETUP

            Dungeon dungeon = new Dungeon();

            Party monsterParty = new Party();

            // Create some monsters for an encounter
            Being goblin1 = new Being("Goblin Chieftain")
            {
                Class = new(CharacterClassType.Monster),
                Defense = 10,
                HitPoints = new(DieType.d6),
                ActiveWeapon = new Weapon
                    { Name = "Battle Axe", Type = WeaponType.Melee, DamageDie = new DiceHand(1, DieType.d12) }
            };
            goblin1.RollAbilities();
            Ability constitutionGoblin1 = goblin1.GetAbilityByType(AbilityType.Constitution);
            goblin1.HitPoints.Roll(constitutionGoblin1.GetModifierValue());

            monsterParty.AddPartyMember(goblin1);

            for (int i = 0; i < 10; i++)
            {
                Being goblin = new Being("Goblin Soldier")
                {
                    Class = new(CharacterClassType.Monster),
                    Defense = 5,
                    HitPoints = new(DieType.d6),
                    ActiveWeapon = new Weapon
                        { Name = "Short Sword", Type = WeaponType.Melee, DamageDie = new DiceHand(1, DieType.d6) }
                };
                goblin.RollAbilities();
                Ability constitutionGoblin = goblin.GetAbilityByType(AbilityType.Constitution);
                goblin.HitPoints.Roll(constitutionGoblin.GetModifierValue());

                monsterParty.AddPartyMember(goblin);
            }

            // Add the monsters to an encounter
            Encounter encounter = new Encounter
            {
                EncounterParty = monsterParty,
                Position = new GamePosition(10, 10)
            };

            // Add the encounter to the dungeon
            dungeon.Encounters.Add(encounter);

            #endregion

            #region BATTLE_SETUP

            // OSRlib is heavily event-driven and most major entities have public events. Subscribe to events
            // like these to change the state of your UI and/or prompt the player for action (such as selecting
            // a target to attack during an encounter).
            encounter.EncounterStarted += (sender, eventArgs) =>
            {
                _testOutputHelper.WriteLine(
                    $"Encounter has started! Monsters:\r\n{((Encounter)sender).EncounterParty}");
            };

            // Example of subscribing to an event that you might use to update the UI state to notify the player
            // or make some other changes within your application.
            encounter.EncounterEnded += (sender, eventArgs) =>
            {
                Encounter enc = sender as Encounter;

                if (enc.AdventuringParty.IsAlive)
                {
                    _testOutputHelper.WriteLine("Your party has won the battle!");
                }
                else if (enc.EncounterParty.IsAlive)
                {
                    _testOutputHelper.WriteLine("Sorry, your party has been vanquished.");
                }
            };

            #endregion

            #region SECTION_BATTLE_START

            // Add the adventuring party to the encounter.
            encounter.SetAdventuringParty(playerParty);

            // Subscribe to some events on the combatants so we can respond to things that happen to them.
            List<Being> combatants =
                encounter.AdventuringParty.Members.Concat(encounter.EncounterParty.Members).ToList();
            foreach (Being combatant in combatants)
            {
                combatant.SelectedAsTarget += (s, e) =>
                {
                    Being attackedBeing = s as Being;
                    BeingTargetingEventArgs args = e as BeingTargetingEventArgs;

                    _testOutputHelper.WriteLine(
                        $"{e.TargetingBeing} attacks {attackedBeing} with their {e.TargetingBeing.ActiveWeapon}...");
                };
                combatant.ActionPerformed += (s, e) =>
                {
                    GameActionEventArgs actionArgs = e as GameActionEventArgs;
                    GameAction action = actionArgs.Action;

                    if (action.Victor.Equals(combatant))
                    {
                        _testOutputHelper.WriteLine(
                            $"{combatant} rolled a {action.AttackRoll} and hit for {action.DamageRoll} points of damage.");
                    }
                    else
                    {
                        _testOutputHelper.WriteLine($"{combatant} rolled a {action.AttackRoll} and missed.");
                    }
                };
                combatant.Killed += (s, e) => { _testOutputHelper.WriteLine($"{((Being)s).Name} was killed!"); };
            }

            // Start the battle. This will fire the EncounterStarted event we subscribed to above.
            encounter.StartEncounter();

            while (!encounter.IsEncounterEnded)
            {
                if (fighter.IsAlive)
                {
                    fighter.SelectTarget(fighter.PotentialTargets.First());
                    fighter.PerformActionOnSelectedTargets();

                    if (!encounter.IsEncounterEnded)
                    {
                        encounter.PerformStep();
                    }
                }

                if (wizard.IsAlive)
                {
                    // The wizard spell Fireball can attack all targets in the monster party
                    wizard.SelectTargets(wizard.PotentialTargets.ToList());
                    wizard.PerformActionOnSelectedTargets();

                    if (!encounter.IsEncounterEnded)
                    {
                        encounter.PerformStep();
                    }
                }
            }

            #endregion
        }

        private void PrintCharacter(Being character)
        {
            _testOutputHelper.WriteLine(character.Name);
            _testOutputHelper.WriteLine($"  Class:   {character.Class}");
            _testOutputHelper.WriteLine($"  Hit die: {character.HitPoints.HitDie}");
            _testOutputHelper.WriteLine($"  HP:      {character.HitPoints}");
            _testOutputHelper.WriteLine($"  AC:      {character.Defense}");
            _testOutputHelper.WriteLine($"  Weapon:  {character.ActiveWeapon} ({character.ActiveWeapon.DamageDie})");

            foreach (var ability in character.Abilities)
            {
                _testOutputHelper.WriteLine(ability.ToString());
            }
        }
    }
}