using System.IO;
using SQLite;
using GTANetworkServer;
using System;

namespace fivenk_rp
{
    /// <summary>
    /// Database is a singleton class. Its purpose is to setup
    /// the schema for the SQLite DB and provide an instance
    /// to the SQLiteConnection member via Database.Instance().
    /// 
    /// Models can use the the SQLiteConnection instance
    /// to abstract the database away. Anything that is not a model
    /// should not use the Database class directly.
    /// 
    /// Models that need to be saved and updated should be added
    /// to the Database.CreateTables method.
    /// </summary>
    public static class Database
    {
        private const string FIVENK_DATABASE = "fnk_db.sqlite";

        private static SQLiteConnection DATABASE;

        /// <summary>
        /// Set our singleton and setup the schema
        /// </summary>
        public static void Init()
        {
            DATABASE = new SQLiteConnection(FIVENK_DATABASE);
            API.shared.consoleOutput("Database initialized!");
            CreateTables();
        }

        /// <summary>
        /// This method returns the singleton instance, only call from a model.
        /// </summary>
        /// <returns>An instance to the SQLiteConnection. You can assume this is never null.</returns>
        public static SQLiteConnection Instance()
        {
            if (DATABASE == null)
            {
                Init();
            }
            return DATABASE;
        }

        /// <summary>
        /// Initialize or update the SQLite scheme to match our models
        /// </summary>
        private static void CreateTables()
        {
            DATABASE.CreateTable<Player>();
        }

        /// <summary>
        /// Close the SQLite connection
        /// </summary>
        public static void DeInit()
        {
            if (DATABASE == null) return;
            DATABASE.Close();
        }
    }
}
