using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;
using System.Reflection;

namespace fivenk_rp
{
    class Debug : Script
    {
        [Command("gun")]
        [Acl(Acl.Default)]
        public void SpawnGunForPlayer(Client sender, WeaponHash gun, int ammo = 500)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                if (CheckPlayerLoggedInWithError(sender))
                {
                    API.givePlayerWeapon(sender, gun, ammo, true, true);
                }
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("car")]
        [Acl(Acl.Default)]
        public void SpawnCarForPlayer(Client sender, VehicleHash model)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                if (CheckPlayerLoggedInWithError(sender))
                {
                    var rot = API.getEntityRotation(sender.handle);
                    var veh = API.createVehicle(model, sender.position, new Vector3(0, 0, rot.Z), 0, 0);

                    API.setPlayerIntoVehicle(sender, veh, -1);
                }
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("skin")]
        [Acl(Acl.Admin)]
        public void ChangeSkinCommand(Client sender, PedHash model)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                if (CheckPlayerLoggedInWithError(sender))
                {
                    API.setPlayerSkin(sender, model);
                    API.sendNativeToPlayer(sender, 0x45EEE61580806D63, sender.handle);
                }
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("where")]
        [Acl(Acl.Default)]
        public void WhereAmI(Client sender)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                if (CheckPlayerLoggedInWithError(sender))
                {
                    Vector3 Coords = API.getEntityPosition(sender.handle);
                    if (Coords != null)
                    {
                        API.sendChatMessageToPlayer(sender, Coords.ToString());
                    }
                }
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        private bool CheckPlayerLoggedInWithError(Client player)
        {
            if (!ClientHelper.IsPlayerLoggedIn(player))
            {
                API.sendChatMessageToPlayer(player, "~r~ERROR: ~w~You must be logged in to do that! (~b~/login [password]~w~)");
                return false;
            }
            return true;
        }
    }
}
