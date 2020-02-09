using StardewModdingAPI.Events;
using System;
using System.Threading.Tasks;

namespace NetworkMessenger.Receivers
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

        }

        /// <summary>
        /// Initializes the receiver that will fire when the "NetworkMessenger.MoneyReceiver" message is sent
        /// </summary>
        public abstract Action<object> Create();

        /// <summary>
        /// Begins the process of checking for messages
        /// </summary>
        public virtual void Start()
        {
            Network.SMAPI.Events.GameLoop.UpdateTicked += CheckForNewMessages;
        }

        /// <summary>
        /// Ends the process of checking for messages
        /// </summary>
        public virtual void Stop()
        {
            Network.SMAPI.Events.GameLoop.UpdateTicked -= CheckForNewMessages;
        }

        /// <summary>
        /// Forces the game to retrieve any messages they have not received yet
        /// </summary>
        public virtual void ForceGetUnarrivedMessages()
        {
            CheckForNewMessages();
        }

        /// <summary>
        /// Check for requests in multiplayer games every tick
        /// </summary>
        /// <param name="sender">The sender of the UpdateTicking event</param>
        /// <param name="args">Event arguments for the UpdateTicking event</param>
        public void CheckForNewMessages(object sender = null, UpdateTickedEventArgs args = null)
        {
            foreach (object payload in PayloadRetriever.RetrievePayloads(Address))
                Task.Run(() => { RequestHandler(payload); });
        }
    }
}
