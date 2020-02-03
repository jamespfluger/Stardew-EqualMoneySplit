using static SocialistMoneySplit.Utils.MoneyReceiverUtil;

namespace SocialistMoneySplit.Models
{
    /// <summary>
    /// Context of Player/Money to be sent to other players
    /// </summary>
    public class NetworkPayload
    {
        /// <summary>
        /// Share of Money to be sent to other Farmers
        /// </summary>
        public int Money { get; set; }
        /// <summary>
        /// Name of Farmer sending the Money
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The event that is sending the money
        /// </summary>
        public EventContext EventContext { get; set; }

        public NetworkPayload() { }
        /// <summary>
        /// Context of Farmer/Money information to be sent to other Farmers
        /// </summary>
        /// <param name="money">Share of Money to be sent to other Farmers</param>
        /// <param name="name">Name of Farmer sending the Money</param>
        /// <param name="eventContext">The event that is sending the money</param>
        public NetworkPayload(int money, string name, EventContext eventContext)
        {
            this.Money = money;
            this.Name = name;
            this.EventContext = eventContext;
        }
    }
}
