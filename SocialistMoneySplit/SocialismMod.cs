using SocialistMoneySplit.Events;
using SocialistMoneySplit.Utils;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace SocialistMoneySplit
{
    /// <summary>
    /// Mod used to evenly split money earned from selling items between farmers
    /// </summary>
    public class SocialismMod : Mod
    {
        /// <summary>
        /// The SMAPI API used for monitoring and logging
        /// </summary>
        public static IMonitor Logger { get; private set; }
        /// <summary>
        /// The SMAPI API used to integrate mods with the base Stardew Valley game
        /// </summary>
        public static IModHelper SMAPI { get; private set; }

        // Initialize all event handler classes
        private InventoryEventHandlers inventoryChangedHandler = new InventoryEventHandlers();
        private GameLoopEventHandlers gameLoopHandler = new GameLoopEventHandlers();
        private SaveEventHandlers saveEventHandler = new SaveEventHandlers();
        
        /// <summary>
        /// Checks if this is the first day the user is connecting for
        /// </summary>
        private bool isFirstDay = true;

        /// <summary>
        /// Entry point of SocialismMod
        /// </summary>
        /// <param name="helper"></param>
        public override void Entry(IModHelper helper)
        {
            Logger = base.Monitor;
            SMAPI = base.Helper;

            SMAPI.Events.GameLoop.DayStarted += FirstDayEventSubscriptions;
        }

        private void FirstDayEventSubscriptions(object sender, DayStartedEventArgs args)
        {
            if (Context.IsMultiplayer && isFirstDay)
            {
                // Instantiate all events needed
                SMAPI.Events.Player.InventoryChanged += inventoryChangedHandler.OnInventoryChanged;
                SMAPI.Events.GameLoop.UpdateTicking += gameLoopHandler.OnUpdateTicking;
                SMAPI.Events.GameLoop.DayStarted += gameLoopHandler.OnDayStartedHandler;
                SMAPI.Events.GameLoop.DayEnding += gameLoopHandler.OnDayEndingHandler;
                SMAPI.Events.GameLoop.Saving += saveEventHandler.OnSavingHandler;
                SMAPI.Events.GameLoop.Saved += saveEventHandler.OnSavedHandler;

                // Start subscribing to the event of returning to the title
                SMAPI.Events.GameLoop.ReturnedToTitle += ReturnToTitleEventUnsubcriptions;
                SMAPI.Events.GameLoop.DayStarted -= FirstDayEventSubscriptions;
            }
        }

        private void ReturnToTitleEventUnsubcriptions(object sender, ReturnedToTitleEventArgs args)
        {
            // Instantiate all events needed
            SMAPI.Events.Player.InventoryChanged -= inventoryChangedHandler.OnInventoryChanged;
            SMAPI.Events.GameLoop.UpdateTicking -= gameLoopHandler.OnUpdateTicking;
            SMAPI.Events.GameLoop.DayStarted -= gameLoopHandler.OnDayStartedHandler;
            SMAPI.Events.GameLoop.DayEnding -= gameLoopHandler.OnDayEndingHandler;
            SMAPI.Events.GameLoop.Saving -= saveEventHandler.OnSavingHandler;
            SMAPI.Events.GameLoop.Saved -= saveEventHandler.OnSavedHandler;

            // Re-add the first day event subscriptions
            SMAPI.Events.GameLoop.DayStarted += FirstDayEventSubscriptions;
            SMAPI.Events.GameLoop.ReturnedToTitle -= ReturnToTitleEventUnsubcriptions;
        }

        private void StartReceivers()
        {
        }

        private void StopReceivers()
        {
        }
    }
}
