﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tbrpg.CoreRules
{

    /// <summary>
    /// Specification defining any object that that is not a <see cref="IGameItem"/>.
    /// </summary>
    /// <remarks>A class implementing IGamePiece indicates that the object can perform a <see cref="GameAction"/> on another IGamePiece.</remarks>
    public interface IGamePiece
    {
        /// <summary>
        /// The friendly name of the GamePiece.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the GamePiece can be the target of an attack.
        /// </summary>
        bool IsTargetable { get; set; }

        /// <summary>
        /// Gets or sets the current hit point value of the GamePiece.
        /// </summary>
        int HitPoints { get; set; }

        /// <summary>
        /// Gets or sets the maximum hit point value of the GamePiece.
        /// </summary>
        int MaxHitPoints { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the minimum attack roll needed to hit the GamePiece.
        /// </summary>
        int Defense { get; set; }

        /// <summary>
        /// Returns the value of an attack roll by the GamePiece.
        /// </summary>
        /// <returns>Value to be compared to a GamePiece's defense value.</returns>
        int GetAttackRoll();

        /// <summary>
        /// Returns the value of a damage roll by the GamePiece.
        /// </summary>
        /// <returns>Value to be deducted from a GamePiece's hit point value.</returns>
        int GetDamageRoll();

        /// <summary>
        /// Applies the specified amount of damage to this GamePiece.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        /// <returns>Value indicating whether the GamePiece was killed.</returns>
        bool ApplyDamage(int damage);
    }
}
