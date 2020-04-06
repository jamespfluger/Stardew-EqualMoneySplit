using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Linq;

namespace EqualMoneySplit.Events
{
    public class MultiplayerEventHandlers
    {
        public void OnPeerContextReceived(object sender, PeerContextReceivedEventArgs args)
        {
            IMultiplayerPeer newPlayer = args.Peer;

            string errorMessage = "";
            var currentPlayerMod = EqualMoneyMod.SMAPI.ModRegistry.Get(Models.Constants.ModId);
            var newPeerMod = newPlayer.GetMod(Models.Constants.ModId);

            if (!args.Peer.HasSmapi)
                errorMessage = "Player {0} does not have SMAPI installed! EqualMoneySplit will not function properly!";
            else if (newPlayer.Mods.Count() == 0)
                errorMessage = "Player {0} does not have any mods installed! EqualMoneySplit will not function properly!";
            else if (newPeerMod == null)
                errorMessage = "Player {0} does not have EqualMoneySplit installed! EqualMoneySplit will not function properly!";
            else if(newPeerMod.Version.IsOlderThan(currentPlayerMod.Manifest.Version))
                errorMessage = "Player {0} has an older version of EqualMoneySplit than yours! EqualMoneySplit will not function properly!";
            else if (newPeerMod.Version.IsNewerThan(currentPlayerMod.Manifest.Version))
                errorMessage = "Player {0} has a newer version of EqualMoneySplit than yours! EqualMoneySplit will not function properly!";

            if (!string.IsNullOrEmpty(errorMessage))
                Game1.chatBox.addErrorMessage(string.Format(errorMessage, newPlayer.PlayerID));
        }
    }
}
