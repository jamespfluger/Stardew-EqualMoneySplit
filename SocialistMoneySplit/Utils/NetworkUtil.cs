using SocialistMoneySplit.Models;
using SocialistMoneySplit.Network;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SocialistMoneySplit.Utils
{
    /// <summary>
    /// Utility that sends or receives messages over the Network
    /// </summary>
    public static class NetworkUtil<T> 
    {
        /// <summary>
        /// Messages that will be sent or received
        /// </summary>
        public static ConcurrentDictionary<string, List<NetworkMessage>> Messages { get; set; }

        /// <summary>
        /// Utility for handling Network messages
        /// </summary>
        static NetworkUtil()
        {
            Messages = new ConcurrentDictionary<string, List<NetworkMessage>>();
        }

        /// <summary>
        /// Handles the event raised after a mod message is received
        /// </summary>
        /// <param name="sender">The sender of the ModMessageReceived event</param>
        /// <param name="args">Event arguments for the ModMessageReceived event</param>
        public static void OnModMessageReceived(object sender, ModMessageReceivedEventArgs args)
        {
            if (!Messages.ContainsKey(args.Type))
                Messages.TryAdd(args.Type, new List<NetworkMessage>());

            NetworkMessage message = new NetworkMessage();
            message.Address = args.Type;
            message.Sender = Game1.getFarmer(args.FromPlayerID).UniqueMultiplayerID;
            message.Payload = args.ReadAs<T>();
            message.Receiver = Game1.player.UniqueMultiplayerID;

            Messages[args.Type].Add(message);
        }

        /// <summary>
        /// Sends a message to a single Farmer
        /// </summary>
        /// <param name="address">Destination address the message will be sent to</param>
        /// <param name="payload">Message that will be sent</param>
        public static void SendMessage(string address, object payload, long recipient = -1)
        {
            Messenger<T>.SendMessageToFarmer(address, payload, recipient);
        }

        /// <summary>
        /// Sends a message to all Farmers
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload to be sent to Farmers</param>
        public static void SendMessageToAllFarmers(string address, object payload)
        {
            Messenger<T>.SendMessageToAllFarmers(address, payload);
        }
    }
}
