namespace SocialistMoneySplit.Models
{
    /// <summary>
    /// Context of Player/Money to be sent to other players
    /// </summary>
    public class MoneyPayload
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

        public MoneyPayload() { }

        /// <summary>
        /// Context of Farmer/Money information to be sent to other Farmers
        /// </summary>
        /// <param name="money">Share of Money to be sent to other Farmers</param>
        /// <param name="name">Name of Farmer sending the Money</param>
        public MoneyPayload(int money, string name)
        {
            this.Money = money;
            this.Name = name;
        }

        /// <summary>
        /// Context of Farmer/Money/Event information to be sent to other Farmers
        /// </summary>
        /// <param name="money">Share of Money to be sent to other Farmers</param>
        /// <param name="name">Name of Farmer sending the Money</param>
        /// <param name="eventContext">The event that is sending the money</param>
        /// 
        public MoneyPayload(int money, string name, EventContext eventContext)
        {
            this.Money = money;
            this.Name = name;
            this.EventContext = eventContext;
        }
    }
}
