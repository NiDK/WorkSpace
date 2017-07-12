using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;

namespace PwC.C4.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IC4MailMasterService
    {

        #region MailMaster

        [OperationContract]
        int InsertToMailQueue(MailQueueModelStream model);

        [OperationContract]
        int InsertToMailQueueWithAttachment(MailQueueModelStream model,List<MailAttachment> attachments);

        [OperationContract]
        int InsertToMailQueueWithAttachmentId(MailQueueModelStream model, List<int> attachmentIds);

        [OperationContract]
        MailMessageResult InsertToMailAttachment(MailAttachment attachment);

        #endregion


    }


}
