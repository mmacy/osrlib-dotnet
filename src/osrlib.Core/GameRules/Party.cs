using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace osrlib.Core
{
    /// <summary>
    /// Represents a collection of Beings. Two Parties are pit against each other in combat.
    /// </summary>
    public class Party
    {
        private List<Being> _partyMembers = new List<Being>();

        /// <summary>
        /// Event raised when the last living member of the party is <see cref="Being.Killed"/>.
        /// </summary>
        public event EventHandler Defeated;

        #region Public Methods
        /// <summary>
        /// Adds the specified Being to this Party.
        /// </summary>
        /// <param name="being">The Being to add to this Party.</param>
        public void AddPartyMember(Being being)
        {
            if (!_partyMembers.Contains(being))
            {
                being.Killed += Being_Killed;
                _partyMembers.Add(being);
            }
            else
                throw new InvalidOperationException("That Being is already a member of this Party");
        }

        /// <summary>
        /// Adds the specified <see cref="Being"/>s to the Party.
        /// </summary>
        /// <param name="newMembers">The collection of <see cref="Being"/>s to add.</param>
        public void AddPartyMembers(IList<Being> newMembers)
        {
            foreach (Being newMember in newMembers)
            {
                AddPartyMember(newMember);
            }
        }

        /// <summary>
        /// Removes the specified Being from this Party.
        /// </summary>
        /// <param name="being">The Being to remove from this Party.</param>
        public void RemovePartyMember(Being being)
        {
            if (_partyMembers.Contains(being))
            {
                being.Killed -= Being_Killed;
                _partyMembers.Remove(being);
            }
            else
                throw new InvalidOperationException("That Being is not a member of this party.");
        }

        /// <summary>
        /// Removes all Beings from this Party.
        /// </summary>
        public void ClearParty()
        {
            _partyMembers.Clear();
        }

        /// <summary>
        /// Gets a string showing each party member and their health status.
        /// </summary>
        /// <param name="beings">A collection of party members.</param>
        /// <returns>String showing the list of party members and their remaining hit points (or "DEAD").</returns>
        private static string GetPartyString(List<Being> beings)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Being member in beings)
            {
                string status = member.IsAlive ? "Hit points: " + member.HitPoints.ToString() : "DEAD";
                string line = $"[{beings.IndexOf(member)}] {member.Name}\t{status}";

                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a string representation of the Party.
        /// </summary>
        /// <returns>A multiline list of the party members and their health status.</returns>
        public override string ToString() => GetPartyString(_partyMembers);

        #endregion

        /// <summary>
        /// Raises the <see cref="Party.Defeated"/> event if the killed <see cref="Being"/> was
        /// the last living Being in the Party.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Information about the event.</param>
        private void Being_Killed(object sender, EventArgs e)
        {
            if (!this.IsAlive)
            {
                OnDefeated();
            }
        }

        /// <summary>
        /// Raises the <see cref="Defeated"/> event, notifying subscribers that the last living
        /// <see cref="Being"/> in this party was <see cref="Being.Killed"/>.
        /// </summary>
        private void OnDefeated() => Defeated?.Invoke(this, new EventArgs());

        #region Public Properties

        /// <summary>
        /// Gets or sets the owner of this party.
        /// </summary>
        public User UserId { get; set; }

        /// <summary>
        /// Gets whether at least one member of this Party is alive. If there are no
        /// Beings alive in a Party, the Party is considered dead.
        /// </summary>
        public bool IsAlive => this.LivingMembers.Any();

        /// <summary>
        /// Gets a collection of all Beings in this party, living or otherwise.
        /// </summary>
        /// <remarks>To add or remove party members, use <see cref="AddPartyMember(Being)"/> and <see cref="RemovePartyMember(Being)"/>.</remarks>
        public List<Being> Members => _partyMembers;

        /// <summary>
        /// Gets a collection of Beings in this party that are alive.
        /// </summary>
        /// <remarks>To add or remove party members, use <see cref="AddPartyMember(Being)"/> and <see cref="RemovePartyMember(Being)"/>.</remarks>
        public ReadOnlyCollection<Being> LivingMembers
        {
            get
            {
                return (from member in _partyMembers
                        where member.IsAlive
                        select member).ToList().AsReadOnly();
            }
        }

        #endregion
    }
}
