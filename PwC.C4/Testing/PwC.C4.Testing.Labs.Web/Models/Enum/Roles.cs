using System;

namespace PwC.C4.Testing.Labs.Web.Models.Enum
{
    [Flags]
    public enum Roles
    {
        NormalUser = 0,
        PM = 1,
        Admin = 2,
        EditUser = 3,
        AppAdmin = 4
    }
}