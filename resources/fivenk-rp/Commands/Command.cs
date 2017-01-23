using System;
using GTANetworkServer;
using GTANetworkShared;
using System.Collections.Generic;
using System.Reflection;

namespace fivenk_rp
{
    public class Command
    {
        public Command(string cmdString, MethodInfo handler, string helpText="", Acl acl=Acl.Default)
        {
            this.cmdString = cmdString;
            this.handler = handler;
            this.helpText = helpText;
            this.acl = acl;
        }

        public string cmdString { get; set; }
        public MethodInfo handler { get; set; }
        public string helpText { get; set; }
        public Acl acl { get; set; }
    }


    public class Commands : Script
    {
        private Dictionary<string, Command> cmdList;

        public Commands()
        {
            API.onChatCommand += onChatCommandHandler;
            API.onResourceStart += onResourceStartHandler;
            this.cmdList = new Dictionary<string, Command>();
        }

        private void onResourceStartHandler()
        {
            // build up the list of commands
            foreach (var method in typeof(Commands).GetMethods())
            {
                object[] cmdArray = method.GetCustomAttributes(typeof(CmdAttribute), false);
                if (cmdArray.Length > 0)
                {
                    CmdAttribute cmdAttribute = cmdArray[0] as CmdAttribute;
                    Command cmd = new Command(cmdAttribute.cmdString, method, helpText: cmdAttribute.helpText, acl: cmdAttribute.Acl);
                    this.cmdList.Add(cmdAttribute.cmdString, cmd);
                }
            }
        }

        private void onChatCommandHandler(Client sender, string cmdString, CancelEventArgs cancel)
        {
            Player player = ClientHelper.GetPlayerFromClient(sender);
            if (player != null && this.cmdList.ContainsKey(cmdString))
            {
                Command cmd = this.cmdList[cmdString];
                if (player.AclLevel >= cmd.acl)
                {
                    cmd.handler.Invoke(this, new object[] { sender });
                }
            }
        }

        [Cmd("test", "test help", Acl=Acl.Default)]
        private void testCmd(Client sender)
        {

        }
    }
}
