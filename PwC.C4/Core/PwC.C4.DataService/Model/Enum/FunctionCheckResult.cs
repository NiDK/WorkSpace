using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.DataService.Model.Enum
{
    public enum FunctionCheckResult
    {
        NoPermissions =0,
        Permissioned =1,
        RoleNotExist=2,
        FunctionNotExist=3
    }
}
