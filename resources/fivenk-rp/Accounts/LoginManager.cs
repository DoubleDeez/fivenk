using GTANetworkServer;
using GTANetworkShared;
using System;

namespace fivenk_rp
{
    public class LoginManager : Script
    {
        public LoginManager()
        {
            API.onPlayerConnected += OnPlayerConnected;
            API.onPlayerDisconnected += OnPlayerDisconnected;
            API.onResourceStop += OnResourceStop;
        }

        public void OnPlayerConnected(Client player)
        {
            API.freezePlayer(player, true);
            API.sendChatMessageToPlayer(player, "You are unable to move until you ~b~/register [password]~w~ and ~b~/login [password]~w~");
        }

        public void OnPlayerDisconnected(Client client, string reason)
        {
            ClientHelper.SavePlayer(client);
        }

        public void Login(Client sender, string password)
        {
            if (ClientHelper.IsPlayerLoggedIn(sender))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
                return;
            }

            if (!Player.TryToLoginPlayer(sender, password))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR:~w~ Wrong password, or account doesn't exist!");
            }
            else
            {
                API.sendChatMessageToPlayer(sender, "~g~Logged in successfully! ~w~Use your ~b~Arrow Keys~w~ to select a job");
                // Unfreeze the player
                API.freezePlayer(sender, false);
                API.call("SpawnManager", "CreateSkinSelection", sender);
            }
        }

        public void Register(Client sender, string password)
        {
            if (ClientHelper.IsPlayerLoggedIn(sender))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
                return;
            }

            if (Player.DoesPlayerExist(sender.socialClubName))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~An account linked to your Social Club name already exists!");
                return;
            }

            if (Player.TryToCreatePlayer(sender, password))
            {
                API.sendChatMessageToPlayer(sender, "~g~Account created! ~w~Now log in with ~y~/login [password]");
            }
            else
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~We failed to create your account, please try again later.");
            }
        }

        public void OnResourceStop()
        {
            foreach (var client in API.getAllPlayers())
            {
                foreach (var data in API.getAllEntityData(client))
                {
                    API.resetEntityData(client, data);
                }
            }
            Database.DeInit();
        }
    }
}