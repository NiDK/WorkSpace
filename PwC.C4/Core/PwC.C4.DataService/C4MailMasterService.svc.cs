using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Persistance;

namespace PwC.C4.DataService
{
    public class C4MailMasterService : IC4MailMasterService
    {
        public int InsertToMailQueue(MailQueueModelStream model)
        {
            return MailMasterDao.InsertToMailQueue(model);
        }

        public int InsertToMailQueueWithAttachment(MailQueueModelStream model,
            List<MailAttachment> attachments)
        {
            return MailMasterDao.InsertToMailQueueWithAttachment(model, attachments);
        }

        public int InsertToMailQueueWithAttachmentId(MailQueueModelStream model, List<int> attachmentIds)
        {
            return MailMasterDao.InsertToMailQueueWithAttachment(model, attachmentIds);
        }

        public MailMessageResult InsertToMailAttachment(MailAttachment attachment)
        {
            var m = new MailMessageResult()
            {
                ReturnId = MailMasterDao.InsertToMailAttachment(attachment)
            };
            return m;
        }



    }
}
