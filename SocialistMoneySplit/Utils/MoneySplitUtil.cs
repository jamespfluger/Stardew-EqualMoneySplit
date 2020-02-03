using StardewValley;
using System;

namespace SocialistMoneySplit.Utils
{
    /// <summary>
    /// Utility used to split money between Farmers
    /// </summary>
    public static class MoneySplitUtil
    {
        /// <summary>
        /// Calculates the equal amount of Money each Farmer will receive
        /// </summary>
        /// <param name="totalNewMoney">The amount of money to be divided equally</param>
        /// <returns>An event amount each Farmer will receive from the share</returns>
        public static int GetPerPlayerShare(int totalNewMoney)
        {
            int numberOfFarmers = Game1.getOnlineFarmers().Count;
            int moneyPerPlayer = (int)Math.Ceiling(Convert.ToDouble(totalNewMoney) / numberOfFarmers);
            SocialismMod.Monitor.Log("MoneySplitUtil-GetPerPlayerShare | Each player will receive: " + moneyPerPlayer);
            return moneyPerPlayer;
        }

        /// <summary>
        /// Corrects the current Farmer's Money after they have earned money for their own wallet
        /// </summary>
        /// <param name="totalNewMoney">The amount of Money the Farmer initially earned</param>
        /// <param name="moneyPerPlayer">The corrected even share of Money the Farmer should have earned</param>
        public static void CorrectLocalPlayer(int totalNewMoney, int moneyPerPlayer)
        {
            // Update the current farmer's money  |  Curr = Curr - (Split Share * (Number of Farmers-1))
            SocialismMod.Monitor.Log(@"MoneySplitUtil-CorrectLocalPlayer | Local farmer " + Game1.player.Name + " |previous:" + Game1.player.Money   + "|remove:" + totalNewMoney + "|add:" + moneyPerPlayer + "|result:" + (Game1.player.Money - totalNewMoney + moneyPerPlayer));
            Game1.player.Money = Game1.player.Money - totalNewMoney + moneyPerPlayer;
        }
    }
}
