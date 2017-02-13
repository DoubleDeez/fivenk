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
            int GroupId = (int)Group.Type.TheOfficials;
            CreateBlip(Group.SpawnPositions[GroupId], 60, Group.BlipColors[GroupId]);
        }

        private static void CreateTheHicksBlip()
        {
            // Erlenmeyer flask
            int GroupId = (int)Group.Type.TheHicks;
            CreateBlip(Group.SpawnPositions[GroupId], 499, Group.BlipColors[GroupId]);
        }

        private static void CreateTheThugsBlip()
        {
            // Pot leaf
            int GroupId = (int)Group.Type.TheThugs;
            CreateBlip(Group.SpawnPositions[GroupId], 469, Group.BlipColors[GroupId]);
        }

        private static void CreateTheTriadBlip()
        {
            // Skull
            int GroupId = (int)Group.Type.TheTriad;
            CreateBlip(Group.SpawnPositions[GroupId], 84, Group.BlipColors[GroupId]);
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
