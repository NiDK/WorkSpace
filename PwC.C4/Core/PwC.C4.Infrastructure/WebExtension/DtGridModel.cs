using System.Collections.Generic;

namespace PwC.C4.Infrastructure.WebExtension
{
    public class DtGridModel<T>
    {
        [Newtonsoft.Json.JsonProperty("data")]
        public IList<T> Data { get; set; }
        [Newtonsoft.Json.JsonProperty("draw")]
        public int Draw { get; set; }
        [Newtonsoft.Json.JsonProperty("recordsTotal")]
        public long RecordsTotal { get; set; }
        [Newtonsoft.Json.JsonProperty("recordsFiltered")]
        public long RecordsFiltered { get; set; }
    }
}
 