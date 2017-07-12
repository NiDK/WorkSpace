using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Dfs.Converter.Model
{
    public class ConvertionInfo
    {
        public string ConvertFileName { get; set; }

        public DateTime ConvertStatTime { get; set; }

        public DateTime ConvertFinishTime { get; set; }

        /// <summary>
        /// Size:L,M,S
        /// </summary>
        public string ConvertMode { get; set; }

        public string ConvertPara { get; set; }

        public string ConvertServer { get; set; }

        public string ConvertDfsPath { get; set; }
    }
}
