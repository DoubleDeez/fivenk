using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fivenk_rp.fivenk_rp.Commands
{
    class AclAttribute : Attribute
    {
        public readonly Acl acl;

        public AclAttribute(Acl acl)
        {
            this.acl = acl;
        }
    }
}
