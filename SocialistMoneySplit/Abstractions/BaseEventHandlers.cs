using StardewValley;

namespace SocialistMoneySplit.Abstractions
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
        public virtual void QuickLogMoney(string eventName)
        {
            SocialismMod.Logger.Log(eventName + " | " + Game1.player.Name + " money:" + Game1.player.Money);
        }
    }
}
