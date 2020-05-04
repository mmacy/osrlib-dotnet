using tbrpg.Dice;

namespace tbrpg.CoreRules
{
    public class Weapon : IGameItem
    {
        /// <summary>
        /// The name of the weapon.
        /// </summary>
        public string Name { get; set; } = "Generic Weapon";

        /// <summary>
        /// The weapon's description.
        /// </summary>
        public string Description { get; set; } = "This is a generic weapon.";

        /// <summary>
        /// The die code for the weapon. Default: <c>1d6</c>.
        /// </summary>
        public string DamageDie { get; set; } = "1d6";

        /// <summary>
        /// The number of targets <see cref="IGamePiece"/>s this weapon can attack in one round. Default: <c>1</c>.
        /// </summary>
        public int NumberOfAttacks { get; set; } = 1;

        /// <summary>
        /// Rolls the weapon's <see cref="DamageDie"/> and returns the result.
        /// </summary>
        /// <returns>The amount of damage rolled.</returns>
        public int GetDamageRoll()
        {
            if (_damageRoll == null)
            {
                _damageRoll = new DiceRoll(this.DamageDie);
            }

            return _damageRoll.RollDice();
        }

        private DiceRoll _damageRoll;

        public override string ToString() => this.Name;
    }
}
