using System.IO;
using System.Data.SQLite;
using GTANetworkServer;
using System;

namespace fivenk_rp
{
    public static class Database
    {
        private const string FIVENK_DATABASE = "fnk_db.sqlite";
        private const string LOGGED_IN_KEY = "LOGGED_IN";
        private const int DEFAULT_LEVEL = 1;
        private const int DEFAULT_CASH = 1000;

        private static SQLiteConnection DATABASE;

        public static void Init()
        {
            if (!File.Exists(FIVENK_DATABASE))
            {
                SQLiteConnection.CreateFile(FIVENK_DATABASE);
                API.shared.consoleOutput("Created Database File!");
            }
            DATABASE = new SQLiteConnection("Data Source=" + FIVENK_DATABASE + ";Version=3;");
            DATABASE.Open();
            API.shared.consoleOutput("Database initialized!");
        }

        public static void DeInit()
        {
            DATABASE.Close();
        }

        public static bool DoesAccountExist(string name)
        {
            string SQL = "SELECT SocialClubName FROM players WHERE SocialClubName='"
                + name + "' LIMIT 1;";
            SQLiteCommand command = new SQLiteCommand(SQL, DATABASE);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader.Read();
        }

        public static bool IsPlayerLoggedIn(Client player)
        {
            return API.shared.getEntityData(player, LOGGED_IN_KEY) == true;
        }

        public static void CreatePlayerAccount(Client player, string password)
        {
            if (DoesAccountExist(player.socialClubName))
            {
                return;
            }

            string SQL = "INSERT INTO players (SocialClubName, Password, Level, Cash) values ("
                + "'" + player.socialClubName + "', "
                + "'" + API.shared.getHashSHA256(password) + "', "
                + "'" + Acl.Default + "', "
                + "'" + DEFAULT_LEVEL + "', "
                + "'" + DEFAULT_CASH + "');";
            SQLiteCommand command = new SQLiteCommand(SQL, DATABASE);
            command.ExecuteNonQuery();
        }

        public static bool TryLoginPlayer(Client player, string password)
        {
            string SQL = "SELECT SocialClubName FROM players WHERE SocialClubName='"
                + player.socialClubName
                + "' AND Password='" + API.shared.getHashSHA256(password) + "' LIMIT 1;";
            SQLiteCommand command = new SQLiteCommand(SQL, DATABASE);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader.Read();
        }

        public static void LoadPlayerAccount(Client player)
        {
            string SQL = "SELECT * FROM players WHERE SocialClubName='"
                   + player.socialClubName + "' LIMIT 1;";
            SQLiteCommand command = new SQLiteCommand(SQL, DATABASE);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                API.shared.setEntityData(player, "SocialClubName", reader["SocialClubName"].ToString());
                API.shared.setEntityData(player, "Acl", Convert.ToInt32(reader["ACL"]));
                API.shared.setEntityData(player, "Level", Convert.ToInt32(reader["Level"]));
// crashes                API.shared.setEntityData(player, "Job", Convert.ToInt32(reader["Job"]));
                API.shared.setEntityData(player, "Cash", Convert.ToInt32(reader["Cash"]));
                API.shared.setEntityData(player, LOGGED_IN_KEY, true);
            }
        }

        public static void SavePlayerAccount(Client player)
        {
            if (!DoesAccountExist(player.socialClubName))
            {
                return;
            }

            string SQL = "UPDATE players SET Cash='" + API.shared.getEntityData(player, "Cash")
                + "', Level='" + API.shared.getEntityData(player, "Level")
                + "', Job='" + API.shared.getEntityData(player, "Job")
                + "' WHERE SocialClubName='" + player.socialClubName + "';";
            SQLiteCommand command = new SQLiteCommand(SQL, DATABASE);
            command.ExecuteNonQuery();
        }
    }
}
