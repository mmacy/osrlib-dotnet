using System;
using osrlib.Dice;

namespace osrlib.CoreRules
{
    /// <summary>
    /// The GameAction represents an act initiated by one entity on another, such as a
    /// melee attack from one Being on another, or a lockpick attempt from a Being on
    /// a locked treasure chest.
    /// </summary>
    public class GameAction
    {
        /// <summary>
        /// Gets the attack roll result of the GameAction.
        /// </summary>
        /// <value>The attack roll associated with the GameAction's attack operation.</value>
        public DiceRoll AttackRoll { get; private set; }

        /// <summary>
        /// Gets the damage roll result of the GameAction.
        /// </summary>
        /// <value>The damage roll associated with the GameAction's damage operation.</value>
        public DiceRoll DamageRoll { get; private set; }

        /// <summary>
        /// Creates a new instance of the GameAction with the specified attacker and defender.
        /// </summary>
        /// <param name="source">The <see cref="Being"/> performing the action.</param>
        /// <param name="target">The <see cref="Being"/> that is the target of the action.</param>
        public GameAction(Being source, Being target)
        {
            this.ActionSource = source;
            this.ActionTarget = target;
        }

        /// <summary>
        /// Gets or sets the <see cref="Being"/> performing the action.
        /// </summary>
        public Being ActionSource { get; set; }

        /// <summary>
        /// Gets or sets the target <see cref="Being"/>.
        /// </summary>
        public Being ActionTarget { get; set; }

        /// <summary>
        /// Gets the victor of the GameAction.
        /// </summary>
        public Being Victor { get; private set; }

        /// <summary>
        /// Performs the <see cref="GameAction"/> by applying the attacker's attack roll to the defender's defense
        /// value, and applying damage to the defender if the attack roll succeeded.
        /// </summary>
        /// <returns>The victor in the GameAction.</returns>
        /// <remarks>The victor in the GameAction can be the defending GamePiece.</remarks>
        public Being PerformAction()
        {
            // If the defender is attackable, compare an attack roll by the attacker with the
            // defender's defense value, apply damage if successful, and return the victor.

            if (this.ActionTarget.IsTargetable)
            {
                this.AttackRoll = this.ActionSource.GetAttackRoll();
                this.Victor = this.AttackRoll.LastRoll >= this.ActionTarget.Defense ? this.ActionSource : this.ActionTarget;

                if (this.Victor == this.ActionSource)
                {   this.DamageRoll = this.ActionSource.GetDamageRoll();
                    this.ActionTarget.ApplyDamage(this.DamageRoll.LastRoll);
                }
            }
            else
            {
                //TODO: Just return the defender as victor if Defender.IsAttackable == false?

                throw new InvalidOperationException("The target is invalid (IsTargetable is false).");
            }

            return this.Victor;
        }
    }
}
