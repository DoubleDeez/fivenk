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
            API.onPlayerConnected += onPlayerConnectHandler;
            API.onPlayerDisconnected += onPlayerDisconnectHandler;
        }

        public void OnResourceStartHandler()
        {
            API.setGamemodeName("FiveNK-RP");
        }

        public void onPlayerConnectHandler(Client player)
        {
            API.sendNotificationToAll("~b~~h~" + player.name + "~h~ ~w~joined.");
        }

        public void onPlayerDisconnectHandler(Client player, string reason)
        {
            API.sendNotificationToAll("~b~~h~" + player.name + "~h~ ~w~quit. (" + reason + ")");
        }
    }
}
