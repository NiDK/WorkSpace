using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PwC.C4.Infrastructure.Helper
{

    public static class JsonHelper
    {

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize(object obj)
        {
            var jSetting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };
            return JsonConvert.SerializeObject(obj, jSetting);
        }

    }
}
