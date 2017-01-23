using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Security.Cryptography;

namespace fivenk_rp
{
    public class LoginManager : Script
    {
        public LoginManager()
        {
            Database.Init();

            API.onPlayerConnected += OnPlayerConnected;
            API.onPlayerDisconnected += OnPlayerDisconnected;
            API.onResourceStop += OnResourceStop;
        }

        public void OnPlayerConnected(Client player)
        {
            API.freezePlayer(player, true);
            API.sendChatMessageToPlayer(player, "You are unable to move until you ~b~/register [password]~w~ and ~b~/login [password]~w~");
        }

        public void OnPlayerDisconnected(Client player, string reason)
        {
            Database.SavePlayerAccount(player);
            if (API.isAclEnabled())
            {
                API.logoutPlayer(player);
            }
        }

        [Command(SensitiveInfo = true)]
        public void Login(Client sender, string password)
        {
            if (Database.IsPlayerLoggedIn(sender))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
                return;
            }

            if (API.isAclEnabled())
            {
                var logResult = API.loginPlayer(sender, password);
                switch (logResult)
                {
                    case 0:
                    case 4:
                    case 5:
                    default:
                        break;
                    case 3:
                    case 1:
                        API.sendChatMessageToPlayer(sender, "~g~Login successful!~w~ Logged in as ~b~" + API.getPlayerAclGroup(sender) + "~w~.");
                        break;
                    case 2:
                        API.sendChatMessageToPlayer(sender, "~r~ERROR:~w~ Wrong password!");
                        break;
                }
            }

            if (!Database.TryLoginPlayer(sender, password))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR:~w~ Wrong password, or account doesn't exist!");
            }
            else
            {
                API.sendChatMessageToPlayer(sender, "~g~Logged in successfully! ~w~Use your ~b~Arrow Keys~w~ to select a job");
                // Unfreeze the player
                API.freezePlayer(sender, false);
                API.call("SpawnManager", "CreateSkinSelection", sender);

                Player senderPlayer = ClientHelper.GetPlayerFromClient(sender);
                if (senderPlayer != null)
                {
                    int cash = senderPlayer.Cash;
                    API.triggerClientEvent(sender, "update_cash_display", cash);
                }
            }
        }

        [Command(SensitiveInfo = true)]
        public void Register(Client sender, string password)
        {
            if (Database.IsPlayerLoggedIn(sender))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You're already logged in!");
                return;
            }

            if (Database.DoesAccountExist(sender.socialClubName))
            {
                API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~An account linked to your Social Club name already exists!");
                return;
            }

            // Generate a salt for the password
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[255];
            rng.GetBytes(buffer);

            if(Database.CreatePlayerAccount(sender, password, BitConverter.ToString(buffer)))
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