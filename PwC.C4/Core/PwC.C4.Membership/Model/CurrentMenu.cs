using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Membership.Model
{
    public class CurrentMenu
    {
        public string MenuName { get; set; }

        public string MenuUrl { get; set; }

        public Guid MenuId { get; set; }

        public int MenuOrder { get; set; }

        public Guid ParentId { get; set; }

        public List<CurrentMenu> SubMenus { get; set; } 
    }
}
