using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace fivenk_rp
{
    public abstract class Model
    {
        /// <summary>
        /// Wrapper for Database.Instance()
        /// </summary>
        /// <returns>An instance to the SQLiteConnection. You can assume this is never null.</returns>
        protected static SQLiteConnection GetDB()
        {
            return Database.Instance();
        }
    }

}
