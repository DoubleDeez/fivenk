using System.IO;
using SQLite;
using GTANetworkServer;
using System;

namespace fivenk_rp
{
    public static class Database
    {
        private const string FIVENK_DATABASE = "fnk_db.sqlite";
        private const int DEFAULT_LEVEL = 1;
        private const int DEFAULT_CASH = 1000;

        private static SQLiteConnection DATABASE;

        public static void Init()
        {
            DATABASE = new SQLiteConnection(FIVENK_DATABASE);
            API.shared.consoleOutput("Database initialized!");
            CreateTables();
        }

        private static void CreateTables()
        {
            DATABASE.CreateTable<Player>();
        }

        public static void DeInit()
        {
            DATABASE.Close();
        }

        public static bool DoesAccountExist(string name)
        {
            return DATABASE.Table<Player>().Where(p => (p.SocialClubName == name)).Count() > 0;
        }

        public static bool IsPlayerLoggedIn(Client player)
        {
            return API.shared.getEntityData(player, "Player").AclLevel != Acl.NotLoggedIn;
        }

        public static bool CreatePlayerAccount(Client player, string password, string salt)
        {
            if (DoesAccountExist(player.socialClubName))
            {
                return false;
            }

            int PlayerID = DATABASE.Insert(new Player()
            {
                SocialClubName = player.socialClubName,
                Salt = salt,
                Password = API.shared.getHashSHA256(password + salt),
                AclLevel = Acl.Default,
                Level = DEFAULT_LEVEL,
                Cash = DEFAULT_CASH,
                TimeRegistered = DateTime.UtcNow
            });

            // If playerID is below 0, it failed to create the player
            if (PlayerID < 0)
            {
                API.shared.consoleOutput("Failed to create player in Database");
                return false;
            }
            return true;
        }

        public static bool TryLoginPlayer(Client player, string password)
        {
            Player PlayerFromDB = DATABASE.Table<Player>().Where(p => (p.SocialClubName == player.socialClubName)).First();
            if (PlayerFromDB == null)
            {
                return false;
            }

            string HashedInputPassword = API.shared.getHashSHA256(password + PlayerFromDB.Salt);
            if (!PlayerFromDB.Password.Equals(HashedInputPassword))
            {
                return false;
            }
            PlayerFromDB.TimeLastLoggedIn = DateTime.UtcNow;
            API.shared.setEntityData(player, "Player", PlayerFromDB);
            return true;
        }

        public static void SavePlayerAccount(Client player)
        {
            Player p = API.shared.getEntityData(player, "Player");
            if (p == null)
            {
                return;
            }
            DATABASE.Update(p);
        }
    }
}
