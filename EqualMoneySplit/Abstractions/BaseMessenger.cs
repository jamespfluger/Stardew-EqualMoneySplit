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
        /// Sends a message to a given Farmer, defaulting to all farmers
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="payload">Payload data to be delivered to the Farmer</param>
        /// <param name="recipient"></param>
        public virtual void SendCoreMessageToFarmer(string address, object payload, long recipient = -1)
        {
            SendPayloadToFarmer(address, payload, recipient);
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
        private void SendPayloadToFarmer(string address, object payload, long recipient)
        {
            SendCoreMessageToFarmer(address, payload, recipient);
            EqualMoneyMod.Logger.Log("Messenger.SendMessageToFarmer() | Local farmer " + Game1.player.Name + " Is sending a message to " + address + " for " + Game1.getFarmer(recipient));
            EqualMoneyMod.SMAPI.Multiplayer.SendMessage(payload, address, new[] { EqualMoneyMod.SMAPI.Multiplayer.ModID }, recipient != -1 ? new[] { recipient } : null);
        }
    }
}
