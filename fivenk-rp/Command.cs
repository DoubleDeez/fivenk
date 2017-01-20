using System;
using GTANetworkServer;
using GTANetworkShared;
using System.Collections.Generic;

namespace fivenk_rp
{
    public class Command
    {
        public Command(string cmdString, string helpText="", Acl acl=Acl.Default)
        {
            this.cmdString = cmdString;
            this.helpText = helpText;
            this.acl = acl;
        }

        public string cmdString { get; set; }
        public string helpText { get; set; }
        public Acl acl { get; set; }
    }


    public class Commands : Script
    {
        public Commands()
        {
            API.onChatCommand += onChatCommandHandler;
            API.onResourceStart += onResourceStartHandler;
            Dictionary<string, Command> cmdList = new Dictionary<string, Command>();
        }

        private void onResourceStartHandler()
        {
            // build up the list of commands
            foreach (var property in typeof(Commands).GetProperties())
            {
                object[] cmdArray = property.GetCustomAttributes(typeof(CmdAttribute), false);
                if (cmdArray.Length > 0)
                {
                    CmdAttribute cmdAttribute = cmdArray[0] as CmdAttribute;
                    Command cmd = new Command(cmdAttribute)   
                }
            }
        }

        private void onChatCommandHandler(Client sender, string command, CancelEventArgs cancel)
        {
            int senderAcl = Convert.ToInt32(API.shared.getEntityData(sender, "Acl"));

            Attribute.GetCustomAttribute()
        }

        [Cmd("test", "test help", Acl=Acl.Default)]
        private void testCmd(Client sender)
        {
            
        }
    }
}
