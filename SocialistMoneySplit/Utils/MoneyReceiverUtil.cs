using SocialistMoneySplit.Models;
using SocialistMoneySplit.Network;
using StardewValley;

namespace SocialistMoneySplit.Utils
{
    /// <summary>
    /// A utility used to send/receive money from other Farmers
    /// </summary>
    public static class MoneyReceiverUtil
    {
        /// <summary>
        /// Which event the money is being received at
        /// </summary>
        public enum EventContext
        {
            EndOfDay,
            InventoryChange
        }

        public static Receiver<NetworkPayload> moneyReceiver;
        public const string moneyReceiverName = "SocialistMoneySplit.MoneyReceiver";

        /// <summary>
        /// Initializes the Money Receiver
        /// </summary>
        public static void Init()
        {
            // Add the event handler for our specific mod
            SocialismMod.Helper.Events.Multiplayer.ModMessageReceived += NetworkUtil<NetworkPayload>.OnModMessageReceived;

            // Then initialize our specific receiver and start it up
            moneyReceiver = InitializeMoneyReceiver();
            moneyReceiver.Start();

            SocialismMod.Monitor.Log("MoneyMessengerUtil.Init() | Initialized the money receiver");
        }

        /// <summary>
        /// Sends all farmers the "SocialistMoneySplit.MoneyReceiver" message to tell them to update their money
        /// </summary>
        /// <param name="newMoney">The amount of money for each farmer to receive</param>
        /// <param name="eventContext">THhe event triggering the money update</param>
        public static void SendMoneyUpdateNotification(int newMoney, EventContext eventContext)
        {
            NetworkPayload moneyData = new NetworkPayload(newMoney, Game1.player.Name, eventContext);

            SocialismMod.Monitor.Log("MoneyMessengerUtil.SendMoneyUpdateNotification | Local farmer " + Game1.player.Name + " is sending " + newMoney + " to all farmers");
            NetworkUtil<NetworkPayload>.SendMessageToAllFarmers(moneyReceiverName, moneyData);
        }

        /// <summary>
        /// Forces the game to retrieve any messages they have not received yet
        /// </summary>
        public static void ForceGetUnarrivedMessages()
        {
            moneyReceiver.CheckForNewMessages();
        }

        /// <summary>
        /// Initializes the receiver that will fire when the "SocialistMoneySplit.MoneyReceiver" message is sent
        /// </summary>
        private static Receiver<NetworkPayload> InitializeMoneyReceiver()
        {
            return new Receiver<NetworkPayload>(moneyReceiverName, (networkMoneyData) =>
            {
                SocialismMod.Monitor.Log("MoneyMessengerUtil.ReceivingMoney | Local farmer " + Game1.player.Name + " is receiving " + networkMoneyData.Money + " from " + networkMoneyData.Name);
                SocialismMod.Monitor.Log("MoneyMessengerUtil.ReceivingMoney | Local farmer " + Game1.player.Name + " previously had " + Game1.player.Money + " and will now have " + (Game1.player.Money + networkMoneyData.Money));

                if (networkMoneyData.EventContext == EventContext.InventoryChange)
                    Game1.showGlobalMessage(networkMoneyData.Name + " sent you " + networkMoneyData.Money + "g !");
                else if (networkMoneyData.EventContext == EventContext.EndOfDay)
                    Game1.chatBox.addInfoMessage(networkMoneyData.Name + " sent you " + networkMoneyData.Money + "g !");

                Game1.player.Money += networkMoneyData.Money;
            });
        }
    }
}
