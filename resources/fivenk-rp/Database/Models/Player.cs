using GTANetworkServer;
using SQLite;
using System;

namespace fivenk_rp
{
    public class Player
    {
        /** Account Data */
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

        /** Game Data */
        public Jobs Job { get; set; }
        public int Level { get; set; }
        public int Cash { get; set; }

        /** Position Data */
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        /** Rotation Data */
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
    }
}