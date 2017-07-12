using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;


namespace PwC.C4.Common.Interface
{
    internal interface IEmailTemplateService
    {
        List<EmailTemplate> GetEmailTemplates(string groupName = null);

        List<EmailTemplate> GetEmailTemplates(int pageIndex, int pageSize, out int totalCount, string groupName = null);

        List<EmailTemplate> GetEmailTemplates( string name, int pageIndex, int pageSize, out int totalCount, string groupName = null);

        EmailTemplate GetEmailTemplate(int paraId);

        EmailTemplate GetEmailTemplate(string code, string groupName = null);

        List<EmailTemplate> GetEmailTemplate(List<string> codes, string groupName = null);

        int UpdateEmailTemplate(EmailTemplate emailParameter);

        bool DeleteEmailTemplate(int paraId, int modifyBy);
    }
}
