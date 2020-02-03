using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Threading.Tasks;

namespace SocialistMoneySplit.Network
{
    /// <summary>
    /// Service that handles incoming messages 
    /// </summary>
    /// <typeparam name="T">Type of message that will be received</typeparam>
    public class Receiver<T>
    {
        /// <summary>
        /// Destination address the message will be received from
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Action/Method that will be performed when a request is received
        /// </summary>
        public Action<T> RequestHandler { get; set; }

        /// <summary>
        /// Receiver for a specific mod address
        /// </summary>
        /// <param name="address">Destination address the message will be received from</param>
        /// <param name="requestHandler">Action/Method that will be performed when a request is received</param>
        public Receiver(string address, Action<T> requestHandler)
        {
            this.Address = address;
            this.RequestHandler = requestHandler;
        }

        /// <summary>
        /// Begins the process of checking for messages
        /// </summary>
        public void Start()
        {
            SocialismMod.Helper.Events.GameLoop.UpdateTicked += CheckForNewMessages;
        }

        /// <summary>
        /// Ends the process of checking for messages
        /// </summary>
        public void Stop()
        {
            SocialismMod.Helper.Events.GameLoop.UpdateTicked -= CheckForNewMessages;
        }

        /// <summary>
        /// Check for requests in multiplayer games every tick
        /// </summary>
        /// <param name="sender">The sender of the UpdateTicking event</param>
        /// <param name="args">Event arguments for the UpdateTicking event</param>
        public void CheckForNewMessages(object sender = null, UpdateTickedEventArgs args = null)
        {
            if (!Game1.IsMultiplayer)
                return;

            foreach (T payload in PayloadHandler<T>.RetrievePayloads(Address))
                Task.Run(() => { RequestHandler((T)payload); });
        }
    }
}

