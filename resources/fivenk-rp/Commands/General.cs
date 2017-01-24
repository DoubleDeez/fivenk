﻿using GTANetworkServer;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    public class General : Script
    {
        [Command("h", "Show a list of available commands")]
        public void hCmd(Client sender)
        {
            listAllCommands(sender);
        }

        [Command("help", "Show a list of available commands")]
        public void helpCmd(Client sender)
        {
            listAllCommands(sender);
        }

        [Command("cmd", "Show a list of available commands")]
        public void cmdCmd(Client sender)
        {
            listAllCommands(sender);
        }

        [Command("commands", "Show a list of available commands")]
        public void commandsCmd(Client sender)
        {
            listAllCommands(sender);
        }

        
        [Command(SensitiveInfo = true)]
        public void Login(Client sender, string password)
        {
            API.call("LoginManager", "Login", sender, password);
        }

        [Command(SensitiveInfo = true)]
        public void Register(Client sender, string password)
        {
            API.call("LoginManager", "Register", sender, password);
        }

        private void listAllCommands(Client sender)
        {
            List<string> commands = new List<string>();
            //Player player = API.shared.getEntityData(sender, "Player");
            //Acl senderAcl = player == null ? Acl.NotLoggedIn : player.AclLevel;

            Assembly assembly = typeof(General).Assembly;
            IEnumerable<MethodInfo> commandMethods = from type in assembly.GetTypes()
                                                     from method in type.GetMethods()
                                                     where method.IsDefined(typeof(CommandAttribute))
                                                     select method;
            foreach (var method in commandMethods)
            {
                Acl methodAcl = CommandHelper.GetMethodAcl(method);
                //AclAttribute aclAttribute = method.GetCustomAttribute<AclAttribute>(false);
                //Acl methodAcl = aclAttribute == null ? Acl.NotLoggedIn : aclAttribute.acl;
                if(ClientHelper.DoesClientHavePermission(sender, methodAcl))
                {
                    CommandAttribute commandAttribute = method.GetCustomAttribute<CommandAttribute>(false);
                    if (commandAttribute.CommandString == null)
                    {
                        commands.Add("/" + method.Name.ToLower());
                    }
                    else
                    {
                        commands.Add("/" + commandAttribute.CommandString);
                    }
                }
            }

            API.sendChatMessageToPlayer(sender, string.Join(" ", commands));
        }
    }
}
