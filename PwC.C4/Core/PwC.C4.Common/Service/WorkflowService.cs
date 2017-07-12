using System;
using System.Collections.Generic;
using System.Linq;
using PwC.C4.Common.Interface;
using PwC.C4.Common.Model;
using PwC.C4.Common.Model.Enum;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using WorkflowEngineRuntime.DataModel;

namespace PwC.C4.Common.Service
{
    public class WorkflowService:IWorkflowService
    {
        private static C4.Infrastructure.Logger.LogWrapper _log;
        private static WorkflowService _instance = null;
        private static readonly object LockHelper = new object();
        private static WorkflowRuntimeServiceClient _workFlowClient = null;
        private static C4CommonServiceClient _c4Client = null;
        private static string _appCode = null;
        public WorkflowService()
        {
        }

        public static IWorkflowService Instance()
        {
            if (_instance == null || _c4Client == null || _workFlowClient == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _c4Client != null && _workFlowClient != null && _appCode != null &&
                        _log != null) return _instance;
                    _instance = new WorkflowService();
                    _c4Client = new C4CommonServiceClient();
                    _workFlowClient = new WorkflowRuntimeServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                    _log = new LogWrapper();
                }
            }
            return _instance;
        }

        private WorkflowResult DoAction(WorkflowActionMode mode, WorkflowModel model, bool forceInit = false)
        {
            var client = new WorkflowRuntimeServiceClient();
            var finalResult = new WorkflowResult()
            {
                Result = false
            };
            try
            {
                var baseModel = model.FormId != 0
                    ? _c4Client.WorkFlow_GetByFormId(_appCode,model.EntityName, model.FormId)
                    : _c4Client.WorkFlow_GetByRecordId(_appCode, model.EntityName, model.RecordId);
                var arg = model.Arguments ?? new List<InputArgument>();
                model.ActionCode = string.IsNullOrEmpty(model.ActionCode) ? "AdminModify" : model.ActionCode;
                var workFlowInput = new WorkflowInput()
                {
                    ActionCode = model.ActionCode,
                    WorkflowCode = model.WorkFlowCode,
                    UserID = model.UserId,
                    Comment = model.Comment,
                    BusinessInfo = model.BusinessInfo,
                    InputArgumentsList = arg.ToArray()
                };

                var result = new ActionResult() {ActionSuccessful = false};
                switch (mode)
                {
                    case WorkflowActionMode.GoNext:
                        if (baseModel != null && !forceInit)
                        {
                            workFlowInput.InstanceID = baseModel.InstanceId;
                            model.WorkflowInstanceId = baseModel.InstanceId;
                            model.IsInitialize = false;
                            result = client.DoAction(workFlowInput);
                        }
                        else
                        {
                            workFlowInput.InstanceID = 0;
                            model.IsInitialize = true;
                            result = client.InitializeWorkflow(workFlowInput);
                        }
                        break;
                    case WorkflowActionMode.GoBack:
                        workFlowInput.InstanceID = baseModel.InstanceId;
                        workFlowInput.ActionCode = baseModel.ActionCode;
                        workFlowInput.UserID = baseModel.UserId;
                            result = client.RollbackDoAction(workFlowInput);
                            break;
                    case WorkflowActionMode.GoAnywhere:
                        workFlowInput.InstanceID = baseModel.InstanceId;
                        result = client.ChangeInstanceStateManually(workFlowInput, model.TargetState);
                        break;
                }


                if (result.ActionSuccessful)
                {
                    var wfbase = new WorkflowBase()
                    {
                        AppCode = AppSettings.Instance.GetAppCode(),
                        WorkflowCode = model.WorkFlowCode,
                        EntityName = model.EntityName,
                        FormId = model.FormId,
                        RecordId = model.RecordId,
                        UserId = model.UserId,
                        UserRole = model.UserRole,
                        Comment = model.Comment,
                        InstanceId = workFlowInput.InstanceID
                    };
                    switch (mode)
                    {
                        case WorkflowActionMode.GoNext:
                            var rsu = result.CurrentStateList.First();
                            wfbase.Status = rsu.StateCode;
                            wfbase.ActionCode = model.ActionCode;
                            wfbase.UserId = model.UserId;
                            wfbase.InstanceId = rsu.InstanceID;
                            break;
                        case WorkflowActionMode.GoBack:
                            var unactive = _c4Client.WorkFlow_GetLastUnactivedByInstanceId(_appCode, workFlowInput.WorkflowCode,
                                workFlowInput.InstanceID);
                            if (unactive != null)
                            {
                                wfbase.Status = unactive.Status;
                                wfbase.ActionCode = unactive.ActionCode;
                                wfbase.UserId = unactive.UserId;
                            }
                            
                            break;
                        case WorkflowActionMode.GoAnywhere:
                            wfbase.Status = model.TargetState;
                            wfbase.ActionCode = model.ActionCode;
                            wfbase.UserId = model.UserId;
                            break;
                    }


                    var initId = _c4Client.Workflow_Insert(wfbase);
                    var updateSuccess = false;
                    if (initId == 0)
                    {
                        if (model.IsInitialize)
                        {
                            client.RollbackInitializeWorkflow(workFlowInput);
                        }
                        else
                        {
                            client.RollbackDoAction(workFlowInput);
                        }
                    }
                    else
                    {
                        updateSuccess = true;
                    }
                    finalResult.NextState = wfbase.Status;
                    finalResult.WorkflowInstanceId = wfbase.InstanceId;
                    finalResult.Result = updateSuccess;
                    return finalResult;
                }
                else
                {
                    _log.Error("DoAction Error, ActionSuccessful=false or CurrentStateList.Any=false,Input Model:" +
                               JsonHelper.Serialize(model) + "; \r\n Workflow result:" + JsonHelper.Serialize(result));
                }
                return finalResult;
            }
            catch (Exception ee)
            {
                _log.Error("GoNext Error, Model:" + JsonHelper.Serialize(model), ee);
                return finalResult;
            }
        }

        public WorkflowResult GoNext(WorkflowModel model, bool forceInit = false)
        {
            return DoAction(WorkflowActionMode.GoNext, model);
        }

        public WorkflowResult GoBack(WorkflowModel model)
        {
            return DoAction(WorkflowActionMode.GoBack, model);
        }

        public WorkflowResult GoAnywhere(WorkflowModel model)
        {
            return DoAction(WorkflowActionMode.GoAnywhere, model);
        }

        public WorkflowBase GetActiveWorkFlow(string entityName, string recordId)
        {
            return _c4Client.WorkFlow_GetByRecordId(_appCode, entityName, recordId);
        }

        public WorkflowBase GetActiveWorkFlow(int instanceId)
        {
            return _c4Client.WorkFlow_GetActiveByInstanceId(_appCode, instanceId);
        }

        public WorkflowBase GetLastUnactivedWorkFlow(string workflowCode, int instanceId)
        {
            return _c4Client.WorkFlow_GetLastUnactivedByInstanceId(_appCode, workflowCode, instanceId);
        }

        public WorkflowBase GetLastUnactivedWorkFlow(string entityName, string recordId)
        {
            return _c4Client.WorkFlow_GetLastUnactivedByRecordId(_appCode, entityName, recordId);
        }

        public bool GoAnywhereWithoutWorkflow(WorkflowModel model)
        {
            try
            {
                var wfbase = new WorkflowBase
                {
                    AppCode = AppSettings.Instance.GetAppCode(),
                    WorkflowCode = model.WorkFlowCode,
                    EntityName = model.EntityName,
                    FormId = model.FormId,
                    RecordId = model.RecordId,
                    UserId = model.UserId,
                    UserRole = model.UserRole,
                    Comment = model.Comment,
                    InstanceId = model.WorkflowInstanceId,
                    Status = model.TargetState,
                    ActionCode = model.ActionCode
                };
                var initId = _c4Client.Workflow_Insert(wfbase);
                return initId > 0;
            }
            catch (Exception ee)
            {
                _log.Error("GoAnywhereWithoutWorkflow error！", ee);
                return false;
            }

        }

        public InstanceInfo[] GetWorkflowInstanceInfo(string workflowCode, int[] instanceId)
        {
            var client = new WorkflowRuntimeServiceClient();
            try
            {
                return client.GetInstanceInfo(workflowCode, instanceId);
            }
            catch (Exception e)
            {
                _log.Error("GetCurrentWorkflow error！",e);
                return new InstanceInfo[0];
            }
        }

        public ActionResult UpdateWorkflowPara(WorkflowInput workflowInput, bool isRereshCurrentState)
        {
            var client = new WorkflowRuntimeServiceClient();
            try
            {
                return client.ChangeWorkflowArguments(workflowInput, isRereshCurrentState);
            }
            catch (Exception e)
            {
                _log.Error("UpdateWorkflowPara error！", e);
                return new ActionResult()
                {
                    ActionSuccessful = false,
                    InstanceID = workflowInput.InstanceID,
                    WorkflowCode = workflowInput.WorkflowCode
                };
            }
        }
    }
}
