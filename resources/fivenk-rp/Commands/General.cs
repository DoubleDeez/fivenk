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
        [Command("h", "Show a list of available commands", Alias="help,cmd,commands")]
        public void hCmd(Client sender)
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

        [Command("whisper", Alias = "w", GreedyArg = true)]
        [Acl(Acl.Default)]
        public void WhisperPlayer(Client sender, Client target, string message)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                API.sendChatMessageToPlayer(target, "~g~" + API.getPlayerName(sender) + " whipsers: ~w~" + message);
                API.sendChatMessageToPlayer(sender, "~g~Whispering to " + API.getPlayerName(target) + ": ~w~" + message);
                API.setEntityData(target, "ReplyTo", sender);
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }

        [Command("reply", Alias = "r", GreedyArg = true)]
        [Acl(Acl.Default)]
        public void ReplyPlayer(Client sender, string message)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                Client target = API.getEntityData(sender, "ReplyTo");
                if (target == null)
                {
                    API.sendChatMessageToPlayer(sender, "~r~ERROR:~w~ You have no one to reply to.");
                    return;
                }
                API.sendChatMessageToPlayer(target, "~g~" + API.getPlayerName(sender) + " whipsers: ~w~" + message);
                API.sendChatMessageToPlayer(sender, "~g~Whispering to " + API.getPlayerName(target) + ": ~w~" + message);
                API.setEntityData(target, "ReplyTo", sender);
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
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
                Acl methodAcl = CommandHelper.GetMethodAcl(method);
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
