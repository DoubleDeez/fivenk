using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    class CommandHelper
    {
        public static Acl GetMethodAcl(MethodBase method)
        {
            AclAttribute aclAttribute = method.GetCustomAttribute<AclAttribute>(false);
            return aclAttribute == null ? Acl.NotLoggedIn : aclAttribute.acl;
        }

        public static void ClientDoesNotHavePermission(Client sender)
        {
                API.shared.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You do not have permission to run this command!");
        }
    }
}
