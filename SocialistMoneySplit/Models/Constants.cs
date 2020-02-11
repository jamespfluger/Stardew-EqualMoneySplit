namespace SocialistMoneySplit.Models
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
        /// Base address mods send/receive from
        /// </summary>
        public static string ModReceiverAddress { get; private set; } = "SocialistMoneySplit.Receiver";
    }
}
