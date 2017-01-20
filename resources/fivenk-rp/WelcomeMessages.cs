using System;
using System.Collections.Generic;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    public class WelcomeMsgs : Script
    {
        public WelcomeMsgs()
        {
            API.onPlayerConnected += onPlayerConnect;
            API.onPlayerDisconnected += onPlayerDisconnect;
        }

        public void onPlayerConnect(Client player)
        {
            API.sendNotificationToAll("~b~~h~" + player.name + "~h~ ~w~joined.");
        }

        public void onPlayerDisconnect(Client player, string reason)
        {
            API.sendNotificationToAll("~b~~h~" + player.name + "~h~ ~w~quit. (" + reason + ")");
        }
    }
}
