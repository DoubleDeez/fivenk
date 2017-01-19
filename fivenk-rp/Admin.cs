using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;

namespace fivenk_rp
{
    public class AdminScript : Script
    {
        public AdminScript()
        {
        }

        [Command(ACLRequired = true)]
        public void SetTime(Client sender, int hours, int minutes)
        {
            API.setTime(hours, minutes);
        }

        [Command(ACLRequired = true)]
        public void SetWeather(Client sender, int newWeather)
        {
            API.setWeather(newWeather);
        }

        [Command(ACLRequired = true)]
        public void Logout(Client sender)
        {
            API.logoutPlayer(sender);
        }

        [Command(ACLRequired = true)]
        public void Start(Client sender, string resource)
        {
            if (!API.doesResourceExist(resource))
            {
                API.sendChatMessageToPlayer(sender, "~r~No such resource found: \"" + resource + "\"");
            }
            else if (API.isResourceRunning(resource))
            {
                API.sendChatMessageToPlayer(sender, "~r~Resource \"" + resource + "\" is already running!");
            }
            else
            {
                API.startResource(resource);
                API.sendChatMessageToPlayer(sender, "~g~Started resource \"" + resource + "\"");
            }
        }

        [Command(ACLRequired = true)]
        public void Stop(Client sender, string resource)
        {
            if (!API.doesResourceExist(resource))
            {
                API.sendChatMessageToPlayer(sender, "~r~No such resource found: \"" + resource + "\"");
            }
            else if (!API.isResourceRunning(resource))
            {
                API.sendChatMessageToPlayer(sender, "~r~Resource \"" + resource + "\" is not running!");
            }
            else
            {
                API.stopResource(resource);
                API.sendChatMessageToPlayer(sender, "~g~Stopped resource \"" + resource + "\"");
            }
        }

        [Command(ACLRequired = true)]
        public void Restart(Client sender, string resource)
        {
            if (API.doesResourceExist(resource))
            {
                API.stopResource(resource);
                API.startResource(resource);

                API.sendChatMessageToPlayer(sender, "~g~Restarted resource \"" + resource + "\"");
            }
            else
            {
                API.sendChatMessageToPlayer(sender, "~r~No such resource found: \"" + resource + "\"");
            }
        }

        [Command(GreedyArg = true, ACLRequired = true)]
        public void Kick(Client sender, Client target, string reason)
        {
            API.kickPlayer(target, reason);
        }

        [Command(ACLRequired = true)]
        public void Kill(Client sender, Client target)
        {
            API.setPlayerHealth(target, -1);
        }

        [Command("tp", ACLRequired = true)]
        public void Teleport(Client sender, Client target)
        {
            var pos = API.getEntityPosition(sender.handle);

            API.createParticleEffectOnPosition("scr_rcbarry1", "scr_alien_teleport", pos, new Vector3(), 1f);

            API.setEntityPosition(sender.handle, API.getEntityPosition(target.handle));
        }

        [Command("tpto", ACLRequired = true)]
        public void TeleportTo(Client sender, Client target)
        {
            var pos = API.getEntityPosition(target.handle);

            API.createParticleEffectOnPosition("scr_rcbarry1", "scr_alien_teleport", pos, new Vector3(), 1f);

            API.setEntityPosition(target.handle, API.getEntityPosition(sender.handle));
        }
    }
}
