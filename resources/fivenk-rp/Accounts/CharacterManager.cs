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
        private const string EVENT_SELECT_CHARACTER = "select_character";
        private const string EVENT_FAIL_SELECT_CHARACTER = "fail_select_character";
        private const string EVENT_SUCCESS_SELECT_CHARACTER = "success_select_character";

        public CharacterManager()
        {
            API.onClientEventTrigger += OnClientEventHandler;
        }

        static public void SendPlayerToCharacterSelection(Client client)
        {
            Player player = ClientHelper.GetPlayerFromClient(client);
            if (player == null)
            {
                return;
            }

            API.shared.setEntityPosition(client, CharacterCreatorPos);
            API.shared.setEntityRotation(client, CharacterCreatorDirection);
            if (Character.DoesPlayerHaveAnyCharacters(player))
            {
                API.shared.triggerClientEvent(client, EVENT_DISPLAY_CHARACTER_SELECTOR
                    , Character.GetCharactersForPlayerStringified(player));
            }
            else
            {
                API.shared.triggerClientEvent(client, EVENT_DISPLAY_CHARACTER_CREATOR);
            }
        }

        private bool TryToCreateCharacter(Client client, string CharacterName, int GroupIndex, int SkinHash)
        {
            if (Character.DoesCharacterWithNameExist(CharacterName))
            {
                API.triggerClientEvent(client, EVENT_FAIL_CREATE_CHARACTER
                    , "A character with this name already exists.");
                return false;
            }

            Group.Type GroupType = (Group.Type)GroupIndex;
            PedHash SkinHashEnum = (PedHash)SkinHash;
            if (!Character.TryToCreateCharacter(client, CharacterName, GroupType, SkinHashEnum))
            {
                // TODO#48 - Log error
                API.triggerClientEvent(client, EVENT_FAIL_CREATE_CHARACTER, "Please rejoin or try again later.");
                return false;
            }

            API.triggerClientEvent(client, EVENT_SUCCESS_CREATE_CHARACTER);
            return true;
        }

        public void OnCharacterSelected(Client client, int CharacterId)
        {
            Player player = ClientHelper.GetPlayerFromClient(client);
            if (player == null)
            {
                // TODO#48 - Log error
                API.triggerClientEvent(client, EVENT_FAIL_SELECT_CHARACTER
                    , "Failed to verify account, please reconnect and try again.");
                return;
            }

            Character character = Character.GetCharacterWithId(player.Id, CharacterId);
            if (character == null)
            {
                // TODO#48 - Log error
                API.triggerClientEvent(client, EVENT_FAIL_SELECT_CHARACTER
                    , "Failed to retrieve the selected character, please rejoin and try again.");
                return;
            }

            API.shared.setEntityData(client, "Character", character);
            Job CharJob = JobData.GetJob(character.JobId);
            int GroupIndex = 0;
            if (CharJob != null)
            {
                GroupIndex = Convert.ToInt32(CharJob.JobGroup);
            }
            Color NametagColor = Group.Colors[GroupIndex];

            API.setPlayerName(client, character.CharacterName);
            API.setPlayerNametag(client, character.CharacterName);
            API.setPlayerNametagColor(client
                , Convert.ToByte(NametagColor.red)
                , Convert.ToByte(NametagColor.green)
                , Convert.ToByte(NametagColor.blue));
            API.setEntityPosition(client, character.GetPosition());
            API.setEntityRotation(client, character.GetRotation());
            API.setPlayerSkin(client, character.SkinHash);
            // Apparently this native is necessary for setting skin?
            API.sendNativeToPlayer(client, 0x45EEE61580806D63, client.handle);
            API.triggerClientEvent(client, EVENT_SUCCESS_SELECT_CHARACTER);
        }

        public void OnClientEventHandler(Client sender, string eventName, object[] args)
        {
            if (eventName.Equals(EVENT_CREATE_CHARACTER) && args.Length == 3)
            {
                TryToCreateCharacter(sender
                    , Convert.ToString(args[0])
                    , Convert.ToInt32(args[1])
                    , Convert.ToInt32(args[2]));
            }
            else if (eventName.Equals(EVENT_DISPLAY_CHARACTER_SELECTOR))
            {
                API.shared.setEntityRotation(sender, CharacterCreatorDirection);
                API.shared.triggerClientEvent(sender, EVENT_DISPLAY_CHARACTER_SELECTOR
                    , Character.GetCharactersForPlayerStringified(ClientHelper.GetPlayerFromClient(sender)));
            }
            else if (eventName.Equals(EVENT_SELECT_CHARACTER) && args.Length == 1)
            {
                OnCharacterSelected(sender, Convert.ToInt32(args[0]));
            }
        }
    }
}
