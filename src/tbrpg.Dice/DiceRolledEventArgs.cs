/*************************************************************************
 * File:        DiceRolledEventArgs.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2016 Marshall Macy II
 *************************************************************************/

using System;

namespace tbrpg.Dice
{
    /// <summary>
    /// Represents the method that handles a <see cref="DiceRoll.DiceRolled"/> event.
    /// </summary>
    /// <param name="sender">The object that generated the event.</param>
    /// <param name="e">The information for the event.</param>
    public delegate void DiceRolledEventHandler(Object sender, DiceRolledEventArgs e);

    public class DiceRolledEventArgs : EventArgs
    {
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
