using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using tbrpg.Dice;

namespace tbrpg.CoreRules
{
    /// <summary>
    /// The Being represents a living entity in within an <see cref="Adventure"/>, and is used for both player characters and monsters.
    /// </summary>
    public class Being : IGamePiece
    {
        private DiceRoll _attackRoll = new DiceRoll("1d20");

        /// <summary>
        /// Event raised when the Being's HitPoints reach zero or below.
        /// </summary>
        public event EventHandler Killed;

        /// <summary>
        /// Event raised when the Being's <see cref="PotentialTargets"/> collection is populated.
        /// </summary>
        public event EventHandler PotentialTargetsAdded;

        /// <summary>
        /// Event raised when this Being selects a target with <see cref="AddSelectedTarget(Being)"/>.
        /// </summary>
        public event BeingTargetingEventHandler TargetSelected;

        /// <summary>
        /// Event raised when this Being has been selected as a target; that is, the
        /// targeting Being's <see cref="Being.AddSelectedTarget(Being)"/> was called with
        /// this Being as the target.
        /// </summary>
        public event BeingTargetingEventHandler SelectedAsTarget;

        #region Public properties
        /// <summary>
        /// Gets or sets the name of the Being.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Being's <see cref="CharacterClass"/>.
        /// </summary>
        /// <remarks>This can be a player character class or a monster type.</remarks>
        public CharacterClass Class { get; set; }

        /// <summary>
        /// Gets or sets the Alignment of the Being.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets the Being's <see cref="Ability"/> collection.
        /// </summary>
        public List<Ability> Abilities { get; set; }

        /// <summary>
        /// Gets or sets the number of hit points for the Being.
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// Gets or sets the maximum hit points for the Being.
        /// </summary>
        public int MaxHitPoints { get; set; }

        /// <summary>
        /// Gets or sets the number of experience points for the Being.
        /// </summary>
        /// <remarks>This is the amount of experience possessed by a player character or the XP value of a monster.</remarks>
        public int ExperiencePoints { get; set; }

        /// <summary>
        /// Gets whether the Being is alive (has greater than zero hit points).
        /// </summary>
        public bool IsAlive => HitPoints > 0;

        /// <summary>
        /// Gets or sets whether the Being can be attacked.
        /// </summary>
        public bool IsTargetable { get; set; }

        /// <summary>
        /// Gets or sets the active item (weapon, spell, etc.) for the Being.
        /// </summary>
        public IGameItem ActiveItem { get; set; } =
            new Weapon
            {
                Name = "Fist",
                Description = "Default weapon.",
                DamageDie = "1d2",
                NumberOfAttacks = 1
            };

        /// <summary>
        /// Gets or sets the minimum attack roll needed to hit the GamePiece.
        /// </summary>
        public int Defense { get; set; } //TODO: Defense to take into account armor and all modifiers such as magic items or enchantments/spells.

        /// <summary>
        /// Gets the list of targets from which this Being can select one or more targets before calling <see cref="PerformActionOnSelectedTargets"/>.
        /// </summary>
        /// <remarks>You can't populate this list directly. Use <see cref="AddPotentialTargets(List{Being})"/> instead.</remarks>
        public ReadOnlyCollection<Being> PotentialTargets => _potentialTargets.AsReadOnly();

        private List<Being> _potentialTargets = new List<Being>();

        /// <summary>
        /// Gets the list of targets that the Being has selected for its next <see cref="GameAction"/>.
        /// </summary>
        /// <remarks>You can't populate this list directly. Use <see cref="AddSelectedTarget"/>, then call <see cref="PerformActionOnSelectedTargets"/>
        /// to perform <see cref="GameAction"/>s on the targets in the collection with this Being's <see cref="ActiveItem"/>.
        /// </remarks>
        public ReadOnlyCollection<Being> SelectedTargets
        {
            get { return _selectedTargets.AsReadOnly(); }
        }
        private List<Being> _selectedTargets = new List<Being>();

        #endregion Public properties

        #region Public methods
        /// <summary>
        /// Returns the value of an attack roll by the Being.
        /// </summary>
        /// <returns>Value to be compared to a Being's defense value.</returns>
        public int GetAttackRoll()
        {
            //TODO: GetAttackRoll to take into account all modifiers, including ability modifiers and enchantments on the Being's active IGameItem.
            return _attackRoll.RollDice();
        }

        /// <summary>
        /// Returns the value of a damage roll by the Being.
        /// </summary>
        /// <returns>Value to be deducted from a Being's hit points.</returns>
        public int GetDamageRoll()
        {
            //TODO: GetDamageRoll to take into account type of weapon/spell and all modifiers, including ability modifiers and weapon enchantments.
            return this.ActiveItem.GetDamageRoll();
        }

