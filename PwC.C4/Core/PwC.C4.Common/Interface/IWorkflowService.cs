using System;
using PwC.C4.Common.Model;
using PwC.C4.DataService.Model;
using WorkflowEngineRuntime.DataModel;

namespace PwC.C4.Common.Interface
{
    public interface IWorkflowService
    {

        WorkflowResult GoNext(WorkflowModel model, bool forceInit = false);

        WorkflowResult GoBack(WorkflowModel model);

        WorkflowResult GoAnywhere(WorkflowModel model);

        WorkflowBase GetActiveWorkFlow(string entityName, string recordId);

        WorkflowBase GetActiveWorkFlow(int instanceId);

        WorkflowBase GetLastUnactivedWorkFlow(string workflowCode, int instanceId);

        WorkflowBase GetLastUnactivedWorkFlow(string entityName, string recordId);

        bool GoAnywhereWithoutWorkflow(WorkflowModel model);

        InstanceInfo[] GetWorkflowInstanceInfo(string workflowCode, int[] instanceId);

        ActionResult UpdateWorkflowPara(WorkflowInput workflowInput, bool isRereshCurrentState);
    }
}
