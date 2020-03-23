using EqualMoneySplit.Networking.Models;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EqualMoneySplit.Networking.Communicators
{
    /// <summary>
    /// Handles outgoing messages to Farmers and the corresponding responses 
    /// </summary>
    public abstract class Messenger
    {
        /// <summary>
        /// Sends a message to a given Farmer, defaulting to all farmers
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient">The farmer who will receive the message</param>
        public virtual void SendMessageToFarmer(string address, object payload, long recipient)
        {
            Message message = new Message(address, payload, recipient);

            SendCoreMessageToFarmer(message);
        }

        /// <summary>
        /// Sends a message to all Farmers
        /// </summary>
        /// <param name="address">Destination address to send the payload</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        public virtual void SendMessageToAllFarmers(string address, object payload)
        {
            Message message = new Message(address, payload, -1);

            SendCoreMessageToFarmer(message);
        }

        /// <summary>
        /// Sends a request to a given Farmer and waits for an acknowledgement back
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient">The farmer who will receive the message</param>
        public void SendRequestToFarmer(string address, object payload, long recipient)
        {
            SendCoreRequestToFarmer(address, payload, recipient);
        }

        /// <summary>
        /// Sends a request to a given Farmer and waits for their acknowledgements
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        public void SendRequestToAllFarmers(string address, object payload)
        {
            foreach(Farmer farmer in Game1.getOnlineFarmers())
            {
                if (farmer.UniqueMultiplayerID != Game1.player.UniqueMultiplayerID)
                    SendCoreRequestToFarmer(address, payload, farmer.UniqueMultiplayerID);
            }
        }

        /// <summary>
        /// Sends a Message with an object payload to a given farmer
        /// </summary>
        /// <param name="message">The message to be delivered to the farmer</param>
        private void SendCoreMessageToFarmer(Message message)
        {
            EqualMoneyMod.Logger.Log($"Local farmer {Game1.player.Name} is sending a message to {message.Address} for {Game1.getFarmer(message.Recipient)}");
            EqualMoneyMod.SMAPI.Multiplayer.SendMessage(message, message.Address, new[] { EqualMoneyMod.SMAPI.Multiplayer.ModID }, message.Recipient != -1 ? new[] { message.Recipient } : null);
        }

        /// <summary>
        /// Sends a Message with an object payload to a given farmer and waits for an acknowledgement
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient">The farmer who will receive the message</param>
        private void SendCoreRequestToFarmer(string address, object payload, long recipient)
        {
            Task requestTask = Task.Run(() =>
            {
                // Establish a unique message ID (and return address) for this message
                Message message = new Message(address, payload, recipient);
                message.MessageId = Guid.NewGuid().ToString();

                bool hasReceivedAcknowledgement = false;

                // We need to set up a specific event handler for this message ID
                EventHandler<ModMessageReceivedEventArgs> onIfMessageReceived = (sender, args) => CheckForMessageDelivery(message.MessageId, ref hasReceivedAcknowledgement);
                EqualMoneyMod.SMAPI.Events.Multiplayer.ModMessageReceived += onIfMessageReceived;

                // Send the message to the farmer
                SendCoreMessageToFarmer(message);
                EqualMoneyMod.Logger.Log($"Attempting to deliver message {message.MessageId} to {message.Recipient}");

                // Hold execution until we find out if our message was actually received
                int intervalsWaited = 0;
                while (!hasReceivedAcknowledgement && intervalsWaited < 1000)
                {
                    Thread.Sleep(10);
                    intervalsWaited++;
                }

                if (!hasReceivedAcknowledgement)
                    EqualMoneyMod.Logger.Log($"Message {message.MessageId} did not receive a response from {message.Recipient}");
                else
                    EqualMoneyMod.Logger.Log($"Message {message.MessageId} was successfully delivered to {message.Recipient}");

                // We remove the event handler after we received the response
                EqualMoneyMod.SMAPI.Events.Multiplayer.ModMessageReceived -= onIfMessageReceived;
            });
        }

        /// <summary>
        /// Checks to see if a message was delivered
        /// </summary>
        /// <param name="messageId">Unique ID of a message we are looking for</param>
        /// <param name="hasReceivedPayload">Whether or not a message has been received</param>
        private void CheckForMessageDelivery(string messageId, ref bool hasReceivedPayload)
        {
            if (Network.Instance.RetrieveMessages(messageId).Any())
            {
                hasReceivedPayload = true;
                Network.Instance.ReceivedMessages.TryRemove(messageId, out _);
            }
        }
    }
}
