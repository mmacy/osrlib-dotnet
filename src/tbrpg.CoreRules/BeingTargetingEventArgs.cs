using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tbrpg.CoreRules
{
    /// <summary>
    /// Provides information for events in which one Being is targeting another. This is
    /// typically done prior to performing a <see cref="GameAction"/>.
    /// </summary>
    public class BeingTargetingEventArgs
    {
        /// <summary>
        /// Gets or sets the Being doing the targeting.
        /// </summary>
        public Being TargetingBeing { get; set; }

        /// <summary>
        /// Gets or sets the Being that is being targeted.
        /// </summary>
        public Being TargetedBeing { get; set; }
    }
}
