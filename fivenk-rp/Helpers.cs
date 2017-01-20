using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    class Helpers
    {
        /** Launches an async task after the specified amount of time */
        public static void Delay(int ms, Action action)
        {
            new Task(() =>
            {
                API.shared.sleep(ms);
                action();
            }).Start();
        }
    }
}
