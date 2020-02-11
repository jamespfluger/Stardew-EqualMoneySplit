namespace EqualMoneySplit.Models
{
    /// <summary>
    /// Context of Payload that is sent over Network
    /// </summary>
    public class NetworkMessage
    {
        /// <summary>
        /// Destination address the message will be sent to
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Payload to be delivered
        /// </summary>
        public object Payload { get; set; }
        /// <summary>
        /// Farmer sending the message
        /// </summary>
        public long Sender { get; set; }
        /// <summary>
        /// Destination receiver message will be sent to
        /// </summary>
        public long Receiver { get; set; }

        /// <summary>
        /// Message that will be sent over Network
        /// </summary>
        public NetworkMessage() { }

        /// <summary>
        /// Message that will be sent over Network
        /// </summary>
        /// <param name="address">Destination address the message will be sent to</param>
        /// <param name="payload">Payload to be delivered</param>
        /// <param name="sender">Farmer sending the message</param>
        /// <param name="recipient">ID of farmer to send message to</param>
        public NetworkMessage(string address, object payload, long sender, long recipient = -1)
        {
            this.Address = address;
            this.Payload = payload;
            this.Sender = sender;
            this.Receiver = recipient;
        }
    }
}
