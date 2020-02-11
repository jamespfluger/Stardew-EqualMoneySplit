using SocialistMoneySplit.Abstractions;
using SocialistMoneySplit.Models;
using SocialistMoneySplit.Networking.Communicators;
using SocialistMoneySplit.Utils;
using StardewModdingAPI.Events;
using StardewValley;

namespace SocialistMoneySplit.Events
{
    /// <summary>
    /// Handles events related to when the game saves
    /// </summary>
    public class SaveEventHandlers : BaseEventHandlers
    {
        /// <summary>
        /// Corrects the local Farmer's money before the save begins to occur
        /// Also forces the game to handle any messages that have not been handled
        /// </summary>
        /// <param name="sender">The sender of the Saving event</param>
        /// <param name="args">Event arguments for the Saving event</param>
        public void OnSavingHandler(object sender, SavingEventArgs args)
        {
            QuickLogMoney("SaveEventHandler | Saving");
            SocialismMod.Logger.Log("Current location: " + Game1.player.currentLocation.Name);

            // Correct the local player's money after they have
            MoneySplitUtil.CorrectLocalPlayer(PersistantFarmerData.ShippingBinMoney, PersistantFarmerData.ShareToSend);
            
            // Force the receiver to check for unhandled messages
            MoneyReceiver.Instance.CheckForNewMessages();
        }

        /// <summary>
        /// Handles the event raised after the game is saved
        /// </summary>
        /// <param name="sender">The sender of the Saved event</param>
        /// <param name="args">Event arguments for the Saved event</param>
        public void OnSavedHandler(object sender, SavedEventArgs args)
        {
            QuickLogMoney("SaveEventHandler | Saved");
            SocialismMod.Logger.Log("Current location: " + Game1.player.currentLocation.Name);
        }

    }
}
