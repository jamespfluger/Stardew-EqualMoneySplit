using NetworkMessenger.Models;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NetworkMessenger
{
    /// <summary>
    /// Utility that sends or receives messages over the Network
    /// </summary>
    public class Network
    {

        public static IMonitor Logger { get; set; }
        public static IModHelper SMAPI { get; set; }

        /// <summary>
        /// Messages that will be sent or received
        /// </summary>
        public ConcurrentDictionary<string, List<NetworkMessage>> Messages { get; set; }
        /// <summary>
        /// Global instance of Network
        /// </summary>
        public static Network Instance { get { return network.Value; } }
        /// <summary>
        /// Local instance of the Network class loaded lazily
        /// </summary>
        private static readonly Lazy<Network> network = new Lazy<Network>(() => new Network());

        /// <summary>
        /// Utility for handling Network messages
        /// </summary>
        public Network()
        {
            Messages = new ConcurrentDictionary<string, List<NetworkMessage>>();
        }

        /// <summary>
        /// Handles the event raised after a mod message is received
        /// </summary>
        /// <param name="sender">The sender of the ModMessageReceived event</param>
        /// <param name="args">Event arguments for the ModMessageReceived event</param>
        public void OnModMessageReceived(object sender, ModMessageReceivedEventArgs args)
        {
            if (!Messages.ContainsKey(args.Type))
                Messages.TryAdd(args.Type, new List<NetworkMessage>());

            NetworkMessage message = new NetworkMessage();
            message.Address = args.Type;
            message.Sender = Game1.getFarmer(args.FromPlayerID).UniqueMultiplayerID;
            message.Payload = args.ReadAs<object>();
            message.Receiver = Game1.player.UniqueMultiplayerID;

            Messages[args.Type].Add(message);
        }

        /// <summary>
        /// Sends a message to a single Farmer
        /// </summary>
        /// <param name="address">Destination address the message will be sent to</param>
        /// <param name="payload">Message that will be sent</param>
        public void SendMessage(string address, object payload, long recipient = -1)
        {
            //
        }

        /// <summary>
        /// Sends a message to all Farmers
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload to be sent to Farmers</param>
        public void SendMessageToAllFarmers(string address, object payload)
        {
            //
        }
    }
}
