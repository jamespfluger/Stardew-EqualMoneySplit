using PostSharp.Aspects;
using SocialistMoneySplit.Models;
using SocialistMoneySplit.Utils;
using StardewValley;
using System;

namespace SocialistMoneySplit.Injectors
{
    /// <summary>
    /// Injector used to handle entry/exit of the Day Ending event method
    /// </summary>
    [Serializable]
    public class OnDayEndingInjector : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Stores current farmer's data and shipping bin's value before the Day Ending event method executes
        /// </summary>
        /// <param name="args">Arguments of advices of aspects for OnMethodBoundaryAspect</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            // Calculate all money that will be received from the shipping bin
            PersistantFarmerData.ShippingBinMoney = ItemValueUtil.CalculateItemCollectionValue(Game1.player.personalShippingBin);
            PersistantFarmerData.ShareToSend = MoneySplitUtil.GetPerPlayerShare(PersistantFarmerData.ShippingBinMoney);
        }

        /// <summary>
        /// Sends the equal share of the earned money after the Day Ending event method executes
        /// </summary>
        /// <param name="args">Arguments of advices of aspects for OnMethodBoundaryAspect</param>
        public override void OnExit(MethodExecutionArgs args)
        {
            MoneyReceiverUtil.SendMoneyUpdateNotification(PersistantFarmerData.ShareToSend, MoneyReceiverUtil.EventContext.EndOfDay);
        }
    }
}
