using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PwC.C4.Configuration;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Infrastructure.Helper.NLP.Pinyin
{
    ///<summary>
    /// 拼音词典
    ///</summary>
    [XmlRoot("PingYinDict")]
    public class PingYinDict : BaseConfig<PingYinDict>
    {
        private static readonly LogWrapper Logger = new LogWrapper();
        /// <summary>
        /// 正在词库里支持的最小值
        /// </summary>
        [XmlElement("MinAscii")]
        public  int MinAscii { get; set; }
        /// <summary>
        /// 正在词库里支持的最大值
        /// </summary>
        [XmlElement("MaxAscii")]
        public  int MaxAscii { get; set; }

        
        /// <summary>
        ///<!-- <汉字的Ascii码,对应的拼音>-->
        /// 汉字拼音词典  
        ///</summary>
        private Dictionary<int, String> _dic;


        ///<summary>
        /// 获取汉字拼音词典
        ///</summary>
        ///<returns></returns>
        public  Dictionary<int, String> GetDic()
        {
			if (!_initializationComplete)
            {
                Init();
            }
            return _dic;
        }

        ///<summary>
        /// 全拼的词典文本
        ///</summary>
        [XmlElement("SpellingText")]
        public String SpellingText { get; set; }

        private  object _lockThis = new object();

        
        /// <summary>
        /// 全拼
        /// </summary>
        private string[] _spelling;

        ///<summary>
        /// 获取全拼词典
        ///</summary>
        ///<returns></returns>
        public string[] GetSpelling()
        {
			if (!_initializationComplete)
            {
                Init();
            }
            return _spelling;
        }

    	private bool _initializationComplete = false;

        ///<summary>
        /// 汉字转拼音词典文本
        ///</summary>
        [XmlElement("Chinese2PingYinText")]
        public String Chinese2PingYinText { get; set; }

       
        ///<summary>
        /// 初始化
        ///</summary>
        public void Init()
        {
            lock (_lockThis)
            {
				if(_initializationComplete)
				{
					return;
				}
            	#region 全拼

                var array = PingYinDict.Instance.SpellingText.Split(',');
                var list = new List<String>();
                foreach (var s in array)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        list.Add(s);
                    }
                }
                _spelling = list.ToArray();
#endregion
                #region 汉字转拼音
                var dic = new Dictionary<int, String>();
                var chinese2PingYinTextArray = PingYinDict.Instance.Chinese2PingYinText.Split(';');
                foreach (var s in chinese2PingYinTextArray)
                {
                    if(!String.IsNullOrEmpty(s))
                    {
                        var temparray = s.Split(',');
                        if(temparray.Length==2)
                        {
                            var ascii = temparray[0];
                            var pingyin = temparray[1];
                            int asciiInt = 0;
                            Int32.TryParse(ascii, out asciiInt);
                            if (asciiInt>0)
                            {
								dic.Add(asciiInt, pingyin);
                            }else
                            {
                                Logger.Error(String.Format("当前的配置文件错误：[{0}]不是数值。", ascii));
                            }

                        }else
                        {
                            Logger.Error(String.Format("当前的配置文件错误：[{0}]不合法。",s));
                        }
                    }
                }
                #endregion
            	_dic = dic;
            	_initializationComplete = true;
            }
        }
       
    }
}