/*************************************************************************
 * File:        DiceRoll.cs
 * Author(s):   Marshall Macy II (marshallmacy@gmail.com)
 * Copyright (c) 2011 Marshall Macy II
 *************************************************************************/

using System;

namespace tbrpg.Dice
{
    /// <summary>
    /// The DiceRoll owns a set of <see cref="Dice"/> that can be rolled for result.
    /// </summary>
    /// <remarks>
    /// Add dice to the DiceRoll as a <see cref="DiceHand"/> or with a die code ("1d20"). Get a
    /// result by calling  <see cref="RollDice"/>.
    /// </remarks>
    /// <example>
    /// DiceHand hand = new DiceHand(1, 20);
    /// DiceRoll roll = new DiceRoll(hand);
    /// int result = roll.RollDice();
    ///
    /// DiceRoll roll = new DiceRoll("1d20");
    /// int result = roll.RollDice();
    /// </example>
    /// </example>
    public class DiceRoll
    {
        #region Fields
        /// <summary>
        /// The Dice collection managed by this DiceRoll.
        /// </summary>
        private Dice _dice = new Dice();

        /// <summary>
        /// Holds any Modifiers to the DiceRoll. The value returned by the RollDice() method factors in the Modifiers.
        /// </summary>
        private int _modifiers = 0;
        private int _baseRoll  = 0;
        private int _lastRoll  = 0;

        /// <summary>
        /// Event raised immediately after <see cref="DiceRoll.RollDice"/> is called.
        /// </summary>
        public event DiceRolledEventHandler DiceRolled;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DiceRoll, adding the specified dice to the DiceRoll's Dice collection.
        /// This is the preferred method of constructing a DiceRoll.
        /// </summary>
        /// <param name="diceHand">The DiceHand specifying the Dice to add to the DiceRoll.</param>
        public DiceRoll(DiceHand diceHand)
        {
            if (diceHand != null)
                AddDice(diceHand);
            else
                throw new ArgumentNullException("diceHand", "You must specify a valid DiceHand object.");
        }

        /// <summary>
        /// Creates a new instance of a DiceRoll, adding the Dice represented by the specified die code (e.g. "1d4", "2d6") to its Dice collection
        /// </summary>
        /// <param name="dieCode">The die code string representing the number of dice and the number of sides of each die in the DiceRoll, e.g. "1d4", "2d6".</param>
        public DiceRoll(string dieCode)
        {
            int dieCount = 0;
            int dieSides = 0;

            if (String.IsNullOrEmpty(dieCode))
                throw new ArgumentNullException("dieCode", "You must specify a valid die code string, e.g. \"1d4\", \"2d6\".");
            else if (!Dice.TryParseDieCode(dieCode, out dieCount, out dieSides))
                throw new ArgumentException("dieCode", "You must specify a valid die code string, e.g. \"1d4\", \"2d6\".");

            AddDice(dieCode);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the Dice represented by the specified die code (e.g. "1d4", "2d6") to the Dice collection.
        /// </summary>
        /// <param name="dieCode">The die code string representing the number of dice and the number of sides of each die in the DiceRoll, e.g. "1d4", "2d6".</param>
        public void AddDice(string dieCode)
        {
            _dice.AddDice(dieCode);
        }

        /// <summary>
        /// Adds the specified Die to this DiceRoll's Dice collection.
        /// </summary>
        /// <param name="die">The Dice.Die to add to the DiceRoll's Dice collection.</param>
        public void AddDie(Die die)
        {
            _dice.AddDie(die);
        }

        /// <summary>
        /// Adds the Die contained in the specified Dice collection to this DiceRoll's Dice collection.
        /// </summary>
        /// <param name="dice">The Dice.Dice containing the Die objects to add to the DiceRoll's Dice collection.</param>
        public void AddDice(Dice dice)
        {
            _dice.AddDice(dice);
        }

        /// <summary>
        /// Adds the Dice represented by the specified DiceHand to the DiceRoll's Dice collection.
        /// </summary>
        /// <param name="diceHand">The DiceHand specifying the number and type (number of sides) of Die that should be added to the DiceRoll's Dice collection.</param>
        public void AddDice(DiceHand diceHand)
        {
            _dice.AddDice(diceHand);
        }

        /// <summary>
        /// Adds the specified modifier to the DiceRoll. The value can be negative.
        /// </summary>
        /// <param name="modifier">A value that will modify the DiceRoll. </param>
        public void AddModifier(int modifier)
        {
            _modifiers += modifier;
        }

        /// <summary>
        /// Removes all Dice from the DiceRoll.
        /// </summary>
        public void ClearDice()
        {
            _dice.Clear();
        }

        /// <summary>
        /// Removes all Modifiers from the DiceRoll.
        /// </summary>
        public void ClearModifiers()
        {
            _modifiers = 0;
        }

        /// <summary>
        /// Removes the specified Die from this DiceRoll's Dice collection,if it exists in the collection.
        /// </summary>
        /// <param name="die">The Dice.Die to remove from the DiceRoll's Dice collection.</param>
        public void RemoveDie(Die die)
        {
            if (_dice.Contains(die))
                _dice.Remove(die);
        }

        /// <summary>
        /// Rolls each <see cref="Die"/> in this DiceRolls's <see cref="Dice"/> collection and returns the aggregate.
        /// </summary>
        /// <returns>The sum of the result of a roll of each Die in this DiceRoll's Dice collection.</returns>
        public int RollDice()
        {
            int rollResult = 0;

            //Tabulate the total of each Die's roll
            foreach (Die die in _dice)
            {
                rollResult += die.Roll();
            }

            _baseRoll = rollResult;

            //Add any Modifiers
            rollResult += _modifiers;

            _lastRoll = rollResult;

            //Notify any subscribers
            OnDicedRolled();

            return rollResult;
        }

        /// <summary>
        /// Raises the <see cref="DicedRolled"/> event.
        /// </summary>
        private void OnDicedRolled()
        {
            DiceRolled?.Invoke(this, new DiceRolledEventArgs(this));
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the last base roll result without any Modifiers.
        /// </summary>
        public int BaseRoll
        {
            get { return _baseRoll; }
        }

        /// <summary>
        /// Gets the Dice collection for the DiceRoll.
        /// </summary>
        public Dice Dice
        {
            get { return _dice; }
        }

        /// <summary>
        /// Gets the value of this DiceRoll since last time RollDice() was called.
        /// </summary>
        public int LastRoll
        {
            get { return _lastRoll; }
        }
        #endregion

        /// <summary>
        /// Returns the string representation of the roll such as "1d20 + 2".
        /// </summary>
        /// <returns>String representation of the roll.</returns>
        public override string ToString()
        {
            string roll = String.Empty;

            if (_dice.Count > 0)
            {
                roll = _dice.Count.ToString() + "d" + _dice[0].Sides.ToString();

                if (_modifiers > 0)
                    roll += " + " + _modifiers.ToString();
                else if (_modifiers < 0)
                    roll += " - " + _modifiers.ToString();
            }

            return roll;
        }
    }
}