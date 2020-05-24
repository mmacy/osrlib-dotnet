using System;

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
        /// Event raised when the GameAction is about to be performed.
        /// </summary>
        public event EventHandler PerformingAction;

        /// <summary>
        /// Event raised when the GameAction has just been performed.
        /// </summary>
        public event EventHandler ActionPerformed;

        /// <summary>
        /// Creates a new instance of the GameAction with the specified attacker and defender.
        /// </summary>
        /// <param name="source">The GamePiece performing the action.</param>
        /// <param name="target">The GamePiece that is the target of the action.</param>
        public GameAction(IGamePiece source, IGamePiece target)
        {
            this.ActionSource = source;
            this.ActionTarget = target;
        }

        /// <summary>
        /// Gets or sets the GamePiece performing the action.
        /// </summary>
        public IGamePiece ActionSource { get; set; }

        /// <summary>
        /// Gets or sets the target Gamepiece.
        /// </summary>
        public IGamePiece ActionTarget { get; set; }

        /// <summary>
        /// Gets the victor of the GameAction.
        /// </summary>
        public IGamePiece Victor { get; private set; }

        /// <summary>
        /// Performs the <see cref="GameAction"/> by applying the attacker's attack roll to the defender's defense value.
        /// </summary>
        /// <returns>The victor in the GameAction.</returns>
        /// <remarks>The victor in the GameAction can be the defending GamePiece.</remarks>
        public IGamePiece PerformAction()
        {
            OnPerformingAction();

            // If the defender is attackable, compare an attack roll by the attacker with the
            // defender's defense value, apply damage if successful, and return the victor.

            if (this.ActionTarget.IsTargetable)
            {
                this.Victor = this.ActionSource.GetAttackRoll() >= this.ActionTarget.Defense ? this.ActionSource : this.ActionTarget;

                if (this.Victor == this.ActionSource)
                {
                    this.ActionTarget.ApplyDamage(this.ActionSource.GetDamageRoll());
                }
            }
            else
            {
                //TODO: Just return the defender as victor if Defender.IsAttackable == false?

                throw new InvalidOperationException("The target is invalid (IsTargetable is false).");
            }

            OnActionPerformed();

            return this.Victor;
        }

        /// <summary>
        /// Raises the <see cref="ActionPerformed"/> event, signifying that this action was just performed.
        /// </summary>
        private void OnActionPerformed() => ActionPerformed?.Invoke(this, new EventArgs());

        /// <summary>
        /// Raises the <see cref="PerformingAction"/> event, signifying that this action is about to be performed.
        /// </summary>
        private void OnPerformingAction() => PerformingAction?.Invoke(this, new EventArgs());
    }
}
