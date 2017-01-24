using GTANetworkServer;
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
            if (ClientHelper.DoesClientHavePermission(sender, Acl.Default))
            {
                listAllCommands(sender);
                return;
            }

            API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You do not have permission to run this command!");
        }

        [Command("help", "Show a list of available commands")]
        public void helpCmd(Client sender)
        {
            if (ClientHelper.DoesClientHavePermission(sender, Acl.Default))
            {
                listAllCommands(sender);
                return;
            }

            API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You do not have permission to run this command!");
        }

        [Command("cmd", "Show a list of available commands")]
        public void cmdCmd(Client sender)
        {
            if (ClientHelper.DoesClientHavePermission(sender, Acl.Default))
            {
                listAllCommands(sender);
                return;
            }

            API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You do not have permission to run this command!");
        }

        [Command("commands", "Show a list of available commands")]
        public void commandsCmd(Client sender)
        {
            if (ClientHelper.DoesClientHavePermission(sender, Acl.Default))
            {
                listAllCommands(sender);
                return;
            }

            API.sendChatMessageToPlayer(sender, "~r~ERROR: ~w~You do not have permission to run this command!");
        }

        private void listAllCommands(Client sender)
        {
            List<string> commands = new List<string>();

            Assembly assembly = typeof(General).Assembly;
            IEnumerable<MethodInfo> commandMethods = from type in assembly.GetTypes()
                                                     from method in type.GetMethods()
                                                     where method.IsDefined(typeof(CommandAttribute))
                                                     select method;
            foreach (var method in commandMethods)
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

            API.sendChatMessageToPlayer(sender, string.Join(" ", commands));
        }
    }
}
