using System;

namespace osrlib.Dice
{
    /// <summary>
    /// Represents the method that handles a <see cref="DiceRoll.DiceRolled"/> event.
    /// </summary>
    /// <param name="sender">The object that generated the event.</param>
    /// <param name="e">The information for the event.</param>
    public delegate void DiceRolledEventHandler(Object sender, DiceRolledEventArgs e);

    /// <summary>
    /// Event arguments for the <see cref="DiceRoll.DiceRolled"/> event.
    /// </summary>
    public class DiceRolledEventArgs : EventArgs
    {
        /// <summary>
        /// Creates event arguments for the specified roll.
        /// </summary>
        /// <param name="roll"></param>
        public DiceRolledEventArgs(DiceRoll roll)
        {
            this.RolledDice = roll;
        }

        /// <summary>
        /// Gets or sets the <see cref="DiceRoll"/> for the event.
        /// </summary>
        public DiceRoll RolledDice { get; internal set; }
    }
}
