using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using PwC.C4.Infrastructure.Helper.NLP.Chinese;
using PwC.C4.Infrastructure.BaseLogger;

namespace PwC.C4.Infrastructure.Helper.NLP.Pinyin
{
    /// <summary>
    /// 
    /// </summary>
    public class PingYinConvertHelper
    {
        private static readonly LogWrapper Logger = new LogWrapper();

        private static Regex reg = new Regex("^[\u4e00-\u9fa5]$"); //验证是否输入汉字  

        /// <summary>
        /// 单个汉字转拼音
        /// </summary>
        /// <param name="singleString">单个汉字</param>
        /// <returns></returns>
        private static String SingleConvertToPingYin(String singleString)
        {
            try {
                if (String.IsNullOrEmpty(singleString))
                {
                    return String.Empty;
                }
                if (singleString.Length > 1)
                {
                    singleString = singleString[0].ToString();
                }
                int asc = SingleConvertToAscii(singleString);
                if (asc > 0 && asc < 160)
                {
                    return singleString;
                }
                if (PingYinDict.Instance.MinAscii <= asc && asc <= PingYinDict.Instance.MaxAscii)
                {
                    if (PingYinDict.Instance.GetDic().ContainsKey(asc))
                    {
                        return PingYinDict.Instance.GetDic()[asc];
                    }
                    Logger.Error("指定的汉字没有对应的拼音值，[" + singleString + "]");
                }
            }
            catch (Exception e) {
                Logger.HandleException(new Exception(String.Format("[{0}]转换拼音时异常", singleString), e), "");
            }           
            return singleString;
        }


        /// <summary>
        /// 转换成Ascii码
        /// </summary>
        /// <param name="singleString">单个字符</param>
        /// <returns></returns>
        private static int SingleConvertToAscii(String singleString)
        {
            int asc = 0;
            try {               
                //如果输入的是汉字 
                if (reg.IsMatch(singleString))
                {
                    int m1 = 0, m2 = 0;
                    byte[] arr = Encoding.Default.GetBytes(singleString);
                    m1 = (arr[0]);
                    m2 = (arr[1]);
                    asc = m1 * 256 + m2;
                }
                else
                {
                    asc = Encoding.Default.GetBytes(singleString)[0];
                }
            }
            catch (Exception e) {
                Logger.HandleException(new Exception("未知的字符"+singleString,e),"未知的汉字");
            }
            return asc;
        }

        /// <summary> 
        /// 转换成汉语拼音首字母串
        /// </summary> 
        /// <param name="cnStr">汉字字符串</param> 
        /// <returns>相对应的汉语拼音首字母串</returns> 
        public static string ToShortPingYin(string cnStr)
        {
            if (String.IsNullOrEmpty(cnStr))
            {
                return String.Empty;
            }
            cnStr = ChineseConvertHelper.ToSimplified(cnStr);
            var result = new StringBuilder();
            for (int i = 0; i < cnStr.Length; i++)
            {
                string longpinying = SingleConvertToPingYin(cnStr.Substring(i, 1));
                if (longpinying.Length > 0)
                {
                    result.Append(longpinying[0]);
                }
            }
            return result.ToString().ToUpper();
        }

