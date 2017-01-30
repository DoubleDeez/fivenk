using GTANetworkServer;
using GTANetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    public static class Apartment
    {
        public static void onPlayerConnected(Client sender)
        {
            // load player's apartments + markers upon connecting
            // TODO: iterate through player's data to see what apartment's they own
            // TODO: fix player rotation when entering/exiting apartment
            // TODO: add a short 1-2 sec loading screen or something when player teleports to apartment

            // Adding marker outside of police station for testing
            API.shared.createMarker(1, new Vector3(424.7259, -979.2656, 29.69717), new Vector3(), new Vector3(), new Vector3(1, 1, 1), 100, 255, 255, 0, 0);
            var pol_colShape = API.shared.createCylinderColShape(new Vector3(424.7259, -979.2656, 29.69717), 1, 2);
            pol_colShape.onEntityEnterColShape += pol_enterColShape;

            // Adding marker in apartment
            API.shared.createMarker(1, new Vector3(-786.8663, 315.7642, 216.6250), new Vector3(), new Vector3(), new Vector3(1, 1, 1), 100, 255, 255, 0, 1);
            var apt_colShape = API.shared.createCylinderColShape(new Vector3(-786.8663, 315.7642, 216.6250), 1, 2);
            apt_colShape.onEntityEnterColShape += apt_enterColShape;
        }

        public static void pol_enterColShape(ColShape shape, NetHandle entity)
        {
            Client sender = API.shared.getPlayerFromHandle(entity);
            if (sender == null)
            {
                return;
            }
            API.shared.setEntityDimension(sender, 1);
            API.shared.setEntityPosition(sender.handle, new GTANetworkShared.Vector3(-785.6118, 315.6014, 217.6385));
        }

        public static void apt_enterColShape(ColShape shape, NetHandle entity)
        {
            Client sender = API.shared.getPlayerFromHandle(entity);
            if (sender == null)
            {
                return;
            }
            API.shared.setEntityDimension(sender, 0);
            API.shared.setEntityPosition(sender.handle, new GTANetworkShared.Vector3(426.7259, -979.2656, 30.6852));
        }
    }
}
