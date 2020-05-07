namespace tbrpg.CoreRules
{
    /// <summary>
    /// Provides information for events in which one Being is targeting another.
    /// Such events are typically fired prior to performing a <see cref="GameAction"/>.
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
