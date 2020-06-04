using System;

namespace osrlib.Dice
{
    /// <summary>
    /// The DiceRoll owns a set of <see cref="Dice"/> that can be rolled for result.
    /// </summary>
    /// <remarks>
    /// Add dice to the DiceRoll as a <see cref="DiceHand"/>, then get the roll result by calling  <see cref="RollDice()"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Roll one twenty-sided die.
    /// DiceHand hand = new DiceHand(1, DieType.d20);
    /// DiceRoll roll = new DiceRoll(hand);
    /// int result = roll.RollDice();
    /// </code>
    /// </example>
    public class DiceRoll
    {
        #region Fields
        /// <summary>
        /// The Dice collection managed by this DiceRoll.
        /// </summary>
        private Dice _dice = new Dice();

        /// <summary>
        /// Event raised immediately after <see cref="DiceRoll.RollDice()"/> is called.
        /// </summary>
        public event DiceRolledEventHandler DiceRolled;
        #endregion

        /// <summary>
        /// Creates a new instance of a DiceRoll, adding the specified dice to the DiceRoll's Dice collection.
        /// </summary>
        /// <param name="diceHand">The DiceHand specifying the Dice to add to the DiceRoll.</param>
        public DiceRoll(DiceHand diceHand)
        {
            if (diceHand != null)
                AddDice(diceHand);
            else
                throw new ArgumentNullException("diceHand", "You must specify a valid DiceHand object.");
        }

        #region Public Methods
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
        /// <param name="modifier">A value that will modify the DiceRoll.</param>
        public void AddModifier(int modifier)
        {
            this.ModifierTotal += modifier;
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
            this.ModifierTotal = 0;
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

            this.BaseRoll = rollResult;

            //Add any Modifiers
            rollResult += this.ModifierTotal;

            this.LastRoll = rollResult;

            //Notify any subscribers
            OnDicedRolled();

            return rollResult;
        }

        /// <summary>
        /// Rolls the dice in the specified <see cref="DiceHand"/> and returns the roll - the sum of each rolled die and the modifier, if specified.
        /// </summary>
        /// <param name="diceHand">The handful of dice to roll.</param>
        /// <param name="modifier">The modifer value to apply to the roll.</param>
        /// <returns>The resultant DiceRoll (a DiceRoll with its <see cref="DiceRoll.RollDice()"/> method having been called).</returns>
        public static int RollDice(DiceHand diceHand, int modifier = 0)
        {
            DiceRoll roll = new DiceRoll(diceHand);
            roll.AddModifier(modifier);

            return roll.RollDice();
        }

        /// <summary>
        /// Raises the <see cref="DiceRoll.DiceRolled"/> event.
        /// </summary>
        private void OnDicedRolled()
        {
            DiceRolled?.Invoke(this, new DiceRolledEventArgs(this));
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the sum of all modifiers applied to the roll with <see cref="AddModifier"/>.
        /// The <see cref="RollDice()"/> method adds this value to the <see cref="BaseRoll"/> to obtain its result.
        /// </summary>
        public int ModifierTotal { get; private set; } = 0;

        /// <summary>
        /// Gets the last roll result without any <see cref="ModifierTotal"/>.
        /// </summary>
        public int BaseRoll { get; private set; } = 0;

        /// <summary>
        /// Gets the result of the last time <see cref="RollDice()"/> was called.
        /// This value includes all modifiers applied to the roll.
        /// </summary>
        public int LastRoll { get; private set; } = 0;

        /// <summary>
        /// Gets the Dice collection for the DiceRoll.
        /// </summary>
        public Dice Dice
        {
            get { return _dice; }
        }
        #endregion

        /// <summary>
        /// Returns the string representation of the latest roll of this DiceRoll in the format 'N (NdN +/- N)'. For example: "16 (1d20 + 2)".
        /// </summary>
        /// <returns>String representation of the roll.</returns>
        public override string ToString()
        {
            string roll = String.Empty;

            if (_dice.Count > 0)
            {
                roll = $"{this.LastRoll} ({_dice.Count.ToString()}d{_dice[0].Sides.ToString()}";

                if (this.ModifierTotal > 0)
                    roll += "+" + this.ModifierTotal.ToString() + ")";
                else if (this.ModifierTotal < 0)
                    roll += this.ModifierTotal.ToString() + ")";
                else
                    roll += ")";
            }

            return roll;
        }
    }
}