        /// <summary>
        /// 判断value是否是个拼音
        /// </summary>
        /// <param name="value">拼音</param>
        /// <returns></returns>
        public static bool IsPingYin(string value)
        {
            value = value.ToLower();
            foreach (string s in PingYinDict.Instance.GetSpelling())
            {
                if (value.StartsWith(s))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  
        /// 汉字转换成全拼的拼音  
        /// </summary>  
        ///  <param name="chstr">汉字字符串  </param>
        /// <returns>转换后的拼音字符串</returns>  
        public static string ToLongPingYin(string chstr)
        {
            if (string.IsNullOrEmpty(chstr))
            {
                return string.Empty;
            }
            chstr = ChineseConvertHelper.ToSimplified(chstr);
            var result = new StringBuilder();
            for (int j = 0; j < chstr.Length; j++)
            {
                string value = SingleConvertToPingYin(chstr.Substring(j, 1));
                if (value.Length > 0)
                {
                    result.Append(value.Split('/')[0]);
                }
            }
            return result.ToString(); //返回获取到的汉字拼音  
        }

        ///<summary>
        /// 获取汉字的全拼
        /// 例：“曾”  拼音： “zeng” “ceng”
        ///</summary>
        ///<param name="chstr">汉字字符串 </param>
        ///<returns>返回每一个汉字的拼音，多音字的拼音</returns>
        public static List<PingYinModel> GetLongPingYin(String chstr)
        {
            if (String.IsNullOrEmpty(chstr))
            {
                return null;
            }
            var result = new List<PingYinModel>();
            try {
                chstr = ChineseConvertHelper.ToSimplified(chstr);
                for (int j = 0; j < chstr.Length; j++)
                {
                    string chineseChar = chstr.Substring(j, 1);
                    string pingYins = SingleConvertToPingYin(chineseChar);
                    if (pingYins.Length > 0)
                    {
                        var list = new List<String>();
                        if (pingYins.IndexOf("/") != -1)
                        {
                            string[] pingYinList = pingYins.Split('/');
                            list.AddRange(pingYinList);
                        }
                        else
                        {
                            list.Add(pingYins);
                        }
                        result.Add(new PingYinModel(chineseChar, list));
                    }
                }
            }
            catch (Exception e) { 
                Logger.HandleException(new Exception("未知的字符"+chstr,e), "未知的汉字");
            }            
            return result;
        }

        /// <summary>
        /// 获取当前拼音Model的多音字拼音的组合(主要用于人名的多音组合)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="longPingYin"></param>
        /// <param name="shortPingYin"></param>
        public static void GetPingYingCombination(List<PingYinModel> list, ref List<String> longPingYin, ref List<String> shortPingYin)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var currentList = list[i].PingYins;
                List<String> longPingYinCopy = new List<String>();
                List<String> shortPingYinCopy = new List<String>();
                for (int m = 0; m < currentList.Count; m++)
                {
                    String currentText = currentList[m];
					if (!String.IsNullOrEmpty(currentText))
					{
						if (i == 0)
						{
							longPingYin.Add(currentText);
							shortPingYin.Add(currentText[0].ToString().ToUpper());
						}
						else
						{
							if (m == 0)
							{
								longPingYinCopy.AddRange(longPingYin);
								shortPingYinCopy.AddRange(shortPingYin);
							}
							for (int j = longPingYinCopy.Count * m; j < longPingYin.Count; j++)
							{
								longPingYin[j] += currentText;
								shortPingYin[j] += currentText[0].ToString().ToUpper();
							}
							if (currentList.Count > 1 && m < currentList.Count - 1)
							{
								longPingYin.AddRange(longPingYinCopy);
								shortPingYin.AddRange(shortPingYinCopy);
							}
						}
					}
                }

            }
        }
    
        //medcl 20120316 返回拼音，拼音之间用空格分隔
        public static void GetPingYingWithBlank(List<PingYinModel> list, out List<String> longPingYin, out List<String> shortPingYin)
        {
            longPingYin=new List<string>();
            shortPingYin=new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                var currentList = list[i].PingYins;
                List<String> longPingYinCopy = new List<String>();
                List<String> shortPingYinCopy = new List<String>();
                for (int m = 0; m < currentList.Count; m++)
                {
                    String currentText = currentList[m];
                    if (!String.IsNullOrEmpty(currentText))
                    {
                        if (i == 0)
                        {
                            longPingYin.Add(currentText);
                            shortPingYin.Add(currentText[0].ToString().ToUpper());
                        }
                        else
                        {
                            if (m == 0)
                            {
                                longPingYinCopy.AddRange(longPingYin);
                                shortPingYinCopy.AddRange(shortPingYin);
                            }
                            for (int j = longPingYinCopy.Count * m; j < longPingYin.Count; j++)
                            {
                                longPingYin[j] +=(" " +currentText);
                                shortPingYin[j] +=currentText[0].ToString().ToUpper();
                            }
                            if (currentList.Count > 1 && m < currentList.Count - 1)
                            {
                                longPingYin.AddRange(longPingYinCopy);
                                shortPingYin.AddRange(shortPingYinCopy);
                            }
                        }
                    }
                }

            }
        }

    }
}