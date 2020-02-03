using SocialistMoneySplit.Events;
using SocialistMoneySplit.Utils;
using StardewModdingAPI;

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
        public static IMonitor Monitor { get; set; }
        /// <summary>
        /// The SMAPI API used to integrate mods with the base Stardew Valley game
        /// </summary>
        public static IModHelper Helper { get; set; }

        /// <summary>
        /// Entry point of SocialismMod
        /// </summary>
        /// <param name="helper"></param>
        public override void Entry(IModHelper helper)
        {
            
            Monitor = base.Monitor;
            Helper = base.Helper;

            // Initialize all static classes
            MoneyReceiverUtil.Init();

            // Initialize all event handler classes
            //    TODO: consider making these static? Nothing seems to have a state here...
            InventoryEventHandlers inventoryChangedHandler = new InventoryEventHandlers();
            GameLoopEventHandlers gameLoopHandler = new GameLoopEventHandlers();
            SaveEventHandlers saveEventHandler = new SaveEventHandlers();

            // Instantiate all events needed
            helper.Events.Player.InventoryChanged += inventoryChangedHandler.OnInventoryChanged;
            helper.Events.GameLoop.UpdateTicking += gameLoopHandler.OnUpdateTicking;
            helper.Events.GameLoop.DayStarted += gameLoopHandler.OnDayStartedHandler;
            helper.Events.GameLoop.DayEnding += gameLoopHandler.OnDayEndingHandler;
            helper.Events.GameLoop.Saving += saveEventHandler.OnSavingHandler;
            helper.Events.GameLoop.Saved += saveEventHandler.OnSavedHandler;
        }
    }
}
