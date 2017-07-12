using System.ServiceModel;

namespace PwC.C4.DataService.Model
{
    [MessageContract]
    public class MailMessageResult
    {
        public int ReturnId { get; set; }
    }
}