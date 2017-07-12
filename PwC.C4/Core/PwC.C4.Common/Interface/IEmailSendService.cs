using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;

namespace PwC.C4.Common.Interface
{
    public interface IEmailSendService
    {
        bool SendToMailQueue(MailQueueModel model);
    }
}
