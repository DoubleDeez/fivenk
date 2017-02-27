using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    class BlipManager
    {
        public static void InitBlips()
        {
            CreateTheOfficialsBlip();
            CreateTheHicksBlip();
            CreateTheThugsBlip();
            CreateTheTriadBlip();
        }

        private static void CreateTheOfficialsBlip()
        {
            // Police badge
            CreateBlip(Group.SpawnPositions[(int)Group.Type.TheOfficials], 60, 77);
        }

        private static void CreateTheHicksBlip()
        {
            // Erlenmeyer flask
            CreateBlip(Group.SpawnPositions[(int)Group.Type.TheHicks], 499, 75);
        }

        private static void CreateTheThugsBlip()
        {
            // Pot leaf
            CreateBlip(Group.SpawnPositions[(int)Group.Type.TheThugs], 469, 25);
        }

        private static void CreateTheTriadBlip()
        {
            // Skull
            CreateBlip(Group.SpawnPositions[(int)Group.Type.TheTriad], 84, 60);
        }

        public static Blip CreateBlip(Vector3 Position, int SpriteId, int Color = 4, int Dimension = 0)
        {
            Blip blip = API.shared.createBlip(Position, Dimension);
            API.shared.setBlipSprite(blip, SpriteId);
            API.shared.setBlipColor(blip, Color);
            return blip;
        }
    }
}
