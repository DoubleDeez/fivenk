﻿using GTANetworkServer;
using GTANetworkShared;
using System;

namespace fivenk_rp
{
    public class LoginManager : Script
    {
        private const string EVENT_DISPLAY_LOGIN = "display_login";
        private const string EVENT_DISPLAY_SIGNUP = "display_signup";
        private const string EVENT_PLAYER_CONNECTED = "player_connected";
        private const string EVENT_PLAYER_LOGIN = "player_login";
        private const string EVENT_PLAYER_SIGNUP = "player_signup";
        private const string EVENT_FAIL_LOGIN = "fail_login";
        private const string EVENT_FAIL_SIGNUP = "fail_signup";
        private const string EVENT_SUCCESS_LOGIN = "success_login";
        private const string EVENT_SUCCESS_SIGNUP = "success_signup";

        public LoginManager()
        {
            API.onPlayerDisconnected += OnPlayerDisconnected;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventHandler;
        }

        public void OnPlayerConnected(Client client)
        {
            if (ClientHelper.IsPlayerLoggedIn(client))
            {
                // TODO#48 - Log an error here once we have a logging library, since a player shouldn't be logged in
                //           when they first connect.
                return;
            }
            API.freezePlayer(client, true);
            // Display login popup if no account exists, otherwise display register
            if (Player.DoesPlayerExist(client.socialClubName))
            {
                API.triggerClientEvent(client, EVENT_DISPLAY_LOGIN);
            }
            else
            {
                API.triggerClientEvent(client, EVENT_DISPLAY_SIGNUP);
            }
        }

        public void OnPlayerDisconnected(Client client, string reason)
        {
            ClientHelper.SavePlayer(client);
        }

        public void Login(Client sender, string password)
        {
            if (ClientHelper.IsPlayerLoggedIn(sender))
            {
                API.triggerClientEvent(sender, EVENT_FAIL_LOGIN, "You're already logged in.");
                return;
            }

            if (!Player.TryToLoginPlayer(sender, password))
            {
                API.triggerClientEvent(sender, EVENT_FAIL_LOGIN, "Wrong password, or account doesn't exist.");
            }
            else
            {
                API.triggerClientEvent(sender, EVENT_SUCCESS_LOGIN);
                // Unfreeze the player
                API.freezePlayer(sender, false);
                API.call("SpawnManager", "CreateSkinSelection", sender);
            }
        }

        public void Register(Client sender, string password)
        {
            if (ClientHelper.IsPlayerLoggedIn(sender))
            {
                API.triggerClientEvent(sender, EVENT_FAIL_SIGNUP, "You're already logged in.");
                return;
            }

            if (Player.DoesPlayerExist(sender.socialClubName))
            {
                API.triggerClientEvent(sender, EVENT_FAIL_SIGNUP, "An account linked to your Social Club name already exists.");
                return;
            }

            if (Player.TryToCreatePlayer(sender, password))
            {
                API.triggerClientEvent(sender, EVENT_SUCCESS_SIGNUP);
            }
            else
            {
                API.triggerClientEvent(sender, EVENT_FAIL_SIGNUP, "We failed to create your account, please try again later.");
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
        }

        public void OnClientEventHandler(Client sender, string eventName, params object[] arguments)
        {
            if (eventName.Equals(EVENT_PLAYER_LOGIN) && arguments.Length == 1)
            {
                Login(sender, arguments[0].ToString());
            }
            else if(eventName.Equals(EVENT_PLAYER_SIGNUP) && arguments.Length == 1)
            {
                Register(sender, arguments[0].ToString());
            }
            else if (eventName.Equals(EVENT_PLAYER_CONNECTED))
            {
                OnPlayerConnected(sender);
            }
        }
    }
}