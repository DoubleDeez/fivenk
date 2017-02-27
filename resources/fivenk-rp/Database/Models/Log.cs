using System;
using SQLite;

namespace fivenk_rp
{
    class Log : DatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime TimeLogged { get; set; }
        public int PlayerId { get; set; }
        public int CharacterId { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public byte[] ExtraData { get; set; }

        public const string LOG_LOGGED_IN = "Player logged in";
        public const string LOG_REGISTERED = "Player registered account";
        public const string LOG_DISCONNECTED = "Player disconnected";
        public const string LOG_CHARACTER_SELECTED = "Character selected";
    }
}
