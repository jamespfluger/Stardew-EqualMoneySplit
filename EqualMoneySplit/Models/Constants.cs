namespace EqualMoneySplit.Models
{
    /// <summary>
    /// Type of event triggering an action
    /// </summary>
    public enum EventContext
    {
        InventoryChanged,
        EndOfDay
    }

    /// <summary>
    /// Constant values used throughout mod
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Base address mods send/listen to
        /// </summary>
        public static string ModListenerAddress { get; private set; } = "EqualMoneySplit.Listener";
    }
}
