using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    class ClientHelper
    {
        public static Player GetPlayerFromClient(Client client)
        {
            return API.shared.getEntityData(client, "Player");
        }
    }
}
