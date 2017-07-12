using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Helpers;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.DataService.Persistance;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.DataService
{
    public class C4MembershipService : IC4MembershipService
    {
        public FunctionCheckResult CheckFunctionRight(FunctionCheck functionCheck)
        {
            var result = SecurityDao.CheckFunctionRight(functionCheck);
            return result;
        }
    }
}
