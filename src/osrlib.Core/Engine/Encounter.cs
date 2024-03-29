﻿namespace osrlib.Core.Engine
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
        /// Gets or sets the position within the <see cref="Dungeon"/> of this Encounter.
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
        /// Starts the Encounter. To resolve the Encounter, subscribe to all party members' <see cref="Being.PotentialTargetsAdded"/>
        /// event, and then call <see cref="PerformStep"/> to move the Encounter forward until the <see cref="EncounterEnded"/> is raised.
        /// </summary>
        /// <remarks>
        /// Before calling <see cref="StartEncounter"/>, subscribe to each party members'
        /// <see cref="Being.PotentialTargetsAdded"/> event to be notified when the caller should select a
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

                // When PerformStep() is called, the current attacker (popped off the AttackQueue) might be
                // in the monster party. In that case, we want that monster to select a character from the
                // player party at random and attack that character.
                foreach (Being monster in this.EncounterParty.Members)
                {
                    // TODO: Might want to make this an actual event delegate method so that
                    // we can unsubscribe when the encounter is ended. Right now we assume that
                    // an encounter is only ended when one party is destroyed, but that might not
                    // always be the case. We might support the party running away, for example.
                    // TODO: Might also want to make this optional - the consumer of the lib might
                    // rather have the control over how and when the monsters in their game attack.
                    monster.PotentialTargetsAdded += (s, e) =>
                    {
                        int targetIndex = Utility.Randomizer.GetRandomInt(0, monster.PotentialTargets.Count - 1);
                        monster.SelectTarget(monster.PotentialTargets[targetIndex]);
                        monster.PerformActionOnSelectedTargets();

                        // AutoBattle steps the encounter automatically, so don't want to
                        // also do that here or we get double attacks from the monsters
                        // after the first round.
                        if (!this.IsEncounterEnded && !this.AutoBattleEnabled)
                        {
                            PerformStep();
                        }
                    };
                }

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
        /// Moves the Encounter forward one step by dequeuing an attacker and adding the opposing party's living members
        /// to the attacker's potential targets collection.
        /// </summary>
        /// <remarks>
        /// This method first fills the Encounter's attack queue if it's empty, then dequeues a Being and populates its
        /// <see cref="Being.PotentialTargets"/> collection with living Beings from the opposing party. Doing so raises
        /// its <see cref="Being.PotentialTargetsAdded"/> event which allows subscribers (such as a GUI application) to
        /// prompt for target selection. Targets are selected by calling the <see cref="Being.SelectTarget(Being)"/>
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
                // tell subscribers to select target(s) via Being.SelectTarget, then perform
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
        private void OnEncounterStarted() => EncounterStarted?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Raises the <see cref="EncounterEnded"/> event.
        /// </summary>
        private void OnEncounterEnded()
        {
            this.IsEncounterEnded = true;

            EncounterEnded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fills the attack queues, creates GameActions for each match-up, executes the GameActions, and
        /// resolves the encounter when one of the parties is defeated.
        /// </summary>
        internal void PerformAutoBattle()
        {
            List<Being> targets = new List<Being>();

            while (!this.IsEncounterEnded)
            {
                // 1. PCs attack monsters
                foreach (Being adventurer in this.AdventuringParty.LivingMembers)
                {
                    // First clear out any existing targets
                    targets.Clear();

                    // Select a random target out of the monster party and perform game action
                    targets.AddRange(this.EncounterParty.LivingMembers);

                    if (targets.Any())
                    {
                        int targetIndex = Utility.Randomizer.GetRandomInt(0, targets.Count - 1);

                        adventurer.AddPotentialTargets(targets);
                        adventurer.SelectTarget(targets[targetIndex]);
                        adventurer.PerformActionOnSelectedTargets();
                    }
                }

                // 2. Monsters attack PCs
                foreach (Being monster in this.EncounterParty.LivingMembers)
                {
                    // First clear out any existing targets
                    targets.Clear();

                    targets.AddRange(this.AdventuringParty.LivingMembers);
                    if (targets.Any())
                    {
                        monster.AddPotentialTargets(targets);
                    }
                }
            }
        }
    }
}
