using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer.Constant;
using GTANetworkShared;

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

        // Make sure these map to the enum above
        public static readonly Vector3[] SpawnPositions =
        {
            // TODO: Make these not all the police station
            new Vector3(447.1f, -984.21f, 30.69f),
            new Vector3(447.1f, -984.21f, 30.69f),
            new Vector3(447.1f, -984.21f, 30.69f),
            new Vector3(447.1f, -984.21f, 30.69f)
        };
    }
}
