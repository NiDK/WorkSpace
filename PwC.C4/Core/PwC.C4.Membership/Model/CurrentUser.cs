using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Membership.Model
{
    public class CurrentUserInfo
    {
        public string StaffName { get; set; }
        public string StaffId { get; set; }
        public string StaffCode { get; set; }
        public string TenantId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> Roles { get; set; }
        public KeyValuePair<string, string>[] RolesKv { get; set; }
        public List<string> Groups { get; set; }
        public KeyValuePair<string, string>[] GroupsKv { get; set; }
        public List<CurrentMenu> Menus { get; set; } 

    }
}
