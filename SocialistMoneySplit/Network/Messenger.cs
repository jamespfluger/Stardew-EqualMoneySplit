using SocialistMoneySplit.Models;
using SocialistMoneySplit.Utils;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SocialistMoneySplit.Network
{
    /// <summary>
    /// Handles outgoing messages to Farmers and the corresponding responses 
    /// </summary>
    /// <typeparam name="T">Type of payload to be sent</typeparam>
    public static class Messenger<T>
    {
        /// <summary>
        /// Sends a message to a given address
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient"></param>
        public static void SendMessageToFarmer(string address, object payload, long recipient = -1)
        {
            SocialismMod.Monitor.Log("Sending a message to " + address + " for " + Game1.getFarmer(recipient));
            SocialismMod.Helper.Multiplayer.SendMessage((T)payload, address, new[] { SocialismMod.Helper.Multiplayer.ModID }, recipient != -1 ? new[] { recipient } : null);
        }

        /// <summary>
        /// Sends a message to all Farmers
        /// </summary>
        /// <param name="address">Destination address to send the payload</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        public static void SendMessageToAllFarmers(string address, object payload)
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
        private static Task SendPayloadToFarmer(string address, object payload, long recipient)
        {
            return Task.Run(() =>
            {
                // Establish a unique return address for this specific request
                string returnAddress = address + "." + Guid.NewGuid();
                bool hasReceivedPayload = false;
                T receivedPayload = default(T);

                // We need to set up a specific event handler for this address
                EventHandler<ModMessageReceivedEventArgs> onIfMessageReceived = (sender, args) => CheckIfMessageWasDelivered(returnAddress, ref receivedPayload, ref hasReceivedPayload);
                SocialismMod.Helper.Events.Multiplayer.ModMessageReceived += onIfMessageReceived;
                
                // Send the message to the farmer
                SendMessageToFarmer(address, payload, recipient);

                // Hold execution until we find out if our message was actually received
                int intervalsWaited = 0;
                while (!hasReceivedPayload && intervalsWaited < 1000)
                {
                    Thread.Sleep(10);
                    intervalsWaited++;
                }

                if (intervalsWaited <= 0)
                    SocialismMod.Monitor.Log("MessageSender.SendMessageToFarmer | A request failed to be retrieved properly");

                // We remove the event handler after we received the response
                SocialismMod.Helper.Events.Multiplayer.ModMessageReceived -= onIfMessageReceived;
            });
        }

        /// <summary>
        /// Check to see if the message was delivered
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="resultPayload">Result of a received message</param>
        /// <param name="hasReceivedPayload">Whether or not a message has been received</param>
        private static void CheckIfMessageWasDelivered(string address, ref T resultPayload, ref bool hasReceivedPayload)
        {
            foreach (T retrievedPayload in PayloadHandler<T>.RetrievePayloads(address))
            {
                resultPayload = retrievedPayload;
                hasReceivedPayload = true;
                break;
            }

            if (hasReceivedPayload)
                NetworkUtil<T>.Messages.TryRemove(address, out List<NetworkMessage> throwaway);
        }
    }
}
