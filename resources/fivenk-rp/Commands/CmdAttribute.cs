using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CmdAttribute : Attribute
    {
        public CmdAttribute(string cmd)
        {
            this.cmdString = cmd;   
        }

        public CmdAttribute(string cmd, string helpText)
        {
            this.cmdString = cmd;
            this.helpText = helpText;
        }

        public readonly string helpText;
        public readonly string cmdString;
        public Acl Acl { get; set; }
    }
}
