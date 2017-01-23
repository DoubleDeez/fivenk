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

        public static bool DoesClientHavePermission(Client client, Acl AclLevel)
        {
            Player player = ClientHelper.GetPlayerFromClient(client);
            return (player != null && player.AclLevel >= AclLevel);
        }
    }
}
