using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer.Constant;

namespace fivenk_rp
{
    class Group
    {
        public enum Type
        {
            None = -1,
            TheOfficials,
            TheHicks,
            TheThugs,
            TheTriad,
            MAX
        }

        // Make sure these map to the enum above
        public static readonly string[] Names =
        {
            "The Officials",
            "The Hicks",
            "The Thugs",
            "The Triad"
        };

        // Make sure these map to the enum above
        public static readonly Color[] Colors =
        {
            new Color(0, 155, 255),
            new Color(255, 72, 72),
            new Color(34, 179, 63),
            new Color(255, 206, 43)
        };
    }
}
