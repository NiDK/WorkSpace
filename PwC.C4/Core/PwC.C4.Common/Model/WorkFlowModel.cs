using System;
using System.Collections.Generic;
using WorkflowEngineRuntime.DataModel;

namespace PwC.C4.Common.Model
{
    public class WorkflowModel
    {
        public bool IsInitialize { get; set; }
        public int WorkflowInstanceId { get; set; }
        public string WorkFlowCode { get; set; }
        public string ActionCode { get; set; }
        public string BusinessInfo { get; set; }
        public List<InputArgument> Arguments { get; set; }

        public string EntityName { get; set; }
        public string RecordId { get; set; }
        public int FormId { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string FormStatus { get; set; }
        public string Comment { get; set; }
        /// <summary>
        /// Only for Go anywhere
        /// </summary>
        public string TargetState { get; set; }
    }
}