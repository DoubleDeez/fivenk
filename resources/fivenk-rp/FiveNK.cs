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
            API.onResourceStop += OnResourceStopHandler;
            API.onPlayerConnected += OnPlayerConnectHandler;
            API.onPlayerDisconnected += OnPlayerDisconnectHandler;
            API.onPlayerRespawn += OnPlayerRespawnHandler;
        }

        public void OnResourceStartHandler()
        {
            API.setGamemodeName("FiveNK-RP");
            BlipManager.InitBlips();
        }

        public void OnResourceStopHandler()
        {
            Database.DeInit();
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
    }
}
