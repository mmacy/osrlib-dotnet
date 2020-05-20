using System;
using System.Collections.Generic;
using System.Linq;

namespace tbrpg.CoreRules
{
    /// <summary>
    /// The Encounter contains a <see cref="Party"/>, can accept a <see cref="Party"/> to initiate a battle,
    /// and may contain a reward. The encounter also handles creating GameActions and assigning targets.
    /// </summary>
    public class Encounter
    {
        // Tracks the current queue of attackers in the encounter
        private Queue<Being> _attackQueue = new Queue<Being>();

        /// <summary>
        /// Event raised when a <see cref="Party"/> is added to this encounter and <see cref="GameAction"/>s begin.
        /// </summary>
        public event EventHandler EncounterStarted;

        /// <summary>
        /// Event raised when one <see cref="Party"/> within the encounter is determined the victor, or
        /// the encounter is stopped in some other manner.
        /// </summary>
        public event EventHandler EncounterEnded;

        /// <summary>
        /// Gets or sets the position within the <see cref="tbrpg.CoreRules.Dungeon"/> of this Encounter.
        /// </summary>
        public GamePosition Position { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Party"/> in this encounter.
        /// </summary>
        public Party EncounterParty { get; set; }

        /// <summary>
        /// Gets the <see cref="Party"/> of adventurers for the encounter.
        /// </summary>
        /// <remarks>This is typically the party controlled by the player.</remarks>
        public Party AdventuringParty { get; private set; }

        /// <summary>
        /// Gets whether the Encounter has been started.
        /// </summary>
        public bool IsEncounterStarted { get; private set; }

        /// <summary>
        /// Gets whether the Encounter has been completed.
        /// </summary>
        public bool IsEncounterEnded { get; private set; }

        /// <summary>
        /// Gets or sets a value that specifies whether the Encounter resolves all GameActions between
        /// Beings in the Encounter until one side is defeated.
        /// </summary>
        public bool AutoBattleEnabled { get; set; } = false;

        /// <summary>
        /// Adds the specified <see cref="Party"/> of adventurers that will perform GameActions on the <see cref="EncounterParty"/>.
        /// </summary>
        /// <param name="adventurers">The <see cref="Party"/> of adventurers to act upon the <see cref="EncounterParty"/> in this <see cref="Encounter"/>.</param>
        public void SetAdventuringParty(Party adventurers)
        {
            if (this.AdventuringParty == null)
            {
                this.AdventuringParty = adventurers;
            }
            else
            {
                throw new InvalidOperationException("An adventuring party has already been added to this Encounter.");
            }
        }

        /// <summary>
        /// Starts the Encounter. To resolve the Encounter, subscribe to all party members' <see cref="tbrpg.CoreRules.Being.PotentialTargetsAdded"/>
        /// event, and then call <see cref="PerformStep"/> to move the Encounter forward until the <see cref="EncounterEnded"/> is raised.
        /// </summary>
        /// <remarks>
        /// Before calling <see cref="StartEncounter"/>, subscribe to each party members'
        /// <see cref="tbrpg.CoreRules.Being.PotentialTargetsAdded"/> event to be notified when the caller should select a
        /// target and subsequently call <see cref="GameAction.PerformAction"/>. After PerformAction has been called, check
        /// <see cref="IsEncounterEnded"/> before calling PerformStep again. If AutoBattleEnabled is <c>true</c>, this will
        /// initiate resolution of the full Encounter (e.g. resolve all combat between Encounter parties).
        /// </remarks>
        public void StartEncounter()
        {
            if (!this.IsEncounterStarted)
            {
                this.IsEncounterStarted = true;
                this.IsEncounterEnded = false;

                // Need to notify subscribers of the EncounterEnded event when this encounter has ended,
                // which we do by being notified when one of the parties defeated.
                this.EncounterParty.Defeated += Party_Defeated;
                this.AdventuringParty.Defeated += Party_Defeated;

                OnEncounterStarted();

                // If autobattle is enabled rip through the whole battle in one shot
                if (this.AutoBattleEnabled)
                {
                    PerformAutoBattle();
                }
                else
                {
                    // Otherwise, just start the first step
                    PerformStep();
                }
            }
            else
            {
                throw new InvalidOperationException("This Encounter has already been started. Check IsEncounterStarted before starting an Encounter.");
            }
        }

        /// <summary>
        /// Handles the <see cref="Party.Defeated"/> event and raises the <see cref="EncounterEnded"/> event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event information.</param>
        private void Party_Defeated(object sender, EventArgs e)
        {
            OnEncounterEnded();
        }

        /// <summary>
        /// Moves the Encounter forward one step by dequeuing an attacker and adding as potential targets the opposing
        /// party's living members.
        /// </summary>
        /// <remarks>
        /// This method first fills the Encounter's attack queue if it's empty, then dequeues a Being and populates its
        /// <see cref="Being.PotentialTargets"/> collection with living Beings from the opposing party. Doing so raises
        /// its <see cref="Being.PotentialTargetsAdded"/> event which allows subscribers (such as a GUI application) to
        /// prompt for target selection. Targets are selected by adding calling <see cref="Being.AddSelectedTarget(Being)"/>"/>
        /// method.
        /// </remarks>
        public void PerformStep()
        {
            if (this.IsEncounterEnded)
            {
                throw new InvalidOperationException("This Encounter has already ended, and cannot be stepped. Check the IsEncounterEnded property before calling the PerformStep method.");
            }
            else if (!this.IsEncounterStarted)
            {
                throw new InvalidOperationException("This Encounter has not been started. Call StartEncounter prior to calling the PerformStep method.");
            }

            // Check attack queue, if queue empty, fill attack queue
            if (!_attackQueue.Any())
            {
                FillAttackQueue();
                Console.WriteLine($"Filled attack queue: {_attackQueue.Count} combatants");
            }

            // Dequeue an attacker and create an appropriate target list (only if the attacker is alive, of course)
            Being attacker = _attackQueue.Dequeue();
            if (attacker.IsAlive)
            {
                List<Being> targets = new List<Being>();

                if (this.AdventuringParty.LivingMembers.Contains(attacker))
                {
                    // The attacker is in the adventuring party, so the targets are monsters
                    targets.AddRange(this.EncounterParty.LivingMembers);
                }
                else if (this.EncounterParty.LivingMembers.Contains(attacker))
                {
                    // The attacker is a monster, so the targets are the adventurers
                    targets.AddRange(this.AdventuringParty.LivingMembers);
                }

                // This will raise the Being's PotentialTargetsAdded event, which can be used to
                // tell subscribers to select target(s) via Being.AddSelectedTarget, then perform
                // GameAction(s) on the selected targets via Being.PerformActionOnSelectedTargets.
                attacker.AddPotentialTargets(targets);
            }
            else
            {
                // If we fall through to here, it means that the attacker is dead, and
                // we need to just move on to the next attacker
                PerformStep();
            }
        }

        /// <summary>
        /// Fills the Encounter's attack queue with all living members of both parties. The queue is
        /// processed by calling <see cref="PerformStep"/>.
        /// </summary>
        private void FillAttackQueue()
        {
            // TODO: Fill the Encounter _attackQueue in order of initiative (right now it just fills in order of characters 0 to N, then monsters 0 to N)

            foreach (Being adventurer in this.AdventuringParty.LivingMembers)
            {
                _attackQueue.Enqueue(adventurer);
            }

            foreach (Being monster in this.EncounterParty.LivingMembers)
            {
                _attackQueue.Enqueue(monster);
            }
        }

        /// <summary>
        /// Raises the <see cref="EncounterStarted"/> event.
        /// </summary>
        private void OnEncounterStarted() => EncounterStarted?.Invoke(this, new EventArgs());

        /// <summary>
        /// Raises the <see cref="EncounterEnded"/> event.
        /// </summary>
        private void OnEncounterEnded()
        {
            this.IsEncounterEnded = true;

            EncounterEnded?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fills the attack queues, creates GameActions for each matchup, executes those GameActions, and
        /// resolves the encounter if one of the parties has been defeated.
        /// </summary>
        internal void PerformAutoBattle()
        {
            List<Being> targets = new List<Being>();

            // 1. PCs attack monsters
            foreach (Being adventurer in this.AdventuringParty.LivingMembers)
            {
                // Select a random target out of the monster party and perform game action
                targets.AddRange(this.EncounterParty.LivingMembers);

                if (targets.Any())
                {
                    int targetIndex = Utility.Randomizer.GetRandomInt(0, targets.Count - 1);

                    adventurer.AddPotentialTargets(targets);
                    adventurer.AddSelectedTarget(targets[targetIndex]);
                    adventurer.PerformActionOnSelectedTargets();
                }
            }

            // 2. Monsters attack PCs
            foreach (Being monster in this.EncounterParty.LivingMembers)
            {
                // Select a random target out of the player's party and perform game action
                targets.AddRange(this.AdventuringParty.LivingMembers);

                if (targets.Any())
                {
                    int targetIndex = Utility.Randomizer.GetRandomInt(0, targets.Count - 1);

                    monster.AddPotentialTargets(targets);
                    monster.AddSelectedTarget(targets[targetIndex]);
                    monster.PerformActionOnSelectedTargets();
                }
            }

            // This will be true if the last living member of either party was killed.
            // Since each party subscribes to each members' BeingKilled event, the party
            // knows when it's been Defeated when the last living member is killed.
            if (!this.IsEncounterEnded)
            {
                PerformAutoBattle();
            }
            else
            {
                Console.WriteLine("Encounter ended.");
            }
        }
    }
}
