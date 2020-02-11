using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EqualMoneySplit.Abstractions;
using EqualMoneySplit.Models;
using StardewValley;
using System;

namespace EqualMoneySplit.Networking.Communicators
{
    public sealed class MoneyReceiver : BaseReceiver
    {
        /// <summary>
        /// Use static property to handle instance; only created once on day start in a single thread
        /// </summary>
        public static BaseReceiver Instance { get; private set; } = new MoneyReceiver();

        /// <summary>
        /// Destination address the message will be received from
        /// </summary>
        public override string Address => Constants.ModReceiverAddress;

        /// <summary>
        /// Initializes the receiver that will fire when the "EqualMoneySplit.MoneyReceiver" message is sent
        /// </summary>
        /// <returns>The action to be performed when a response is received</returns>
        public override Action<object> CreateHandler()
        {
            return delegate (object payload)
            {
                MoneyPayload networkMoneyData = JsonConvert.DeserializeObject<MoneyPayload>(((JObject)payload).ToString());
                EqualMoneyMod.Logger.Log("MoneyMessengerUtil.ReceivingMoney | Local farmer " + Game1.player.Name + " is receiving " + networkMoneyData.Money + " from " + networkMoneyData.Name);
                EqualMoneyMod.Logger.Log("MoneyMessengerUtil.ReceivingMoney | Local farmer " + Game1.player.Name + " previously had " + Game1.player.Money + " and will now have " + (Game1.player.Money + networkMoneyData.Money));

                if (networkMoneyData.EventContext == EventContext.InventoryChanged)
                    Game1.showGlobalMessage(networkMoneyData.Name + " sent you " + networkMoneyData.Money + "g!");
                if (networkMoneyData.EventContext == EventContext.EndOfDay)
                    Game1.chatBox.addInfoMessage(networkMoneyData.Name + " sent you " + networkMoneyData.Money + "g!");
                
                Game1.player.Money += networkMoneyData.Money;
            };
        }
    }
}
