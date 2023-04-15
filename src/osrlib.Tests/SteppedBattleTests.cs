namespace osrlib.Tests
{
    /// <summary>
    /// Tests the functionality of the <see cref="Encounter.PerformStep"/> method is used to
    /// progress the battle instead of using <c>Encounter.AutoBattleEnabled = true</c>.
    /// </summary>
    public class SteppedBattleTests
    {
        [Fact]
        public void TestSteppedBattle()
        {
            #region CHARACTER_SETUP
            // Get our dice hands ready
            DiceRoll roll1d10 = new DiceRoll(new DiceHand(1, DieType.d10));
            DiceRoll roll1d6 = new DiceRoll(new DiceHand(1, DieType.d6));

            // Roll up a fighter-type character
            Being fighter = new Being
            {
                Name = "Blarg the Destructor",
                Defense = roll1d10.RollDice() + 5,
                MaxHitPoints = roll1d10.RollDice() + 10
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
            magicSword.AttackModifiers.Add(new Modifier { ModifierSource = magicSword, ModifierValue = 1 });
            magicSword.DamageModifiers.Add(new Modifier { ModifierSource = magicSword, ModifierValue = 1 });
            fighter.ActiveWeapon = magicSword;

            // Roll up a wizard-type character
            Being wizard = new Being
            {
                Name = "Merlin",
                Defense = roll1d6.RollDice(),
                MaxHitPoints = roll1d6.RollDice()
            };
            wizard.HitPoints = wizard.MaxHitPoints;
            wizard.RollAbilities();

            // Give Merlin a sweet fireball spell
            Weapon fireball = new Weapon
            {
                Name = "Fireball",
                Description = "Launches a flaming ball that explodes upon impact.",
                Type = WeaponType.Spell,
                DamageDie = new DiceHand(1, DieType.d12),
            };
            wizard.ActiveWeapon = fireball;

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
                    Console.WriteLine($"Potential targets added for {character.Name} - ready for selecting targets with Being.SelectTarget().");

                    foreach (Being target in character.PotentialTargets)
                    {
                        Console.WriteLine($".... {target}");
                    }
                };
            }
            #endregion

            #region DUNGEON_SETUP
            Dungeon dungeon = new Dungeon();

            Party monsterParty = new Party();

            // Create some monsters for an encounter
            Being goblin1 = new Being
            {
                Name = "Goblin Chieftain",
                Defense = 10,
                MaxHitPoints = DiceRoll.RollDice(new DiceHand(4, DieType.d6)),
                ActiveWeapon = new Weapon { Name = "Battle Axe", Type = WeaponType.Melee, DamageDie = new DiceHand(1, DieType.d12) }
            };
            goblin1.HitPoints = goblin1.MaxHitPoints;
            goblin1.RollAbilities();
            monsterParty.AddPartyMember(goblin1);

            for (int i = 0; i < 10; i++)
            {
                Being goblin = new Being
                {
                    Name = "Goblin Soldier",
                    Defense = 5,
                    MaxHitPoints = DiceRoll.RollDice(new DiceHand(1, DieType.d6)),
                    ActiveWeapon = new Weapon { Name = "Short Sword", Type = WeaponType.Melee, DamageDie = new DiceHand(1, DieType.d6) }
                };
                goblin.HitPoints = goblin.MaxHitPoints;
                goblin.RollAbilities();

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
                    Console.WriteLine($"Encounter has started! Monsters:\r\n{((Encounter)sender).EncounterParty}");
                };

            // Example of subscribing to an event that you might use to update the UI state to notify the player
            // or make some other changes within your application.
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
            // Add the adventuring party to the encounter.
            encounter.SetAdventuringParty(playerParty);

            // Subscribe to some events on the combatants so we can respond to things that happen to them.
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

            // Start the battle. This will fire the EncounterStarted event we subscribed to above.
            encounter.StartEncounter();

            while (!encounter.IsEncounterEnded)
            {
                if (fighter.IsAlive)
                {
                    fighter.SelectTarget(fighter.PotentialTargets[0]);
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
    }
}
