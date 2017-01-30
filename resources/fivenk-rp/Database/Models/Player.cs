using GTANetworkServer;
using SQLite;
using System;
using System.Security.Cryptography;

namespace fivenk_rp
{
    public class Player : Model
    {
        #region Class Constants
        private const Acl DEFAULT_ACL = Acl.Default;
        #endregion
        
        #region Account Data
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string SocialClubName { get; set; }
        [MaxLength(255)]
        public string Salt { get; set; }
        public string Password { get; set; }
        public DateTime TimeRegistered { get; set; }
        public DateTime TimeLastLoggedIn { get; set; }
        public Acl AclLevel { get; set; }
        #endregion

        #region Instance Methods
        public bool IsLoggedIn()
        {
            return AclLevel != Acl.NotLoggedIn;
        }

        public void Save()
        {
            GetDB().Update(this);
        }
        #endregion

        #region Static Methods
        public static bool DoesPlayerExist(string PlayerName)
        {
            return GetDB().Table<Player>().Where(player => (player.SocialClubName == PlayerName)).Count() > 0;
        }

        public static bool TryToCreatePlayer(Client player, string password)
        {
            if (DoesPlayerExist(player.socialClubName))
            {
                return false;
            }

            // Generate a salt for the password
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[255];
            rng.GetBytes(buffer);
            string salt = BitConverter.ToString(buffer);
            
            int PlayerID = GetDB().Insert(new Player()
            {
                SocialClubName = player.socialClubName,
                Salt = salt,
                Password = API.shared.getHashSHA256(password + salt),
                AclLevel = DEFAULT_ACL,
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

        public static bool TryToLoginPlayer(Client client, string password)
        {
            if (!DoesPlayerExist(client.socialClubName))
            {
                return false;
            }

            Player PlayerFromDB = GetDB().Table<Player>().Where(p => (p.SocialClubName == client.socialClubName)).First();
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
            API.shared.setEntityData(client, "Player", PlayerFromDB);
            return true;
        }
        #endregion
    }
}