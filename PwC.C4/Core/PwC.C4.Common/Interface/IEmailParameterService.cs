using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;

namespace PwC.C4.Common.Interface
{
    public interface IEmailParameterService
    {

        List<EmailParameter> GetEmailParameters(string groupName=null);

        List<EmailParameter> GetEmailParameters(int pageIndex, int pageSize, out int totalCount, string groupName = null);

        List<EmailParameter> GetEmailParameters(string name, int pageIndex, int pageSize, out int totalCount, string groupName = null);

        EmailParameter GetEmailParameter(int paraId);

        EmailParameter GetEmailParameter(string code, string groupName = null);

        List<EmailParameter> GetEmailParameter(List<string> codes, string groupName = null);

        int UpdateEmailParameter(EmailParameter emailParameter);

        bool DeleteEmailParameter(int paraId, int modifyBy);
    }
}
