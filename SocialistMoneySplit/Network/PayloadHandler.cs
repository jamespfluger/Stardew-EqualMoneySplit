using SocialistMoneySplit.Models;
using SocialistMoneySplit.Utils;
using System.Collections.Generic;

namespace SocialistMoneySplit.Network
{
    public static class PayloadHandler<T>
    {

        /// <summary>
        /// Receive a set of messages from a Farmer
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="sender">ID of Farmer who sent messages</param>
        /// <returns>List of payloads received</returns>
        public static IEnumerable<T> RetrievePayloads(string address, long sender = -1)
        {
            foreach (NetworkMessage message in GatherNewMessages(address, sender))
                yield return (T)message.Payload;
        }

        /// <summary>
        /// Checks for new messages from a particular farmer that haven't been handled yet
        /// </summary>
        /// <param name="address">Destination address to check for message</param>
        /// <param name="sender">ID of Farmer that sent a message</param>
        /// <returns></returns>
        private static IEnumerable<NetworkMessage> GatherNewMessages(string address, long sender = -1)
        {
            NetworkUtil<T>.Messages.TryAdd(address, new List<NetworkMessage>());

            List<NetworkMessage> messages = new List<NetworkMessage>(NetworkUtil<T>.Messages[address]);

            foreach (NetworkMessage message in messages)
            {
                if (sender == -1 || sender == message.Sender)
                {
                    NetworkUtil<T>.Messages[address].Remove(message);
                    yield return message;
                }
            }
        }
    }
}
