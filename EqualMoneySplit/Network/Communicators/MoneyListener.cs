using EqualMoneySplit.Abstractions;
using EqualMoneySplit.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewValley;
using System;

namespace EqualMoneySplit.Networking.Communicators
{
    public sealed class MoneyListener : BaseListener
    {
        /// <summary>
        /// Use static property to handle instance; only created once on day start in a single thread
        /// </summary>
        public static BaseListener Instance { get; private set; } = new MoneyListener();

        /// <summary>
        /// Destination address the message will be received from
        /// </summary>
        public override string Address => Constants.ModListenerAddress;

        /// <summary>
        /// Initializes the listener that will fire when the "EqualMoneySplit.MoneyListener" message is sent
        /// </summary>
        /// <returns>The action to be performed when a response is received</returns>
        public override Action<object> CreateMessageHandler()
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
