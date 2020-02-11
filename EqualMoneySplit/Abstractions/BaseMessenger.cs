using EqualMoneySplit.Models;
using EqualMoneySplit.Networking;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EqualMoneySplit.Abstractions
{
    /// <summary>
    /// Handles outgoing messages to Farmers and the corresponding responses 
    /// </summary>
    public abstract class BaseMessenger
    {
        /// <summary>
        /// Sends a message to a given address
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient"></param>
        public virtual void SendCoreMessageToFarmer(string address, object payload, long recipient = -1)
        {
            EqualMoneyMod.Logger.Log("Messenger.SendMessageToFarmer() | Local farmer " + Game1.player.Name + " Is sending a message to " + address + " for " + Game1.getFarmer(recipient));
            EqualMoneyMod.SMAPI.Multiplayer.SendMessage(payload, address, new[] { EqualMoneyMod.SMAPI.Multiplayer.ModID }, recipient != -1 ? new[] { recipient } : null);
        }

        /// <summary>
        /// Sends a message to all Farmers
        /// </summary>
        /// <param name="address">Destination address to send the payload</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        public virtual void SendCoreMessageToAllFarmers(string address, object payload)
        {
            SendPayloadToFarmer(address, payload, -1);
        }

        /// <summary>
        /// Sends a generic object payload to a given farmer
        /// </summary>
        /// <param name="address"></param>
        /// <param name="payload"></param>
        /// <param name="recipient"></param>
        private Task SendPayloadToFarmer(string address, object payload, long recipient)
        {
            return Task.Run(() =>
            {
                // Establish a unique return address for this specific request
                string returnAddress = address + "." + Guid.NewGuid();
                bool hasReceivedPayload = false;
                object receivedPayload = null;

                // We need to set up a specific event handler for this address
                EventHandler<ModMessageReceivedEventArgs> onIfMessageReceived = (sender, args) => CheckForMessageDelivery(returnAddress, ref receivedPayload, ref hasReceivedPayload);
                EqualMoneyMod.SMAPI.Events.Multiplayer.ModMessageReceived += onIfMessageReceived;

                // Send the message to the farmer
                SendCoreMessageToFarmer(address, payload, recipient);

                // Hold execution until we find out if our message was actually received
                int intervalsWaited = 0;
                while (!hasReceivedPayload && intervalsWaited < 1000)
                {
                    Thread.Sleep(10);
                    intervalsWaited++;
                }

                if (intervalsWaited >= 1000)
                    EqualMoneyMod.Logger.Log("MessageSender.SendMessageToFarmer() | A request failed to be retrieved properly");

                // We remove the event handler after we received the response
                EqualMoneyMod.SMAPI.Events.Multiplayer.ModMessageReceived -= onIfMessageReceived;
            });
        }

        /// <summary>
        /// Check to see if a message was delivered
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="resultPayload">Result of a received message</param>
        /// <param name="hasReceivedPayload">Whether or not a message has been received</param>
        private void CheckForMessageDelivery(string address, ref object resultPayload, ref bool hasReceivedPayload)
        {
            foreach (object retrievedPayload in Network.Instance.RetrieveMessages(address))
            {
                resultPayload = retrievedPayload;
                hasReceivedPayload = true;
                break;
            }

            if (hasReceivedPayload)
                Network.Instance.Messages.TryRemove(address, out List<NetworkMessage> throwaway);
        }
    }
}
