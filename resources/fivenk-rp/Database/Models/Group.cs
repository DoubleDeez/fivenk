using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    class Group
    {
        public enum Type
        {
            None = -1,
            Neutral,
            TheOfficials,
            TheHicks,
            TheThugs,
            TheTriad,
            MAX
        }

        // Make sure these map to the enum above
        public static readonly string[] Names =
        {
            "Neutral",
            "The Officials",
            "The Hicks",
            "The Thugs",
            "The Triad"
        };
    }
}
