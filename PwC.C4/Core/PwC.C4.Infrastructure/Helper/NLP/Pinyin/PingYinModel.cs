using System;
using System.Collections.Generic;

namespace PwC.C4.Infrastructure.Helper.NLP.Pinyin
{
    public class PingYinModel
    {
        /// <summary>
        /// ºº×ÖµÄ¶à¸öÆ´Òô
        /// </summary>
        public List<String> PingYins { get; set; }
        /// <summary>
        /// ºº×Ö
        /// </summary>
        public String Word { get; set; }

        
        public PingYinModel(String word, List<String> pingYins)
        {
            Word = word;
            PingYins = pingYins;
        }
    }
}