using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Membership.Model
{
    public class CurrentRole
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleType { get; set; }
    }
}
