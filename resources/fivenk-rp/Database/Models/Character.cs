using GTANetworkServer;
using GTANetworkShared;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Vector3 GetPosition()
        {
            return new Vector3(PositionX, PositionY, PositionZ);
        }

        public Vector3 GetRotation()
        {
            return new Vector3(RotationX, RotationY, RotationZ);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(2048);
            sb.AppendLine("{");
            sb.Append("\"Id\": ").Append(PlayerId).AppendLine(",");
            sb.Append("\"Job\": \"").Append(JobData.GetJobTitle(JobId)).AppendLine("\",");
            sb.Append("\"Name\": \"").Append(CharacterName).AppendLine("\",");
            sb.Append("\"Cash\": ").Append(Cash).AppendLine(",");
            sb.Append("\"SkinHash\": ").Append(Convert.ToInt32(SkinHash)).AppendLine("");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static bool DoesPlayerHaveAnyCharacters(Player player)
        {
            return GetDB().Table<Character>()
                .Where(character => (character.PlayerId == player.Id && character.IsDeleted == false)).Count() > 0;
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
            
            Vector3 SpawnPosition = Group.SpawnPositions[Convert.ToInt32(GroupType)];

            Character character = new Character()
            {
                CharacterName = CharacterName,
                PlayerId = player.Id,
                JobId = JobData.GetFirstJobIdForGroup(GroupType),
                TimeCreated = DateTime.UtcNow,
                Cash = 1000,
                SalaryMultiplier = 1.0f,
                SkinHash = SkinHash,
                PositionX = SpawnPosition.X,
                PositionY = SpawnPosition.Y,
                PositionZ = SpawnPosition.Z,
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

        public static List<Character> GetCharactersForPlayer(Player player)
        {
            if (player == null) return new List<Character>();

            TableQuery<Character> Characters = GetDB().Table<Character>()
                .Where(character => (character.PlayerId == player.Id && character.IsDeleted == false));
            return Database.ConvertQueryToList(Characters);
        }

        public static string GetCharactersForPlayerStringified(Player player)
        {
            List<Character> Characters = GetCharactersForPlayer(player);
            StringBuilder sb = new StringBuilder(2048 * Characters.Count + 32);
            sb.AppendLine("[");
            bool IsFirstCharacter = true;
            foreach(Character c in Characters)
            {
                if (!IsFirstCharacter)
                {
                    sb.AppendLine(",");
                }
                sb.Append(c.ToString());
                IsFirstCharacter = false;
            }
            sb.AppendLine("]");
            return sb.ToString();
        }
    }
}
