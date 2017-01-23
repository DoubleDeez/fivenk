using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    public class FiveNK : Script
    {
        public FiveNK()
        {
            API.onResourceStart += OnResourceStartHandler;
            API.onPlayerConnected += OnPlayerConnectHandler;
            API.onPlayerDisconnected += OnPlayerDisconnectHandler;
            API.onPlayerRespawn += OnPlayerRespawnHandler;
        }

        public void OnResourceStartHandler()
        {
            API.setGamemodeName("FiveNK-RP");
        }

        public void OnPlayerConnectHandler(Client player)
        {
            API.sendNotificationToAll("~b~~h~" + player.name + "~h~ ~w~joined.");
        }

        public void OnPlayerDisconnectHandler(Client player, string reason)
        {
            API.sendNotificationToAll("~b~~h~" + player.name + "~h~ ~w~quit. (" + reason + ")");
        }

        public void OnPlayerRespawnHandler(Client player)
        {
            API.setEntityInvincible(player.handle, true);
            AsyncHelpers.Delay(10000, () =>
            {
                API.setEntityInvincible(player.handle, false);
            });
        }

        [Command("god")]
        public void ToggleGodMode(Client sender)
        {
            bool isEnabled = !API.getEntityInvincible(sender);
            API.setEntityInvincible(sender, isEnabled);
            API.sendNotificationToPlayer(sender, String.Format("God mode {0}", isEnabled ? "Enabled" : "Disabled"));
        }

        [Command("whisper", Alias = "w", GreedyArg = true)]
        public void WhisperPlayer(Client sender, Client target, string message)
        {
            API.sendChatMessageToPlayer(target, "~g~" + API.getPlayerName(sender) + " whipsers: ~w~" + message);
            target.setSyncedData("ReplyTo", sender);
        }
    }
}
