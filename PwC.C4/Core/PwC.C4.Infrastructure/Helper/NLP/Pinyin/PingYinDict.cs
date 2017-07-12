using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PwC.C4.Configuration;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Infrastructure.Helper.NLP.Pinyin
{
    ///<summary>
    /// ƴ���ʵ�
    ///</summary>
    [XmlRoot("PingYinDict")]
    public class PingYinDict : BaseConfig<PingYinDict>
    {
        private static readonly LogWrapper Logger = new LogWrapper();
        /// <summary>
        /// ���ڴʿ���֧�ֵ���Сֵ
        /// </summary>
        [XmlElement("MinAscii")]
        public  int MinAscii { get; set; }
        /// <summary>
        /// ���ڴʿ���֧�ֵ����ֵ
        /// </summary>
        [XmlElement("MaxAscii")]
        public  int MaxAscii { get; set; }

        
        /// <summary>
        ///<!-- <���ֵ�Ascii��,��Ӧ��ƴ��>-->
        /// ����ƴ���ʵ�  
        ///</summary>
        private Dictionary<int, String> _dic;


        ///<summary>
        /// ��ȡ����ƴ���ʵ�
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
        /// ȫƴ�Ĵʵ��ı�
        ///</summary>
        [XmlElement("SpellingText")]
        public String SpellingText { get; set; }

        private  object _lockThis = new object();

        
        /// <summary>
        /// ȫƴ
        /// </summary>
        private string[] _spelling;

        ///<summary>
        /// ��ȡȫƴ�ʵ�
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
        /// ����תƴ���ʵ��ı�
        ///</summary>
        [XmlElement("Chinese2PingYinText")]
        public String Chinese2PingYinText { get; set; }

       
        ///<summary>
        /// ��ʼ��
        ///</summary>
        public void Init()
        {
            lock (_lockThis)
            {
				if(_initializationComplete)
				{
					return;
				}
            	#region ȫƴ

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
                #region ����תƴ��
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
                                Logger.Error(String.Format("��ǰ�������ļ�����[{0}]������ֵ��", ascii));
                            }

                        }else
                        {
                            Logger.Error(String.Format("��ǰ�������ļ�����[{0}]���Ϸ���",s));
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