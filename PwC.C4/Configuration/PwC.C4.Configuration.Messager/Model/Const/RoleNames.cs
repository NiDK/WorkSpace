using System.Collections.Generic;

namespace PwC.C4.Configuration.Messager.Model.Const
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleNames
    {
        public const string AppAdmin = "AppAdmin";
        public const string Editor = "Editor";

        public static readonly Dictionary<int, string> RoleNoDic = new Dictionary<int, string>()
        {
            {0, "AppAdmin"},
            {1, "Editor"}
        };

        public static readonly Dictionary<string, int> RoleNaDic = new Dictionary<string, int>()
        {
            {"AppAdmin", 0},
            {"Editor", 1}
        };
    }
}
