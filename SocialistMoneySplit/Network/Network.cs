using SocialistMoneySplit.Models;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SocialistMoneySplit.Networking
{
    /// <summary>
    /// Utility that sends or receives messages over the Network
    /// </summary>
    public partial class Network
    {
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
        /// Checks for new messages from a particular farmer that haven't been handled yet
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="sender">ID of Farmer that sent a message</param>
        /// <returns></returns>
        public IEnumerable<NetworkMessage> RetrieveMessages(string address, long sender = -1)
        {
            Messages.TryAdd(address, new List<NetworkMessage>());

            List<NetworkMessage> messages = new List<NetworkMessage>(Messages[address]);

            foreach (NetworkMessage message in messages)
            {
                if (sender == -1 || sender == message.Sender)
                {
                    Network.Instance.Messages[address].Remove(message);
                    yield return message;
                }
            }
        }
    }
}
