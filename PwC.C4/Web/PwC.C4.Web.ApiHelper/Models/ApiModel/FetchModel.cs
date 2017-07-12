using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Web.ApiHelper.Models.ApiModel
{
    public class FetchModel
    {
        /// <summary>
        /// Keys
        /// </summary>
        public string K { get; set; }
        /// <summary>
        /// key colunm name
        /// </summary>
        public string Kn { get; set; }
        /// <summary>
        /// Properties
        /// </summary>
        public IList<string> P { get; set; }
        /// <summary>
        /// Separator
        /// </summary>
        public string S { get; set; }
        /// <summary>
        /// IconSetting
        /// </summary>
        public string Ico { get; set; }
        /// <summary>
        /// Query
        /// </summary>
        public string Q { get; set; }
        /// <summary>
        /// Query column
        /// </summary>
        public string Qp { get; set; }
        /// <summary>
        /// Ignore
        /// </summary>
        public IList<string> Ig { get; set; }
        /// <summary>
        /// Index
        /// </summary>
        public int? I { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public int? L { get; set; }

        /// <summary>
        /// Show default list
        /// </summary>
        public bool? Isd { get; set; }

        /// <summary>
        /// order column
        /// </summary>
        public string O { get; set; }

        /// <summary>
        /// order method
        /// </summary>
        public string Om { get; set; }

        public IList<string> KeyArray
        {
            get
            {
                if (!string.IsNullOrEmpty(K))
                    return K.Split(new string[] {S}, StringSplitOptions.RemoveEmptyEntries);
                return new List<string>();
            }
        }

    }
}
