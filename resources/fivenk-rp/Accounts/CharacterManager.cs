using System;
using GTANetworkServer;
using GTANetworkShared;
using GTANetworkServer.Constant;

namespace fivenk_rp
{
    class CharacterManager : Script
    {
        private static readonly Vector3 CharacterCreatorPos = new Vector3(3507.47f, 5122.82f, 6.22f);
        private static readonly Vector3 CharacterCreatorDirection = new Vector3(0, 0, 235.89f);

        private const string EVENT_DISPLAY_CHARACTER_SELECTOR = "display_character_selector";
        private const string EVENT_DISPLAY_CHARACTER_CREATOR = "display_character_creator";
        private const string EVENT_CREATE_CHARACTER = "create_character";
        private const string EVENT_FAIL_CREATE_CHARACTER = "fail_create_character";
        private const string EVENT_SUCCESS_CREATE_CHARACTER = "success_create_character";

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

        private void TryToCreateCharacter(Client client, string CharacterName, int GroupIndex, int SkinHash)
        {
            if (Character.DoesCharacterWithNameExist(CharacterName))
            {
                API.triggerClientEvent(client, EVENT_FAIL_CREATE_CHARACTER, "A character with this name already exists.");
                return;
            }

            Group.Type GroupType = (Group.Type)GroupIndex;
            PedHash SkinHashEnum = (PedHash)SkinHash;
            if(!Character.TryToCreateCharacter(client, CharacterName, GroupType, SkinHashEnum))
            {
                API.triggerClientEvent(client, EVENT_FAIL_CREATE_CHARACTER, "Please try again later.");
                return;
            }

            API.triggerClientEvent(client, EVENT_SUCCESS_CREATE_CHARACTER);

            Color NametagColor = Group.Colors[GroupIndex];
            API.setPlayerName(client, CharacterName);
            API.setPlayerNametag(client, CharacterName);
            API.setPlayerNametagColor(client
                , Convert.ToByte(NametagColor.red)
                , Convert.ToByte(NametagColor.green)
                , Convert.ToByte(NametagColor.blue));
            API.setEntityPosition(client, new Vector3(447.1f, -984.21f, 30.69f));
        }

        public void OnClientEventHandler(Client sender, string eventName, object[] args)
        {
            if (eventName.Equals(EVENT_CREATE_CHARACTER) && args.Length == 3)
            {
                TryToCreateCharacter(sender, Convert.ToString(args[0]), Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
            }
        }
    }
}
