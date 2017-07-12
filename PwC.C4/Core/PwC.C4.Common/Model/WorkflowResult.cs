namespace PwC.C4.Common.Model
{
    public class WorkflowResult
    {
        public bool Result { get; set; }
        public int WorkflowInstanceId { get; set; }
        public string NextState { get; set; }
        public string NextRole { get; set; }
    }
}