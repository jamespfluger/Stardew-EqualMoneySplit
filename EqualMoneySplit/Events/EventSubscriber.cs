using EqualMoneySplit.Networking;
using EqualMoneySplit.Networking.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualMoneySplit.Events
{
    public class EventSubscriber

    {
        /// <summary>
        /// Global instance of EventSubscriber
        /// </summary>
        public static EventSubscriber Instance { get { return lazyEventSubscriber.Value; } }

        /// <summary>
        /// Local instance of the EventSubscriber class loaded lazily
        /// </summary>
        private static readonly Lazy<EventSubscriber> lazyEventSubscriber = new Lazy<EventSubscriber>(() => new EventSubscriber());

        // Initialize all event handler classes
        private readonly InventoryEventHandlers inventoryChangedHandler;
        private readonly GameLoopEventHandlers gameLoopHandler;
        private readonly SaveEventHandlers saveEventHandler;

        /// <summary>
        /// Handles the subscriptions and unsubscriptions of events after criteria are checked
        /// </summary>
        public EventSubscriber()
        {
            inventoryChangedHandler = new InventoryEventHandlers();
            gameLoopHandler  = new GameLoopEventHandlers();
            saveEventHandler = new SaveEventHandlers();
        }

        public void AddSubscriptions()
        {
            // Start any mod message listeners we need
            StartNetworkListeners();

            // Instantiate all events needed
            EqualMoneyMod.SMAPI.Events.Player.InventoryChanged += inventoryChangedHandler.OnInventoryChanged;
            EqualMoneyMod.SMAPI.Events.GameLoop.UpdateTicking += gameLoopHandler.OnUpdateTicking;
            EqualMoneyMod.SMAPI.Events.GameLoop.DayStarted += gameLoopHandler.OnDayStartedHandler;
            EqualMoneyMod.SMAPI.Events.GameLoop.DayEnding += gameLoopHandler.OnDayEndingHandler;
            EqualMoneyMod.SMAPI.Events.GameLoop.Saving += saveEventHandler.OnSavingHandler;
            EqualMoneyMod.SMAPI.Events.GameLoop.Saved += saveEventHandler.OnSavedHandler;

            EqualMoneyMod.SMAPI.Events.Multiplayer.ModMessageReceived += Network.Instance.OnModMessageReceived;
        }

        public void RemoveSubscriptions()
        {
            // Remove events that shouldn't be triggering
            EqualMoneyMod.SMAPI.Events.Player.InventoryChanged -= inventoryChangedHandler.OnInventoryChanged;
            EqualMoneyMod.SMAPI.Events.GameLoop.UpdateTicking -= gameLoopHandler.OnUpdateTicking;
            EqualMoneyMod.SMAPI.Events.GameLoop.DayStarted -= gameLoopHandler.OnDayStartedHandler;
            EqualMoneyMod.SMAPI.Events.GameLoop.DayEnding -= gameLoopHandler.OnDayEndingHandler;
            EqualMoneyMod.SMAPI.Events.GameLoop.Saving -= saveEventHandler.OnSavingHandler;
            EqualMoneyMod.SMAPI.Events.GameLoop.Saved -= saveEventHandler.OnSavedHandler;

            EqualMoneyMod.SMAPI.Events.Multiplayer.ModMessageReceived -= Network.Instance.OnModMessageReceived;

            // Stop any mod message listeners we started
            StopNetworkListeners();
        }

        /// <summary>
        /// Start any network listeners needed
        /// </summary>
        private void StartNetworkListeners()
        {
            MoneyListener.Instance.Start();
        }

        /// <summary>
        /// Stop the listeners we started earlier
        /// </summary>
        private void StopNetworkListeners()
        {
            MoneyListener.Instance.Stop();
        }
    }
}
