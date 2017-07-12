using System;
using System.Runtime.Serialization;

namespace PwC.C4.Common.Model
{
    [DataContract]
    public class UserSession
    {
        [DataMember]
        public string SessionKey { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string ValidataCode { get; set; }
        [DataMember]
        public string IPAddess { get; set; }
        [DataMember]
        public int LoginFeltTimes { get; set; }
        [DataMember]
        public DateTime LastActiveTime { get; set; }
    }
}
