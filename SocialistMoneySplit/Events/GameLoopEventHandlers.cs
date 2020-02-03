using SocialistMoneySplit.Injectors;
using SocialistMoneySplit.Models;
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
            if (!Game1.hasLoadedGame)
                return;

            PersistantFarmerData.PocketMoney = Game1.player.Money;
        }

        /// <summary>
        /// Calculates the shipping bin money before the day begins to end and
        /// sends the split share of that value after this function but before the day begins to end
        /// </summary>
        /// <param name="sender">The sender of the DayEndingEvent event</param>
        /// <param name="args">Event arguments for the DayEndingEvent event</param>
        [OnDayEndingInjector]
        public void OnDayEndingHandler(object sender, DayEndingEventArgs args)
        {
            QuickLogMoney("GameLoopEventHandler-EVENT | DayEnding");
        }

        /// <summary>
        /// Handles the event raised after a new day begins
        /// </summary>
        /// <param name="sender">The sender of the DayStarted event</param>
        /// <param name="args">Event arguments for the DayStarted event</param>
        public void OnDayStartedHandler(object sender, DayStartedEventArgs args)
        {
            PersistantFarmerData.ShareToSend = 0;
            PersistantFarmerData.ShippingBinMoney = 0;
            QuickLogMoney("GameLoopEventHandler-EVENT | DayStarted");
        }
    }
}