        /// <summary>
        /// Deducts the specified amount of HitPoints from the Being.
        /// </summary>
        /// <param name="damage">The amount of HitPoints to deduct.</param>
        /// <returns>Whether the applied damage killed the being.</returns>
        /// <remarks>If the Being is killed by this damage, the <see cref="OnBeingKilled"/> event is raised.
        /// The event is only raised if the Being was previously alive. This method will return <c>true</c>
        /// only if the Being was alive prior to taking this damage.</remarks>
        public bool ApplyDamage(int damage)
        {
            bool wasAlive = this.IsAlive;
            bool wasKilled = false;

            this.HitPoints -= damage;

            // Only raise the killed event if the being was alive prior to taking this damage
            if (wasAlive && !this.IsAlive)
            {
                OnKilled();

                wasKilled = true;
            }

            return wasKilled;
        }

        /// <summary>
        /// Adds the specified <see cref="Being"/> to the list of targets that will be
        /// the destination of the <see cref="GameAction"/> executed when <see cref="PerformActionOnSelectedTargets"/>
        /// is called.
        /// </summary>
        /// <param name="target">The <see cref="Being"/> that will be a destination of the
        /// <see cref="GameAction"/> performed when <see cref="PerformActionOnSelectedTargets"/> is called.</param>
        public void AddSelectedTarget(Being target)
        {
            // TODO: Need to do some sort of checking in here to
            // TODO: ensure that only the allowed number of targets
            // TODO: are selected for the active weapon/spell.
            if (!_selectedTargets.Contains(target))
            {
                // TODO: For now, just add it and raise the events.
                _selectedTargets.Add(target);

                // Raise this Being's TargetSelected event as well as
                // the target Being's SelectedAsTarget event
                OnTargetSelected(target);
                target.OnSelectedAsTarget(this);
            }
        }

        /// <summary>
        /// For each <see cref="Being"/> in <see cref="SelectedTargets"/>, creates and performs a <see cref="GameAction"/>.
        /// </summary>
        /// <remarks>Call this after the <see cref="SelectedTargets"/> collection has been populated with <see cref="AddSelectedTarget">.</remarks>
        public void PerformActionOnSelectedTargets()
        {
            foreach (Being target in this.SelectedTargets)
            {
                GameAction action = new GameAction(this, target);
                action.PerformAction();
            }
        }

        public override string ToString() => String.Format($"{this.Name} ({this.HitPoints}/{this.MaxHitPoints})");

        #endregion

        /// <summary>
        /// Adds the specified target Beings to the collection of potential targets for this Being.
        /// </summary>
        /// <param name="targets"></param>
        /// <remarks>Reference <see cref="PotentialTargets"/> to obtain a list of Beings that this Being can target with
        /// its <see cref="ActiveItem"/>.</remarks>
        internal void AddPotentialTargets(List<Being> targets)
        {
            _selectedTargets.Clear();
            _potentialTargets.Clear();
            _potentialTargets.AddRange(targets);

            // Raise PotentialTargetsAdded event. This notifies subscribers that
            // they can now select Beings from PotentialTargets and add them to
            // SelectedTargets, then call PerformActionOnSelectedTargets.
            OnPotentialTargetsAdded();
        }

        /// <summary>
        /// Raises the <see cref="TargetSelected"/> event.
        /// </summary>
        /// <param name="target">The targeted <see cref="Being"/>.</param>
        private void OnTargetSelected(Being target)
        {
            TargetSelected?.Invoke(this, new BeingTargetingEventArgs
            {
                TargetedBeing = target,
                TargetingBeing = this
            });
        }

        /// <summary>
        /// Raises the <see cref="SelectedAsTarget"/> event.
        /// </summary>
        /// <param name="target">The Being that selected this Being as a target.</param>
        internal void OnSelectedAsTarget(Being targeter)
        {
            SelectedAsTarget?.Invoke(this, new BeingTargetingEventArgs
            {
                TargetedBeing = this,
                TargetingBeing = targeter
            });
        }

        /// <summary>
        /// Raises the <see cref="PotentialTargetsAdded"/> event.
        /// </summary>
        private void OnPotentialTargetsAdded() => PotentialTargetsAdded?.Invoke(this, new EventArgs());

        /// <summary>
        /// Raises the <see cref="BeingKilled"/> event, signifying that the Being was just killed.
        /// </summary>
        private void OnKilled() => Killed?.Invoke(this, new EventArgs());
    }
}