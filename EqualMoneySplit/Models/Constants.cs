﻿namespace EqualMoneySplit.Models
{
    /// <summary>
    /// Constant values used throughout mod
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Mod ID for EqualMoneySplit
        /// </summary>
        public static string ModId { get; private set; } = EqualMoneyMod.SMAPI.ModRegistry.ModID;

        /// <summary>
        /// Base address mods send/listen to
        /// </summary>
        public static string MoneySplitListenerAddress { get; private set; } = "EqualMoneySplit.Address.MoneySplit";
    }
}
