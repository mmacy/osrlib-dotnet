﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tbrpg.Dice;

namespace tbrpg.CoreRules
{
    /// <summary>
    /// Represents the method that handles a Being's <see cref="tbrpg.CoreRules.Being.TargetSelected"/> or
    /// <see cref="tbrpg.CoreRules.Being.SelectedAsTarget"/> event.
    /// </summary>
    /// <param name="sender">The object generating the event.</param>
    /// <param name="e">The information for the event.</param>
    public delegate void BeingTargetingEventHandler(object sender, BeingTargetingEventArgs e);

    /// <summary>
    /// The Being represents a living entity in within an <see cref="Adventure"/>, and is used for both player characters and monsters.
    /// </summary>
    public class Being : IGamePiece
    {
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
        public List<Ability> Abilities { get; set; } = new List<Ability>();

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
        /// Gets or sets the Being's active weapon or spell. The active weapon is a weapon or offensive spell, and is
        /// used when when the Being attacks another being.
        /// </summary>
        public Weapon ActiveWeapon { get; set; } =
            new Weapon
            {
                Name = "Fists",
                Description = "Default weapon.",
                DamageDie = new DiceHand(1, DieType.d2)
            };

        /// <summary>
        /// Gets or sets the minimum attack roll needed to hit the GamePiece.
        /// </summary>
        public int Defense { get; set; } //TODO: Defense to take into account armor and all modifiers like dexterity mod and magic items or enchantments/spells.

        /// <summary>
        /// Gets the list of targets from which this Being can select one or more targets before calling <see cref="PerformActionOnSelectedTargets"/>.
        /// </summary>
        /// <remarks>You can't populate this list directly. Use <see cref="AddPotentialTargets(List{Being})"/> instead.</remarks>
        public ReadOnlyCollection<Being> PotentialTargets => _potentialTargets.AsReadOnly();

        private List<Being> _potentialTargets = new List<Being>();

        /// <summary>
        /// Gets the list of targets that the Being has selected for its next <see cref="GameAction"/>.
        /// </summary>
        /// <remarks>
        /// You can't populate this list directly. Use <see cref="AddSelectedTarget"/>, then call <see cref="PerformActionOnSelectedTargets"/>
        /// to perform <see cref="GameAction"/>s on the targets in the collection with this Being's <see cref="ActiveWeapon"/>.
        /// </remarks>
        public ReadOnlyCollection<Being> SelectedTargets
        {
            get { return _selectedTargets.AsReadOnly(); }
        }
        private List<Being> _selectedTargets = new List<Being>();

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Adds a modifier to the specified ability's <see cref="Ability.ScoreModifiers"/> collection.
        /// This acts as a bonus or penalty, effectively an enchantment or a curse.
        /// </summary>
        /// <param name="modifier">The modifier to adjust the specified ability score.</param>
        /// <param name="abilityType">The ability to which to apply the modifier.</param>
        public void AddAbilityModifier(Modifier modifier, AbilityType abilityType)
        {
            Ability abilityToMod = GetAbilityByType(abilityType);

            if (abilityToMod != null)
            {
                abilityToMod.ScoreModifiers.Add(modifier);
            }
            else
            {
                throw new InvalidOperationException($"No ability of type {abilityType.ToString()} exists in the Being's Abilities collection. Have you called RollAbilities()?");
            }
        }

        /// <summary>
        /// Returns the value of an attack roll by the Being. The Being's active weapon (or spell) is used in calculating
        /// the roll, as are any ability modifiers appropriate for the weapon type.
        /// </summary>
        /// <returns>Value to be compared to a Being's defense value.</returns>
        public int GetAttackRoll()
        {
            int modifierValue = GetAbilityModifierValueForWeaponType(this.ActiveWeapon.Type);

            return this.ActiveWeapon.GetAttackRoll(modifierValue);
        }

        /// <summary>
        /// Returns the value of a damage roll by the Being. The Being's active weapon (or spell) is used in calculating
        /// the roll, as are any ability modifiers appropriate for the weapon type.
        /// </summary>
        /// <returns>Value to be deducted from an opponent Being's hit points.</returns>
        public int GetDamageRoll()
        {
            int modifierValue = GetAbilityModifierValueForWeaponType(this.ActiveWeapon.Type);

            return this.ActiveWeapon.GetDamageRoll(modifierValue);
        }

