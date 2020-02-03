using StardewValley;

namespace SocialistMoneySplit.Events
{
    /// <summary>
    /// Contains base logging of current Farmer's money
    /// </summary>
    public abstract class BaseEventHandlers
    {
        /// <summary>
        /// Logs the current Farmer's name and money
        /// </summary>
        /// <param name="eventName">Name of the event occurring</param>
        public void QuickLogMoney(string eventName)
        {
            SocialismMod.Monitor.Log(eventName + " | " + Game1.player.Name + " money:" + Game1.player.Money);
        }
    }
}
