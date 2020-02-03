using SocialistMoneySplit.Injectors;
using SocialistMoneySplit.Models;
using SocialistMoneySplit.Utils;
using StardewModdingAPI.Events;

namespace SocialistMoneySplit.Events
{
    /// <summary>
    /// Handles events related to when the game saves
    /// </summary>
    public class SaveEventHandlers : BaseEventHandlers
    {
        /// <summary>
        /// Corrects the local Farmer's money before the save begins to occur
        /// </summary>
        /// <param name="sender">The sender of the Saving event</param>
        /// <param name="args">Event arguments for the Saving event</param>
        [OnSaveStartingInjector]
        public void OnSavingHandler(object sender, SavingEventArgs args)
        {
            QuickLogMoney("SaveEventHandler-SAVING");
            MoneySplitUtil.CorrectLocalPlayer(PersistantFarmerData.ShippingBinMoney, PersistantFarmerData.ShareToSend);
        }

        /// <summary>
        /// Handles the event raised after the game is saved
        /// </summary>
        /// <param name="sender">The sender of the Saved event</param>
        /// <param name="args">Event arguments for the Saved event</param>
        public void OnSavedHandler(object sender, SavedEventArgs args)
        {
            QuickLogMoney("SaveEventHandler-SAVED");
        }

    }
}
