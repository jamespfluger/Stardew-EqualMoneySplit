using SocialistMoneySplit.Abstractions;
using SocialistMoneySplit.Models;
using SocialistMoneySplit.Networking.Communicators;
using SocialistMoneySplit.Utils;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace SocialistMoneySplit.Events
{
    /// <summary>
    /// Handles events related to game loops
    /// </summary>
    public class GameLoopEventHandlers : BaseEventHandlers
    {
        /// <summary>
        /// Updates the local Farmer's stored money value before the game state is updated (~60 times per second)
        /// </summary>
        /// <param name="sender">The sender of the UpdateTicking event</param>
        /// <param name="args">Event arguments for the UpdateTicking event</param>
        public void OnUpdateTicking(object sender, UpdateTickingEventArgs args)
        {
            if (!Context.IsMultiplayer)
                return;

            PersistantFarmerData.PocketMoney = Game1.player.Money;
        }

        /// <summary>
        /// Calculates the shipping bin money before the day begins to end and
        /// sends the split share of that value after this function but before the day begins to end
        /// </summary>
        /// <param name="sender">The sender of the DayEndingEvent event</param>
        /// <param name="args">Event arguments for the DayEndingEvent event</param>
        public void OnDayEndingHandler(object sender, DayEndingEventArgs args)
        {
            QuickLogMoney("GameLoopEventHandler | DayEnding");
            SocialismMod.Logger.Log("Current location: " + Game1.player.currentLocation.Name);

            // Calculate all money that will be received from the shipping bin
            PersistantFarmerData.ShippingBinMoney = ItemValueUtil.CalculateItemCollectionValue(Game1.player.personalShippingBin);
            PersistantFarmerData.ShareToSend = MoneySplitUtil.GetPerPlayerShare(PersistantFarmerData.ShippingBinMoney);

            // Always send a notification, even if no money has changed
            MoneyMessenger moneyMessenger = new MoneyMessenger();
            moneyMessenger.SendShippingBinNotification(PersistantFarmerData.ShareToSend);
        }

        /// <summary>
        /// Resets the saved value in the shipping bin and the share from that
        /// </summary>
        /// <param name="sender">The sender of the DayStarted event</param>
        /// <param name="args">Event arguments for the DayStarted event</param>
        public void OnDayStartedHandler(object sender, DayStartedEventArgs args)
        {
            QuickLogMoney("GameLoopEventHandler | DayStarted");
            SocialismMod.Logger.Log("Current location: " + Game1.player.currentLocation.Name);

            PersistantFarmerData.ShareToSend = 0;
            PersistantFarmerData.ShippingBinMoney = 0;
        }
    }
}
