using System;
using System.Text;
using System.Text.RegularExpressions;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.BaseLogger;

namespace PwC.C4.Infrastructure.Helper.NLP.Chinese
{

    
    ///<summary>
    /// 中文简繁体互转换
    ///</summary>
    public class ChineseConvertHelper
    {
        private static Regex reg = new Regex("^[\u4e00-\u9fa5]$"); //验证是否输入汉字  

        private static readonly LogWrapper Logger = new LogWrapper();

		
        ///<summary>
        /// 繁体转简体
        ///</summary>
        ///<param name="value"></param>
        ///<returns></returns>
        public static String ToSimplified(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            var result = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
				var str = value[i].ToString();

				if( ChineseDict.Instance.GetTraditionalDic().ContainsKey(str)) //如果当前字符是繁体字，转成简体字
				{
					result.Append(ChineseDict.Instance.GetTraditionalDic()[str]);
				}
				else
				{
					result.Append(str);
					//如果输入的是汉字
					if (reg.IsMatch(str))
					{
						Logger.Info(String.Format("指定的繁体字[{0}]没有找到对应的简体字", str));
					}
				}

            }
            return result.ToString();


        }


		
		
        ///<summary>
        /// 简体转繁体--简体转繁体会不太准确，因为一个简体字可能对应一个以上的繁体字。
        /// 如 “发” 转成 "發","髮"
        ///</summary>
        ///<param name="value"></param>
        ///<returns></returns>
        public static String ToTraditional(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            var result = new StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                var str = value[i].ToString();

				if (ChineseDict.Instance.GetSimplifiedDic().ContainsKey(str)) //如果当前字符是简体字，转成繁体字
				{
					result.Append(ChineseDict.Instance.GetSimplifiedDic()[str]);
				}
                else
                {
                    result.Append(str);
                    //如果输入的是汉字
                    if (reg.IsMatch(str))
                    {
                        Logger.Info(String.Format("指定的简体字[{0}]没有找到对应的繁体字", str));
                    }
                }
            }
            return result.ToString();
        }
    }
}