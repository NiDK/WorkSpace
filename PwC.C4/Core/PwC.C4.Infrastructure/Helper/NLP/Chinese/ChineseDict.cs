using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Serialization;
using PwC.C4.Configuration;

namespace PwC.C4.Infrastructure.Helper.NLP.Chinese
{
    [XmlRoot("ChineseDict")]
    public class ChineseDict : BaseConfig<ChineseDict>
    {
        ///<summary>
        /// 简体字符集
        ///</summary>
        [XmlElement("Simplified")]
        public  String Simplified  { get; set; }
        /// <summary>
        /// 繁体字符集
        /// </summary>
        [XmlElement("Traditional")]
        public  String Traditional  { get; set; }

		private readonly static object LockThis = new object();
		/// <summary>
		/// KEY:繁体字，value:简体字
		/// </summary>
    	private Dictionary<String, String> _traditionaldDic;

		/// <summary>
		/// KEY:简体字，value:繁体字
		/// </summary>
		private Dictionary<String, String> _simplifiedDic;

		public Dictionary<String, String> GetTraditionalDic()
		{
			if(_traditionaldDic==null)
			{
				InitTraditionalDic();
			}
			return _traditionaldDic;
		}

		public Dictionary<String, String> GetSimplifiedDic()
		{
			if (_simplifiedDic == null)
			{
				InitSimplifiedDic();
			}
			return _simplifiedDic;
		}

		private void InitSimplifiedDic()
		{
			lock (LockThis)
			{
				if (_simplifiedDic == null)
				{
					var dic = new Dictionary<string, String>();
					if (Instance.Traditional.Length != Instance.Simplified.Length)
					{
						throw new ConfigurationErrorsException("ChineseDict远程配置文件中的繁体字与简体字个数不同");
					}

					for (int i = 0; i < Instance.Traditional.Length; i++)
					{
						char traditional = Instance.Traditional[i];
						char simplified = Instance.Simplified[i];
						if (!dic.ContainsKey(simplified.ToString()))
						{
							dic.Add(simplified.ToString(), traditional.ToString());
						}
					}
					_simplifiedDic = dic;
				}
			}
		}


		private void InitTraditionalDic()
		{
			lock (LockThis)
			{
				if (_traditionaldDic == null)
				{
					var dic = new Dictionary<string, String>();
					if( Instance.Traditional.Length!= Instance.Simplified.Length)
					{
                        throw new ConfigurationErrorsException("ChineseDict远程配置文件中的繁体字与简体字个数不同");
					}

					for (int i = 0; i < Instance.Traditional.Length; i++)
					{
						char traditional = Instance.Traditional[i];
						char simplified=Instance.Simplified[i];
						if (!dic.ContainsKey(traditional.ToString()))
						{
							dic.Add(traditional.ToString(), simplified.ToString());
						}
					}
					_traditionaldDic = dic;
				}
			}
		}

    }
}