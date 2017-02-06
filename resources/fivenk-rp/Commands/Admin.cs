using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkServer;
using GTANetworkShared;
using System.Threading;
using System.Reflection;

namespace fivenk_rp
{
    public class AdminScript : Script
    {
        public AdminScript()
        {
        }

        [Command()]
        [Acl(Acl.Admin)]
        public void SetTime(Client sender, int hours, int minutes)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                API.setTime(hours, minutes);
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command()]
        [Acl(Acl.Admin)]
        public void SetWeather(Client sender, int newWeather)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                API.setWeather(newWeather);
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command(GreedyArg = true)]
        [Acl(Acl.Moderator)]
        public void Kick(Client sender, Client target, string reason)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                API.kickPlayer(target, reason);
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command()]
        [Acl(Acl.Moderator)]
        public void Kill(Client sender, Client target)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                API.setPlayerHealth(target, -1);
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("tp")]
        [Acl(Acl.Moderator)]
        public void TeleportTo(Client sender, Client target)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                var pos = API.getEntityPosition(sender.handle);
                API.createParticleEffectOnPosition("scr_rcbarry1", "scr_alien_teleport", pos, new Vector3(), 1f);
                API.setEntityPosition(sender.handle, API.getEntityPosition(target.handle));
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("tpp")]
        [Acl(Acl.Moderator)]
        public void TeleportPlayerToYou(Client sender, Client target)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                var pos = API.getEntityPosition(target.handle);
                API.createParticleEffectOnPosition("scr_rcbarry1", "scr_alien_teleport", pos, new Vector3(), 1f);
                API.setEntityPosition(target.handle, API.getEntityPosition(sender.handle));
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("god")]
        [Acl(Acl.Admin)]
        public void ToggleGodMode(Client sender)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                bool isEnabled = !API.getEntityInvincible(sender);
                API.setEntityInvincible(sender, isEnabled);
                API.sendNotificationToPlayer(sender, String.Format("God mode {0}", isEnabled ? "Enabled" : "Disabled"));
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("tpxyz", "~y~Teleport to specific coordinates.\n~y~Usage: ~w~/tpxyz [x] [y] [z]")]
        [Acl(Acl.Moderator)]
        public void TeleportToXYZ(Client sender, double x, double y, double z)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                var pos = API.getEntityPosition(sender.handle);
                API.createParticleEffectOnPosition("scr_rcbarry1", "scr_alien_teleport", pos, new Vector3(), 1f);
                API.setEntityPosition(sender.handle, new Vector3(x, y, z));
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("tppxyz", "~y~Teleport a player to specific coordinates.\n~y~Usage: ~w~/tppxyz [player] [x] [y] [z]")]
        [Acl(Acl.Moderator)]
        public void TeleportPlayerToXYZ(Client sender, Client target, double x, double y, double z)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                var pos = API.getEntityPosition(target.handle);
                API.createParticleEffectOnPosition("scr_rcbarry1", "scr_alien_teleport", pos, new Vector3(), 1f);
                API.setEntityPosition(target.handle, new Vector3(x, y, z));
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }
    }
}
