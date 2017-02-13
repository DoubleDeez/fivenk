using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    class CaptureMission : Script
    {
        private Blip PackageBlip = null;
        private Pickup ThePackage = null;
        private Client PackageHolder = null;

        // How often to update blip position when package is held (in milliseconds)
        private const int BLIP_UPDATE_FREQUENCY = 5 * 1000;
        // Range of time between missions (in milliseconds)
        private const int MISSION_FREQUENCY_LOW = 1 * 60 * 1000;
        private const int MISSION_FREQUENCY_HIGH = 3 * 60 * 1000;

        // Possible spawn locations for the package
        private static readonly Vector3[] SpawnLocations =
        {
            new Vector3(738.4929f, 1292.347f, 360.295f),
        };

        public CaptureMission()
        {
            API.onResourceStart += OnResourceStartHandler;
            API.onPlayerPickup += OnPlayerPickupHandler;
            API.onPlayerDisconnected += OnPlayerDisconnectedHandler;
            API.onPlayerDeath += OnPlayerDeathHandler;
        }

        [Command("rcm")]
        [Acl(Acl.Admin)]
        public void ResetCaptureMission(Client sender)
        {
            API.deleteEntity(ThePackage);
            ThePackage = null;
            API.deleteEntity(PackageBlip);
            PackageBlip = null;
            PackageHolder = null;
            QueueMission();
        }

        private void CreatePackage(Vector3 Position)
        {
            if (ThePackage != null)
            {
                // Package already exists
                return;
            }
            ThePackage = API.createPickup(PickupHash.PortablePackage, Position, new Vector3(), 1, 0);
        }

        private void CreateBlip(Vector3 Position)
        {
            PackageBlip = BlipManager.CreateBlip(Position, 501);
        }

        private void QueueMission()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int timeUntilMission = rnd.Next(MISSION_FREQUENCY_LOW, MISSION_FREQUENCY_HIGH);
            AsyncHelpers.Delay(timeUntilMission, StartMission);
            API.consoleOutput("CaptureMission: Mission queued to start in {0} seconds", timeUntilMission / 1000);
        }

        private void StartMission()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int spawnLocationIndex = rnd.Next(0, SpawnLocations.Length);
            CreatePackage(SpawnLocations[spawnLocationIndex]);
            CreateBlip(SpawnLocations[spawnLocationIndex]);
            API.sendNotificationToAll("A package has been spotted, check your map for its location");
            API.consoleOutput("CaptureMission: Mission started using spawn location index {0} at {1}"
                , spawnLocationIndex, SpawnLocations[spawnLocationIndex].ToString());
        }

        private void UpdateBlip()
        {
            if (PackageBlip == null || PackageHolder == null) return;
            API.setBlipPosition(PackageBlip, API.getEntityPosition(PackageHolder));
            UpdateBlipColor();
            StartUpdateBlipTask();
        }

        private void UpdateBlipColor()
        {
            if (PackageBlip == null || PackageHolder == null) return;
            int GroupId = (int)ClientHelper.GetGroupFromClient(PackageHolder);
            if (GroupId < 0 || GroupId  >= Group.BlipColors.Length)
            {
                API.setBlipColor(PackageBlip, 4);
            }
            else
            {
                API.setBlipColor(PackageBlip, Group.BlipColors[GroupId]);
            }
        }
        
        private void StartUpdateBlipTask()
        {
            AsyncHelpers.Delay(BLIP_UPDATE_FREQUENCY, UpdateBlip);
        }

        private void OnResourceStartHandler()
        {
            QueueMission();
        }

        private void OnPackagePickedUp()
        {
            string PlayerColor = "~w~";
            Group.Type PlayerGroup = ClientHelper.GetGroupFromClient(PackageHolder);
            if (PlayerGroup != Group.Type.None)
            {
                PlayerColor = Group.ChatColors[(int)PlayerGroup];
            }
            
            API.sendNotificationToAll(string.Format("The ~h~package~s~ has been picked up by {0}{1}"
                , PlayerColor, API.getPlayerName(PackageHolder)));
            API.sendNotificationToPlayer(PackageHolder, "You have the ~h~package!");
            API.deleteEntity(ThePackage);
            UpdateBlip();
            ThePackage = null;
        }

        private void OnPackageDropped(Vector3 Position)
        {
            API.sendNotificationToAll("The ~h~package~s~ has been dropped!");
            PackageHolder = null;
            CreatePackage(Position);
        }

        private void OnPlayerPickupHandler(Client client, NetHandle pickup)
        {
            API.consoleOutput("Pickup was picked up");
            if(pickup != ThePackage)
            {
                return;
            }
            PackageHolder = client;
            OnPackagePickedUp();
        }

        private void OnPlayerDisconnectedHandler(Client client, string reason)
        {
            if (client == PackageHolder)
            {
                OnPackageDropped(API.getEntityPosition(client));
            }
        }

        private void OnPlayerDeathHandler(Client client, NetHandle entityKiller, int weapon)
        {
            if (client == PackageHolder)
            {
                OnPackageDropped(API.getEntityPosition(client));
            }
        }
    }
}
