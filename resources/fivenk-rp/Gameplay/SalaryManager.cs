using GTANetworkServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    class SalaryManager : Script
    {
        // How often to payout salary in seconds
        private static int SalaryInterval = 300;

        public SalaryManager()
        {
            API.onResourceStart += OnResourceStartHandler;
        }

        public void OnResourceStartHandler()
        {
            StartPayoutTask();
        }

        public void StartPayoutTask()
        {
            AsyncHelpers.Delay(SalaryInterval * 1000, () =>
            {
                PayoutSalary();
            });
        }

        public void PayoutSalary()
        {
            List<Client> clients = API.getAllPlayers();
            foreach(Client client in clients)
            {
                Character c = ClientHelper.GetCharacterFromClient(client);
                if (c == null) continue;
                int PayAmount = c.PaySalary();
                API.sendChatMessageToPlayer(client, string.Format("~g~PAYDAY!~w~ You got paid ~g~${0}", PayAmount));
            }
            API.consoleOutput(string.Format("Paid salary to {0} client(s)!", clients.Count));
            StartPayoutTask();
        }

        [Command("salaryinterval", Alias = "si,salarytime,st")]
        [Acl(Acl.Admin)]
        public void SetSalaryInterval(Client sender, int interval)
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            Acl methodAcl = CommandHelper.GetMethodAcl(method);

            if (ClientHelper.DoesClientHavePermission(sender, methodAcl))
            {
                SalaryInterval = interval;
                return;
            }

            CommandHelper.ClientDoesNotHavePermission(sender);
        }
    }
}
