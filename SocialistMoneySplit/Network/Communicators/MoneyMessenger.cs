using SocialistMoneySplit.Abstractions;
using SocialistMoneySplit.Models;
using StardewValley;

namespace SocialistMoneySplit.Networking.Communicators
{
    public class MoneyMessenger : BaseMessenger
    {
        /// <summary>
        /// Sends all farmers the "SocialistMoneySplit.MoneyReceiver" message to tell them to update their money
        /// </summary>
        /// <param name="newMoney">The amount of money for each farmer to receive</param>
        /// <param name="eventContext">THhe event triggering the money update</param>
        public void SendWalletNotification(int newMoney)
        {
            MoneyPayload moneyData = new MoneyPayload(newMoney, Game1.player.Name, EventContext.InventoryChanged);
            base.SendCoreMessageToAllFarmers(Constants.ModReceiverAddress, moneyData);

            SocialismMod.Logger.Log("MoneyMessengerUtil.SendWalletNotification | Local farmer " + Game1.player.Name + " is sending " + newMoney + " to all farmers");
        }

        /// <summary>
        /// Sends all farmers the "SocialistMoneySplit.MoneyReceiver" message to tell them to update their money
        /// </summary>
        /// <param name="newMoney">The amount of money for each farmer to receive</param>
        /// <param name="eventContext">THhe event triggering the money update</param>
        public void SendShippingBinNotification(int newMoney)
        {
            MoneyPayload moneyData = new MoneyPayload(newMoney, Game1.player.Name, EventContext.EndOfDay);
            base.SendCoreMessageToAllFarmers(Constants.ModReceiverAddress, moneyData);

            SocialismMod.Logger.Log("MoneyMessengerUtil.SendShippingBinNotification | Local farmer " + Game1.player.Name + " is sending " + newMoney + " to all farmers");
        }
    }
}
