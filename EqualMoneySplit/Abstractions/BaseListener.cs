﻿using EqualMoneySplit.Models;
using EqualMoneySplit.Networking;
using StardewModdingAPI.Events;
using System;
using System.Threading.Tasks;

namespace EqualMoneySplit.Abstractions
{
    public abstract class BaseListener
    {

        /// <summary>
        /// Action/Method that will be performed when a message is received
        /// </summary>
        public Action<object> MessageHandler { get; set; }

        /// <summary>
        /// Destination address the message will be received from
        /// </summary>
        public abstract string Address { get; }

        /// <summary>
        /// Listener for a specific mod address
        /// </summary>
        public BaseListener()
        {
            MessageHandler = CreateMessageHandler();
        }

        /// <summary>
        /// Initializes the listener that will fire when the "EqualMoneySplit.MoneyListener" message is sent
        /// </summary>
        /// <returns>The action to be performed when a response is received</returns>
        public abstract Action<object> CreateMessageHandler();

        /// <summary>
        /// Begins the process of checking for messages every game tick
        /// </summary>
        public virtual void Start()
        {
            EqualMoneyMod.SMAPI.Events.GameLoop.UpdateTicked += CheckForNewMessages;
        }

        /// <summary>
        /// Ends the process of checking for messages every game tick
        /// </summary>
        public virtual void Stop()
        {
            EqualMoneyMod.SMAPI.Events.GameLoop.UpdateTicked -= CheckForNewMessages;
        }

        /// <summary>
        /// Check for unhandled messages
        /// </summary>
        /// <param name="sender">The sender of the UpdateTicking event</param>
        /// <param name="args">Event arguments for the UpdateTicking event</param>
        public virtual void CheckForNewMessages(object sender = null, UpdateTickedEventArgs args = null)
        {
            foreach (NetworkMessage message in Network.Instance.RetrieveMessages(Address))
                Task.Run(() => { MessageHandler(message.Payload); });
        }
    }
}
