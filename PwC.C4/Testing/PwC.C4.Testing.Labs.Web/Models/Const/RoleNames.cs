using System.Collections.Generic;

namespace PwC.C4.Testing.Labs.Web.Models.Const
{
    public static class RoleNames
    {
        public const string NormalUser = "NormalUser";
        public const string PM = "PM";
        public const string Admin = "Admin";
        public const string EditUser = "EditUser";
        public const string AppAdmin = "AppAdmin";

        public static readonly Dictionary<int, string> RoleNoDic = new Dictionary<int, string>()
        {
            {0, "NormalUser"},
            {1, "PM"},
            {2, "Admin"},
            {3, "EditUser"},
            {4, "AppAdmin"}
        };

        public static readonly Dictionary<string, int> RoleNaDic = new Dictionary<string, int>()
        {
            {"NormalUser", 0},
            {"PM", 1},
            {"Admin", 2},
            {"EditUser", 3},
            {"AppAdmin", 4}
        };
    }
}