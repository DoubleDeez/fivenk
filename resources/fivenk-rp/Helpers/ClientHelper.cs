﻿using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    class ClientHelper
    {
        public static Player GetPlayerFromClient(Client client)
        {
            return API.shared.getEntityData(client, "Player");
        }

        public static Character GetCharacterFromClient(Client client)
        {
            if (client == null) return null;
            return API.shared.getEntityData(client, "Character");
        }

        public static Client GetClientWithPlayerName(string PlayerName)
        {
            List<Client> clients = API.shared.getAllPlayers();
            foreach (Client client in clients)
            {
                if (API.shared.getPlayerName(client).Equals(PlayerName))
                {
                    return client;
                }
            }
            return null;
        }

        public static bool IsPlayerLoggedIn(Client client)
        {
            Player player = GetPlayerFromClient(client);
            return (player != null && player.IsLoggedIn());
        }

        public static bool DoesClientHavePermission(Client client, Acl AclLevel)
        {
            Player player = GetPlayerFromClient(client);
            Acl playerAcl = player == null ? Acl.NotLoggedIn : player.AclLevel;
            return (playerAcl >= AclLevel);
        }

        public static void SavePlayer(Client client)
        {
            Player player = GetPlayerFromClient(client);
            if (player == null)
            {
                return;
            }
            player.Save();
        }

        public static void SaveCharacter(Client client)
        {
            Character character = GetCharacterFromClient(client);
            if (character == null)
            {
                return;
            }
            character.Save();
        }
    }
}
