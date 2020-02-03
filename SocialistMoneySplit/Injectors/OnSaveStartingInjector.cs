using PostSharp.Aspects;
using SocialistMoneySplit.Utils;
using System;

namespace SocialistMoneySplit.Injectors
{
    /// <summary>
    /// Injector used to handle entry of the Save Starting event method
    /// </summary>
    [Serializable]
    public class OnSaveStartingInjector : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Forces the mod to retrieve unreceived messages before the Save Starting event method executes
        /// </summary>
        /// <param name="args">Arguments of advices of aspects for OnMethodBoundaryAspect</param>
        public override void OnEntry(MethodExecutionArgs args)
        {   
            MoneyReceiverUtil.ForceGetUnarrivedMessages();
        }
    }
}
