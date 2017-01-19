using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    class Debug : Script
    {
        [Command("gun")]
        public void SpawnGunForPlayer(Client sender, WeaponHash gun, int ammo = 500)
        {
            if (CheckPlayerLoggedInWithError(sender))
            {
                API.givePlayerWeapon(sender, gun, ammo, true, true);
            }
        }

        [Command("car")]
        public void SpawnCarForPlayer(Client sender, VehicleHash model)
        {
            var rot = API.getEntityRotation(sender.handle);
            var veh = API.createVehicle(model, sender.position, new Vector3(0, 0, rot.Z), 0, 0);

            API.setPlayerIntoVehicle(sender, veh, -1);
        }

        private bool CheckPlayerLoggedInWithError(Client player)
        {
            if (!Database.IsPlayerLoggedIn(player))
            {
                API.sendChatMessageToPlayer(player, "~r~ERROR: ~w~You must be logged in to do that! (~b~/login [password]~w~)");
                return false;
            }
            return true;
        }
    }
}
