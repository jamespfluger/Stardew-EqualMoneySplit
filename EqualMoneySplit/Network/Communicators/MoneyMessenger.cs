using EqualMoneySplit.Abstractions;
using EqualMoneySplit.Models;
using StardewValley;

namespace EqualMoneySplit.Networking.Communicators
{
    public class MoneyMessenger : BaseMessenger
    {
        /// <summary>
        /// Sends all farmers the "EqualMoneySplit.MoneyListener" message to tell them to update their money
        /// </summary>
        /// <param name="newMoney">The amount of money for each farmer to receive</param>
        public void SendWalletNotification(int newMoney)
        {
            MoneyPayload moneyData = new MoneyPayload(newMoney, Game1.player.Name, EventContext.InventoryChanged);
            base.SendCoreMessageToAllFarmers(Constants.ModListenerAddress, moneyData);

            Game1.chatBox.addInfoMessage("You sent every player " + newMoney + "g.");

            EqualMoneyMod.Logger.Log("MoneyMessengerUtil.SendWalletNotification | Local farmer " + Game1.player.Name + " is sending " + newMoney + " to all farmers");
        }

        /// <summary>
        /// Sends all farmers the "EqualMoneySplit.MoneyListener" message to tell them to update their money
        /// </summary>
        /// <param name="newMoney">The amount of money for each farmer to receive</param>
        public void SendShippingBinNotification(int newMoney)
        {
            MoneyPayload moneyData = new MoneyPayload(newMoney, Game1.player.Name, EventContext.EndOfDay);
            base.SendCoreMessageToAllFarmers(Constants.ModListenerAddress, moneyData);

            Game1.chatBox.addInfoMessage("You sent every player " + newMoney + "g.");

            EqualMoneyMod.Logger.Log("MoneyMessengerUtil.SendShippingBinNotification | Local farmer " + Game1.player.Name + " is sending " + newMoney + " to all farmers");
        }
    }
}
