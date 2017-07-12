using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PwC.C4.Membership.Model
{
    [DataContract]
    internal class UserDataInCookie
    {
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string StaffCode { get; set; }
        [DataMember]
        public string TentantId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public bool IsMobile { get; set; }
        [DataMember]
        public KeyValuePair<string, string>[] Roles;
        [DataMember]
        public KeyValuePair<string, string>[] Groups;

        // Create a User object and serialize it to a JSON stream.
        public static string WriteFromObject(UserDataInCookie user)
        {


            //Create a stream to serialize the object to.
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(UserDataInCookie));
            ser.WriteObject(ms, user);
            byte[] json = ms.ToArray();
            ms.Close();

            string result = HttpUtility.HtmlEncode(Convert.ToBase64String(json));
            // return Encoding.UTF8.GetString(json, 0, json.Length);
            return result;

        }

        // Deserialize a JSON stream to a User object.
        public static UserDataInCookie ReadToObject(string json)
        {
            json = HttpUtility.HtmlDecode(json);
            UserDataInCookie deserializedUser = new UserDataInCookie();
            MemoryStream ms = new MemoryStream(Convert.FromBase64String(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as UserDataInCookie;
            ms.Close();
            return deserializedUser;
        }

    }
}
