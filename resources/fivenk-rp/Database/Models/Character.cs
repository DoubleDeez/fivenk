using SQLite;
using System;

namespace fivenk_rp
{
    class Character
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
    }
}
