using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer;
using GTANetworkShared;

namespace fivenk_rp
{
    public class FiveNK : Script
    {
        public FiveNK()
        {
            API.onResourceStart += OnResourceStart;
        }

        public void OnResourceStart()
        {
            API.setGamemodeName("FiveNK-RP");
        }
    }
}
