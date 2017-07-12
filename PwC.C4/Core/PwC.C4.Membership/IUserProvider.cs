using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ApplicationCenter.Model;
using PwC.C4.Membership.Model;
using PwC.C4.Membership.Model.Enum;

namespace PwC.C4.Membership
{
    public interface IUserProvider
    {

        string StaffName();

        string StaffId();

        string StaffCode();

        string OrganizationCode();

        bool IsMobileMode();

        CurrentUserInfo GetCurrentUser();

        List<CurrentMenu> GetCurrentMenu();

        List<string> GetCurrentRoles();

        bool CheckToken(HttpContext context, string token,string ticket);

        AuthProvider CurrentProvider();

        void Redirect2LoginPage(HttpContext context);

        void Redirect2LoginPage(HttpContextBase contextBase);
    }
}