        /// <summary>
        /// Deducts the specified amount of HitPoints from the Being.
        /// </summary>
        /// <param name="damage">The amount of HitPoints to deduct.</param>
        /// <returns>Whether the applied damage killed the being.</returns>
        /// <remarks>If the Being is killed by this damage, the <see cref="OnKilled"/> event is raised.
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
        /// <remarks>
        /// Call this after the <see cref="SelectedTargets"/> collection has been populated with <see cref="AddSelectedTarget"/>.
        /// </remarks>
        public void PerformActionOnSelectedTargets()
        {
            foreach (Being target in this.SelectedTargets)
            {
                GameAction action = new GameAction(this, target);
                action.PerformAction();
            }
        }

        /// <summary>
        /// Rolls the specified ability score and adds the ability to the Being's ability collection. If an ability of
        /// the same type is already in the <see cref="Abilities"/> collection, the existing ability is removed before
        /// adding the new one generated by this method.
        /// </summary>
        /// <param name="abilityType">The type of ability to roll and add to the <see cref="Abilities"/> collection.</param>
        /// <returns>The newly rolled Ability.</returns>
        public Ability RollAbilityScore(AbilityType abilityType)
        {
            // Since we're rolling the ability score, clear it out if it already exists.
            // This means that any modifiers on the current ability are also cleared.
            Ability ability = GetAbilityByType(abilityType);
            if (ability != null)
            {
                this.Abilities.Remove(ability);
            }

            // Create the new ability
            ability = new Ability { Type = abilityType };
            ability.RollAbilityScore();
            this.Abilities.Add(ability);

            return ability;
        }

        /// <summary>
        /// Rolls the full set of ability scores for the Being. Calling this method removes any abilities currently in
        /// the Being's <see cref="Abilities"/> collection.
        /// </summary>
        /// <returns>The Being's newly populated <see cref="Abilities"/> collection.</returns>
        public List<Ability> RollAbilities()
        {
            foreach (AbilityType type in Enum.GetValues(typeof(AbilityType)))
            {
                RollAbilityScore(type);
            }

            return this.Abilities;
        }

        /// <summary>
        /// Gets the string representation of the Being.
        /// </summary>
        /// <returns>Single-line text representation of the Being.</returns>
        public override string ToString() => String.Format($"{this.Name} ({this.HitPoints}/{this.MaxHitPoints})");

        #endregion

        /// <summary>
        /// Adds the specified target Beings to the collection of potential targets for this Being.
        /// </summary>
        /// <param name="targets"></param>
        /// <remarks>Reference <see cref="PotentialTargets"/> to obtain a list of Beings that this Being can target with
        /// its <see cref="ActiveWeapon"/>.</remarks>
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
        /// For the given weapon type, returns the associated modifier granted by the appropriate ability. For example,
        /// the strength modifier for a melee weapon or the dexterity modifier for a ranged weapon.
        /// </summary>
        /// <param name="weaponType">The type of weapon for which to obtain the associated ability modifier value.</param>
        /// <returns>The modifier granted or imposed by the ability for the weapon type.</returns>
        private int GetAbilityModifierValueForWeaponType(WeaponType weaponType)
        {
            int modValue = weaponType switch
            {
                WeaponType.Melee  => GetAbilityByType(AbilityType.Strength).GetModifier(),
                WeaponType.Ranged => GetAbilityByType(AbilityType.Dexterity).GetModifier(),
                WeaponType.Spell  => GetAbilityByType(AbilityType.Intelligence).GetModifier(),
                _                 => 0
            };

            return modValue;
        }

        /// <summary>
        /// Gets the ability of the specified type from the Being's abilities collection.
        /// </summary>
        /// <param name="abilityType">The type of the ability to obtain.</param>
        /// <returns>The ability of the specified type from the Being's <see cref="Abilities"/> collection, or <c>null</c>
        /// if no such ability is within the collection.
        /// </returns>
        private Ability GetAbilityByType(AbilityType abilityType)
        {
            if (this.Abilities.Any())
            {
                return this.Abilities.Find(a => a.Type == abilityType);
            }
            else
            {
                return null;
            }
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
        /// <param name="targeter">The Being that selected this Being as a target.</param>
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
        /// Raises the <see cref="Killed"/> event, signifying that the Being was just killed.
        /// </summary>
        private void OnKilled() => Killed?.Invoke(this, new EventArgs());
    }
}