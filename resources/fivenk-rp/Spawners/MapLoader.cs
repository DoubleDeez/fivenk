using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    class MapLoader : Script
    {
        public MapLoader()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
        }

        private List<NetHandle> CreatedEntities = new List<NetHandle>();

        public void OnResourceStop()
        {
            foreach (var handle in CreatedEntities)
            {
                API.deleteEntity(handle);
            }
        }

        public void OnResourceStart()
        {
            if (!Directory.Exists("maps"))
            {
                Directory.CreateDirectory("maps");
            }
            var files = Directory.GetFiles("maps", "*.xml");
            API.consoleOutput("Loading maps...");
            foreach (var path in files)
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var ser = new XmlSerializer(typeof(Map));
                    var myMap = (Map)ser.Deserialize(stream);
                    
                    foreach (var prop in myMap.Objects)
                    {
                        if (prop.Type == ObjectTypes.Prop)
                        {
                            if (prop.Quaternion != null)
                            {
                                CreatedEntities.Add(API.createObject(prop.Hash, prop.Position, prop.Quaternion));
                            }
                            else
                            {
                                CreatedEntities.Add(API.createObject(prop.Hash, prop.Position, prop.Rotation));
                            }
                        }
                        else if (prop.Type == ObjectTypes.Vehicle)
                        {
                            CreatedEntities.Add(API.createVehicle((VehicleHash)prop.Hash, prop.Position, prop.Rotation
                                , prop.PrimaryColor, prop.SecondaryColor));
                        }
                        else if (prop.Type == ObjectTypes.Ped)
                        {
                            CreatedEntities.Add(API.createPed((PedHash)prop.Hash, prop.Position, prop.Rotation.Z));
                        }
                        else if (prop.Type == ObjectTypes.Pickup)
                        {
                            CreatedEntities.Add(API.createPickup((PickupHash)prop.Hash, prop.Position, prop.Rotation, prop.Amount
                                , prop.RespawnTimer));
                        }
                    }
                    API.consoleOutput("Loaded map!");
                }
            }

        }
    }

    public class MapObject
    {
        public ObjectTypes Type { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Quaternion Quaternion { get; set; }
        public int Hash { get; set; }
        public bool Dynamic { get; set; }

        // Ped stuff
        public string Action { get; set; }
        public string Relationship { get; set; }
        public string Weapon { get; set; }

        // Vehicle stuff
        public bool SirensActive { get; set; }
        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }

        // Pickup stuff
        public int Amount { get; set; }
        public uint RespawnTimer { get; set; }

        [XmlAttribute("Id")]
        public string Id { get; set; }
    }

    public class PedDrawables
    {
        public int[] Drawables;
        public int[] Textures;
    }

    public enum ObjectTypes
    {
        Prop,
        Vehicle,
        Ped,
        Pickup
    }

    public class Map
    {
        public List<MapObject> Objects = new List<MapObject>();
        public List<MapObject> RemoveFromWorld = new List<MapObject>();
        public List<object> Markers = new List<object>();
    }
}
