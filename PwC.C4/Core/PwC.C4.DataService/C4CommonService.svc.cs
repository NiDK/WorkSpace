using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Persistance;

namespace PwC.C4.DataService
{
    public class C4CommonService : IC4CommonService
    {
        public int EmailParameter_Create(EmailParameter model)
        {
            return EmailDao.CreateParameter(model);
        }

        public EmailParameter EmailParameter_Get(string appcode,int paraId)
        {
            return EmailDao.GetParameter(appcode, paraId);
        }

        public List<EmailParameter> EmailParameters_GetByGroup(string appcode, string groupName = null)
        {
            return EmailDao.GetEmailParameters(appcode, groupName);
        }

        public List<EmailParameter> EmailParameters_GetByPaging(string appcode, string name, int pageIndex, int pageSize, out int totalCount, string groupName = null)
        {
            return EmailDao.GetEmailParameters(appcode, name, pageIndex, pageSize, out totalCount, groupName);
        }

        public List<EmailParameter> EmailParameters_GetByCodes(string appcode, List<string> codes, string groupName = null)
        {
            return EmailDao.GetParameters(appcode, codes, groupName);
        }

        public int EmailParameter_Update(EmailParameter emailParameter)
        {
            return EmailDao.UpdateParameter(emailParameter);
        }

        public bool EmailParameter_Delete(string appcode, int paraId, int modifyBy)
        {
            return EmailDao.DeleteEmailParameter(appcode, paraId, modifyBy);
        }

        public int EmailTemplate_Create(EmailTemplate model)
        {
            return EmailDao.CreateTemplate(model);
        }

        public EmailTemplate EmailTemplate_Get(string appcode, int tId)
        {
            return EmailDao.GetTemplate(appcode, tId);
        }

        public List<EmailTemplate> EmailTemplates_GetByGroup(string appcode, string groupName = null)
        {
            return EmailDao.GetTemplates(appcode, groupName);
        }

        public List<EmailTemplate> EmailTemplates_GetByPaging(string appcode, string name, int pageIndex, int pageSize, out int totalCount, string groupName = null)
        {
            return EmailDao.GetTemplates(appcode, name, pageIndex, pageSize, out totalCount, groupName);
        }

        public List<EmailTemplate> EmailTemplates_GetByCodes(string appcode, List<string> codes, string groupName = null)
        {
            return EmailDao.GetTemplates(appcode, codes, groupName);
        }

        public int EmailTemplate_Update(EmailTemplate emailTemplate)
        {
            return EmailDao.UpdateTemplate(emailTemplate);
        }

        public bool EmailTemplate_Delete(string appcode, int tId, int modifyBy)
        {
            return EmailDao.DeleteTemplate(appcode, tId, modifyBy);
        }

        public int InsertToMailQueue(MailQueueModel model)
        {
            return MailMasterDao.InsertToMailQueue(model);
        }

        public int InsertToMailQueueWithAttachment(MailQueueModel model, List<MailAttachment> attachments)
        {
            return MailMasterDao.InsertToMailQueueWithAttachment(model, attachments);
        }

        public int InsertToMailQueueWithAttachmentId(MailQueueModel model, List<int> attachmentIds)
        {
            return MailMasterDao.InsertToMailQueueWithAttachment(model, attachmentIds);
        }

        public int InsertToMailAttachment(MailAttachment attachment)
        {
            return MailMasterDao.InsertToMailAttachment(attachment);
        }


        public WorkflowBase WorkFlow_GetByFormId(string appcode, string entityName, int formId)
        {
            return WorkflowDao.GetActiveWorkFlow(appcode, entityName, formId);
        }

        public WorkflowBase WorkFlow_GetByRecordId(string appcode, string entityName, string recordId)
        {
            return WorkflowDao.GetActiveWorkFlow(appcode, entityName, recordId);
        }

        public WorkflowBase WorkFlow_GetLastUnactivedByInstanceId(string appcode, string workflowCode, int instanceId)
        {
            return WorkflowDao.GetLastUnactivedWorkFlow(appcode, workflowCode, instanceId);
        }

        public WorkflowBase WorkFlow_GetLastUnactivedByRecordId(string appcode, string entityName, string recordId)
        {
            return WorkflowDao.GetLastUnactivedWorkFlow(appcode, entityName, recordId);
        }

        public WorkflowBase WorkFlow_GetActiveByInstanceId(string appcode, int instanceId)
        {
            return WorkflowDao.GetActiveByInstanceId(appcode, instanceId);
        }

        public int Workflow_Insert(WorkflowBase model)
        {
            return WorkflowDao.InsertWorkflow(model);
        }


        public StaffInfo Staff_Get(string staffId)
        {
            return StaffMasterDao.GetStaffInfo(staffId);
        }

        public List<StaffInfo> Staff_GetList(string @where)
        {
            return StaffMasterDao.GetStaffList(where);
        }

    }
}
