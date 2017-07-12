using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;

namespace PwC.C4.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IC4CommonService
    {
        #region EmailDao

        [OperationContract]
        int EmailParameter_Create(EmailParameter model);

        [OperationContract]
        EmailParameter EmailParameter_Get(string appcode, int paraId);

        [OperationContract]
        List<EmailParameter> EmailParameters_GetByGroup(string appcode, string groupName = null);

        [OperationContract]
        List<EmailParameter> EmailParameters_GetByPaging(string appcode, string name, int pageIndex, int pageSize, out int totalCount,
            string groupName = null);

        [OperationContract]
        List<EmailParameter> EmailParameters_GetByCodes(string appcode, List<string> codes, string groupName = null);

        [OperationContract]
        int EmailParameter_Update(EmailParameter emailParameter);

        [OperationContract]
        bool EmailParameter_Delete(string appcode, int paraId, int modifyBy);

        [OperationContract]
        int EmailTemplate_Create(EmailTemplate model);

        [OperationContract]
        EmailTemplate EmailTemplate_Get(string appcode, int tId);

        [OperationContract]
        List<EmailTemplate> EmailTemplates_GetByGroup(string appcode, string groupName = null);

        [OperationContract]
        List<EmailTemplate> EmailTemplates_GetByPaging(string appcode, string name, int pageIndex, int pageSize, out int totalCount,
            string groupName = null);

        [OperationContract]
        List<EmailTemplate> EmailTemplates_GetByCodes(string appcode, List<string> codes, string groupName = null);

        [OperationContract]
        int EmailTemplate_Update(EmailTemplate emailTemplate);

        [OperationContract]
        bool EmailTemplate_Delete(string appcode, int tId, int modifyBy);


        #endregion


        #region MailMaster

        [OperationContract]
        int InsertToMailQueue(MailQueueModel model);
        [OperationContract]
        int InsertToMailQueueWithAttachment(MailQueueModel model, List<MailAttachment> attachments);

        [OperationContract]
        int InsertToMailQueueWithAttachmentId(MailQueueModel model, List<int> attachmentIds);

        [OperationContract]
        int InsertToMailAttachment(MailAttachment attachment);


        #endregion


        #region Workflow

        [OperationContract]
        WorkflowBase WorkFlow_GetByFormId(string appcode, string entityName, int formId);

        [OperationContract]
        WorkflowBase WorkFlow_GetByRecordId(string appcode, string entityName, string recordId);

        [OperationContract]
        WorkflowBase WorkFlow_GetLastUnactivedByInstanceId(string appcode, string workflowCode, int instanceId);

        [OperationContract]
        WorkflowBase WorkFlow_GetLastUnactivedByRecordId(string appcode, string entityName, string recordId);

        [OperationContract]
        WorkflowBase WorkFlow_GetActiveByInstanceId(string appcode, int instanceId);

        [OperationContract]
        int Workflow_Insert(WorkflowBase model);

        #endregion

        #region StaffBank


        [OperationContract]
        StaffInfo Staff_Get(string staffId);

        [OperationContract]
        List<StaffInfo> Staff_GetList(string where);

        #endregion

    }


}
