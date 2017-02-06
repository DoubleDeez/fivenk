using GTANetworkServer;
using SQLite;
using System;

namespace fivenk_rp
{
    class Character : DatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string CharacterName { get; set; }
        public int PlayerId { get; set; }
        public JobData.Id JobId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime TimeCreated { get; set; }
        public int Experience { get; set; }
        public int Cash { get; set; }
        public float SalaryMultiplier { get; set; }
        public PedHash SkinHash { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }
        public double RotationX { get; set; }
        public double RotationY { get; set; }
        public double RotationZ { get; set; }


        public static bool DoesPlayerHaveAnyCharacters(Player player)
        {
            return GetDB().Table<Character>().Where(character => (character.PlayerId == player.Id)).Count() > 0;
        }

        public static bool DoesCharacterWithNameExist(string CharName)
        {
            return GetDB().Table<Character>().Where(character => (character.CharacterName == CharName)).Count() > 0;
        }

        public static bool TryToCreateCharacter(Client client, string CharacterName, Group.Type GroupType, PedHash SkinHash)
        {
            if (DoesCharacterWithNameExist(CharacterName))
            {
                return false;
            }

            Player player = ClientHelper.GetPlayerFromClient(client);
            if (player == null)
            {
                return false;
            }

            Character character = new Character()
            {
                CharacterName = CharacterName,
                PlayerId = player.Id,
                JobId = JobData.GetFirstJobIdForGroup(GroupType),
                TimeCreated = DateTime.UtcNow,
                Cash = 1000,
                SalaryMultiplier = 1.0f,
                SkinHash = SkinHash
            };

            int CharacterID = GetDB().Insert(character);

            if (CharacterID < 0)
            {
                API.shared.consoleOutput("Failed to create character in Database");
                return false;
            }

            API.shared.setEntityData(client, "Character", character);
            return true;
        }
    }
}
