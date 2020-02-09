using NetworkMessenger.Models;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkMessenger.Messengers
{
    public abstract class BaseMessenger
    {

        /// <summary>
        /// Sends a message to a given address
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient"></param>
        public void SendCoreMessageToFarmer(string address, object payload, long recipient = -1)
        {
            Network.Logger.Log("Messenger.SendMessageToFarmer() | Local farmer " + Game1.player.Name + " Is sending a message to " + address + " for " + Game1.getFarmer(recipient));
            Network.SMAPI.Multiplayer.SendMessage(payload, address, new[] { Network.SMAPI.Multiplayer.ModID }, recipient != -1 ? new[] { recipient } : null);
        }

        /// <summary>
        /// Sends a message to all Farmers
        /// </summary>
        /// <param name="address">Destination address to send the payload</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        public void SendCoreMessageToAllFarmers(string address, object payload)
        {
            SendPayloadToFarmer(address, payload, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="payload"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        private Task SendPayloadToFarmer(string address, object payload, long recipient)
        {
            return Task.Run(() =>
            {
                // Establish a unique return address for this specific request
                string returnAddress = address + "." + Guid.NewGuid();
                bool hasReceivedPayload = false;
                object receivedPayload = null;

                // We need to set up a specific event handler for this address
                EventHandler<ModMessageReceivedEventArgs> onIfMessageReceived = (sender, args) => CheckIfMessageWasDelivered(returnAddress, ref receivedPayload, ref hasReceivedPayload);
                Network.SMAPI.Events.Multiplayer.ModMessageReceived += onIfMessageReceived;

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
                    Network.Logger.Log("MessageSender.SendMessageToFarmer() | A request failed to be retrieved properly");

                // We remove the event handler after we received the response
                Network.SMAPI.Events.Multiplayer.ModMessageReceived -= onIfMessageReceived;
            });
        }

        /// <summary>
        /// Check to see if the message was delivered
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="resultPayload">Result of a received message</param>
        /// <param name="hasReceivedPayload">Whether or not a message has been received</param>
        private void CheckIfMessageWasDelivered(string address, ref object resultPayload, ref bool hasReceivedPayload)
        {
            foreach (object retrievedPayload in PayloadRetriever.RetrievePayloads(address))
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
