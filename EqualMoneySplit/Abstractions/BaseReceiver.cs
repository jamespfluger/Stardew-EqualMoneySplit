using EqualMoneySplit.Models;
using EqualMoneySplit.Networking;
using StardewModdingAPI.Events;
using System;
using System.Threading.Tasks;

namespace EqualMoneySplit.Abstractions
{
    public abstract class BaseReceiver
    {

        /// <summary>
        /// Action/Method that will be performed when a request is received
        /// </summary>
        public Action<object> RequestHandler { get; set; }

        /// <summary>
        /// Destination address the message will be received from
        /// </summary>
        public abstract string Address { get; }

        /// <summary>
        /// Receiver for a specific mod address
        /// </summary>
        public BaseReceiver()
        {
            RequestHandler = CreateHandler();
        }

        /// <summary>
        /// Initializes the receiver that will fire when the "EqualMoneySplit.MoneyReceiver" message is sent
        /// </summary>
        /// <returns>The action to be performed when a response is received</returns>
        public abstract Action<object> CreateHandler();

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
                Task.Run(() => { RequestHandler(message.Payload); });
        }
    }
}
