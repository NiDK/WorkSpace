using System.Collections.Generic;
using PwC.C4.Sketch.Models.Enum;

namespace PwC.C4.Sketch.Models.Const
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleNames
    {
        public const string User = "User";
        public const string Editor = "Editor";
        public const string Admin = "Admin";
        public const string AppAdmin = "AppAdmin";

        public static readonly Dictionary<int, string> RoleNoDic = new Dictionary<int, string>()
        {
            {0, "User"},
            {1, "Editor"},
            {2, "Admin"},
            {3, "AppAdmin"}
        };
        public static readonly Dictionary<Roles, string> RoleEnDic = new Dictionary<Roles, string>()
        {
            {Roles.User, "User"},
            {Roles.Editor, "Editor"},
            {Roles.Admin, "Admin"},
            {Roles.AppAdmin, "AppAdmin"}
        };
        public static readonly Dictionary<string, int> RoleNaDic = new Dictionary<string, int>()
        {
            {"User", 0},
            {"Editor", 1},
            {"Admin", 2},
            {"AppAdmin", 3}
        };
    }
}
