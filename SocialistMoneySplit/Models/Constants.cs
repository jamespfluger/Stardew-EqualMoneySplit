namespace SocialistMoneySplit.Models
{
    public enum EventContext
    {
        InventoryChanged,
        EndOfDay
    }

    public static class Constants
    {
        public static string WalletAddress { get; private set; }  = ModReceiverAddress + "WalletReceiver";
        public static string ShippingBinAddress { get; private set; } = ModReceiverAddress + "ShippingBinReceiver";

        public static string ModReceiverAddress { get; private set; } = "SocialistMoneySplit.Receiver";
    }
}
