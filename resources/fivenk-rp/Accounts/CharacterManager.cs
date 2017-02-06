using System;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    class CharacterManager : Script
    {
        private static readonly Vector3 CharacterCreatorPos = new Vector3(3507.47f, 5122.82f, 6.22f);
        private static readonly Vector3 CharacterCreatorDirection = new Vector3(0, 0, 235.89f);

        private const string EVENT_DISPLAY_CHARACTER_SELECTOR = "display_character_selector";
        private const string EVENT_DISPLAY_CHARACTER_CREATOR = "display_character_creator";

        public CharacterManager()
        {
            API.onClientEventTrigger += OnClientEventHandler;
        }

        static public void SendPlayerToCharacterSelection(Client client)
        {
            Player player = ClientHelper.GetPlayerFromClient(client);
            if(player == null)
            {
                return;
            }

            API.shared.setEntityPosition(client, CharacterCreatorPos);
            API.shared.setEntityRotation(client, CharacterCreatorDirection);
            if (Character.DoesPlayerHaveAnyCharacters(player))
            {
                API.shared.triggerClientEvent(client, EVENT_DISPLAY_CHARACTER_SELECTOR);
            }
            else
            {
                API.shared.triggerClientEvent(client, EVENT_DISPLAY_CHARACTER_CREATOR);
            }
        }

        public void OnClientEventHandler(Client sender, string eventName, object[] args)
        {

        }
    }
}
