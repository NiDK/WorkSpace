using System;
using System.Collections.Generic;

namespace PwC.C4.Infrastructure.Helper.NLP.Pinyin
{
    public class PingYinModel
    {
        /// <summary>
        /// ���ֵĶ��ƴ��
        /// </summary>
        public List<String> PingYins { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public String Word { get; set; }

        
        public PingYinModel(String word, List<String> pingYins)
        {
            Word = word;
            PingYins = pingYins;
        }
    }
}