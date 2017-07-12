using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace PwC.C4.Infrastructure.Helper
{
    public static class StringOperate
    {

        private static ThesaurusHelper thesaurusHelper = null;
        static StringOperate()
        {
            thesaurusHelper = InitBlackWord();
        }

        public static bool CheckBlackWord(string text, out List<string> listBack)
        {
            listBack = new List<string>();
            var blackWords = thesaurusHelper.SegByList(text);
            foreach (string blackWord in blackWords)
            {
                if (blackWord.Length > 1)
                    listBack.Add(blackWord);
            }
            return listBack.Count > 0;
        }

        public static bool CheckBlackWord(string text, out string black)
        {
            List<string> listBack = new List<string>();
            var blackWords = thesaurusHelper.SegByList(text);
            foreach (string blackWord in blackWords)
            {
                if (blackWord.Length > 1)
                    listBack.Add(blackWord);
            }
            black = String.Join("|", listBack.ToArray());
            return listBack.Count > 0;
        }


        private static ThesaurusHelper InitBlackWord()
        {
            List<string> result = new List<string>();
            using (Stream stream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "PwC.C4.Common.Resources.BlackDict.txt"))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                {
                    while (reader.Peek() >= 0)
                        result.Add(reader.ReadLine());
                    reader.Close();
                }
                stream.Close();
            }
            return new ThesaurusHelper(result);
        }

        public static string FilterSpecial(string str)
        {
            if (string.IsNullOrEmpty(str)) //如果字符串为空，直接返回。
            {
                return str;
            }
            else
            {
                str = str.Replace("'", "‘");
                //str = str.Replace("<", "");
                //str = str.Replace(">", "");
                str = str.Replace("%", "％");
                //str = str.Replace("'delete", "");
                str = str.Replace("''", "‘");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "，");
                //str = str.Replace(".", "。");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace(";", "：");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                //str = str.Replace("&", "");
                str = str.Replace("/", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "？");
                //str = str.Replace(" ", "");
                return str;
            }
        }

        /// <summary>
        /// 在字符串数组中查找指定值是否存在
        /// </summary>
        /// <param name="arStr">数组</param>
        /// <param name="strFind">值</param>
        /// <returns></returns>
        public static bool SearchValueInArrayIsExist(string[] arStr, string strFind)
        {
            bool IsExist = false;
            for (int i = 0; i < arStr.Length; i++)
            {

                if (arStr[i] == strFind)
                {
                    IsExist = true;
                    break;
                }
            }
            return IsExist;
        }

        public static string ClearHtml(string html)
        {
            return Regex.Replace(html, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检测一个字符串是否可以转化为日期。
        /// </summary>
        /// <param name="date">日期字符串。</param>
        /// <returns>是否可以转换。</returns>
        public static bool IsStringDate(string date)
        {
            DateTime dt;
            try
            {
                dt = DateTime.Parse(date);
            }
            catch (FormatException e)
            {
                //日期格式不正确时
                e.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取拆分符左边的字符串
        /// </summary>
        /// <param name="String">需要做处理的字符串</param>
        /// <param name="splitChar">拆分字符</param>
        /// <returns>按照拆分字符拆分好的左侧字符串</returns>
        public static string GetLeftSplitString(string String, char splitChar)
        {
            string result = null;
            string[] tempString = String.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[0].ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取拆分符右边的字符串
        /// </summary>
        /// <param name="String">需要做处理的字符串</param>
        /// <param name="splitChar">拆分字符</param>
        /// <returns>按照拆分字符拆分号的右侧字符串</returns>
        public static string GetRightSplitString(string String, char splitChar)
        {
            string result = null;
            string[] tempString = String.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[tempString.Length - 1].ToString();
            }
            return result;
        }

        /// <summary>
        /// 检查某字符是否是数字
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <returns>True表示是数字,False表示不是数字</returns>
        public static bool RteNum(string str)
        {

            if (string.IsNullOrEmpty(str))
            {

                return false;

            }
            else
            {

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");

                return reg.IsMatch(str);
            }


        }

        /// <summary>
        /// 检测某字符是否为英文字母
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <returns>True表示是英文字母,False表示不是英文字母</returns>
        public static bool CheckEnglish(string str)
        {

            if (string.IsNullOrEmpty(str))
            {

                return false;

            }
            else
            {

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z_]+$");//正则表达式 验证英文、数字、下划线和点Regex(@"^[0-9a-zA-Z_]+$");--^[a-zA-Z0-9_\u4e00-\u9fa5]+$ 

                return reg.IsMatch(str);
            }


        }

        /// <summary>
        /// 判断合法的URL
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidUrl(string strIn)
        {
            return Regex.IsMatch(strIn, @"^http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");


        }

        /// <summary>
        ///  判断是否有非法字符
        /// </summary>
        /// <param name="strString"></param>
        /// <returns>返回TRUE表示有非法字符，返回FALSE表示没有非法字符。</returns>
        public static bool CheckBadStr(string strString)
        {
            bool outValue = false;
            if (strString != null && strString.Length > 0)
            {
                string[] bidStrlist = new string[21];
                bidStrlist[0] = "'";
                bidStrlist[1] = ";";
                bidStrlist[2] = ":";
                bidStrlist[3] = "%";
                bidStrlist[4] = "@";
                bidStrlist[5] = "&";
                bidStrlist[6] = "#";
                bidStrlist[7] = "\"";
                bidStrlist[8] = "net user";
                bidStrlist[9] = "exec";
                bidStrlist[10] = "net localgroup";
                bidStrlist[11] = "select";
                bidStrlist[12] = "asc";
                bidStrlist[13] = "char";
                bidStrlist[14] = "mid";
                bidStrlist[15] = "insert";
                bidStrlist[16] = "delete";
                bidStrlist[17] = "drop";
                bidStrlist[18] = "truncate";
                bidStrlist[19] = "xp_cmdshell";
                bidStrlist[19] = "order";




                string tempStr = strString.ToLower();
                for (int i = 0; i < bidStrlist.Length; i++)
                {
                    //if (tempStr.IndexOf(bidStrlist[i]) != -1)
                    if (tempStr == bidStrlist[i])
                    {
                        outValue = true;
                        break;
                    }
                }
            }
            return outValue;
        }

        /// <summary>
        /// 转换特殊字符为全角,防止SQL注入攻击
        /// </summary>
        /// <param name="str">要过滤的字符</param>
        /// <returns>返回全角转换后的字符</returns>
        public static string ChangeFullAngle(string str)
        {
            string tempStr = str;
            if (string.IsNullOrEmpty(tempStr)) //如果字符串为空，直接返回。
            {
                return tempStr;
            }
            else
            {
                tempStr = str.ToLower();
                tempStr = str.Replace("'", "‘");
                tempStr = str.Replace("--", "－－");
                tempStr = str.Replace(";", "；");
                tempStr = str.Replace("exec", "ＥＸＥＣ");
                tempStr = str.Replace("execute", "ＥＸＥＣＵＴＥ");
                tempStr = str.Replace("declare", "ＤＥＣＬＡＲＥ");
                tempStr = str.Replace("update", "ＵＰＤＡＴＥ");
                tempStr = str.Replace("delete", "ＤＥＬＥＴＥ");
                tempStr = str.Replace("insert", "ＩＮＳＥＲＴ");
                tempStr = str.Replace("select", "ＳＥＬＥＣＴ");
                tempStr = str.Replace("<", "＜");
                tempStr = str.Replace(">", "＞");
                tempStr = str.Replace("%", "％");
                tempStr = str.Replace(@"\", "＼");
                tempStr = str.Replace(",", "，");
                tempStr = str.Replace(".", "．");
                tempStr = str.Replace("=", "＝＝");
                tempStr = str.Replace("||", "｜｜");
                tempStr = str.Replace("[", "【");
                tempStr = str.Replace("]", "】");
                tempStr = str.Replace("&", "＆");
                tempStr = str.Replace("/", "／");
                tempStr = str.Replace("|", "｜");
                tempStr = str.Replace("?", "？");
                tempStr = str.Replace("_", "＿");

                return str;
            }
        }


        /// <summary>
        /// 检测某字符是否为英文字母,数字,下划线
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <returns>True表示是英文字母,False表示不是英文字母</returns>
        public static bool CheckIsCharaterAndNumber(string str)
        {

            if (string.IsNullOrEmpty(str))
            {

                return false;

            }
            else
            {

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[0-9a-zA-Z_]+$");//正则表达式 验证英文、数字、下划线和点Regex(@"^[0-9a-zA-Z_]+$");--^[a-zA-Z0-9_\u4e00-\u9fa5]+$ 

                return reg.IsMatch(str);
            }


        }

        /// <summary>
        /// 检测某字符在某字符串中出现的次数
        /// </summary>
        /// <param name="checkStr">要检测的字符,比如"A"</param>
        /// <param name="str">要检测的字符串,比如"AAABBAACCC"</param>
        /// <returns>返回此字符出现的次数</returns>
        public static int CountCharacter(char checkStr, string str)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == checkStr)
                {
                    count++;
                }
            }
            return count;
        }


        /// <summary>
        /// 按字节数获取字符串的长度
        /// </summary>
        /// <param name="String">要计算的字符串</param>
        /// <returns>字符串的字节数</returns>
        public static int GetByteCount(string String)
        {
            int intCharCount = 0;
            for (int i = 0; i < String.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(String.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }

        /// <summary>
        /// 截取指定字节数的字符串
        /// </summary>
        /// <param name="Str">原字符串</param>
        /// <param name="Num">要截取的字节数</param>
        /// <returns>截取后的字符串</returns>
        public static string CutStr(string Str, int Num)
        {
            if (Encoding.Default.GetBytes(Str).Length <= Num)
            {
                return Str;
            }
            else
            {
                int CutBytes = 0;
                for (int i = 0; i < Str.Length; i++)
                {
                    if (Convert.ToInt32(Str.ToCharArray().GetValue(i)) > 255)
                    {
                        CutBytes = CutBytes + 2;
                    }
                    else
                    {
                        CutBytes = CutBytes + 1;
                    }
                    if (CutBytes == Num)
                    {
                        return Str.Substring(0, i + 1);
                    }
                    if (CutBytes == Num + 1)
                    {
                        return Str.Substring(0, i);
                    }
                }
                return Str;
            }
        }

        /// <summary>
        /// 防止sql注入
        /// </summary>
        /// <param name="inputName"></param>
        /// <returns></returns>
        public static string SqlReplace(string inputName)
        {
            if (string.IsNullOrEmpty(inputName))
            {
                return string.Empty;
            }

            string[] strCheck = { "'", "%", "--", ";", "EXE", "EXECUTE", "DECLARE", "UPDATE", "DELETE", "INSERT", "SELECT", "_" };
            string[] strReplace = { "＇", "％", "－－", "；", "ＥＸＥＣ", "ＥＸＥＣＵＴＥ", "ＤＥＣＬＡＲＥ", "ＵＰＤＡＴＥ", "ＤＥＬＥＴＥ", "ＩＮＳＥＲＴ", "ＳＥＬＥＣＴ", "＿" };
            for (int i = 0; i < strCheck.Length; i++)
            {
                inputName = Regex.Replace(inputName, strCheck[i], strReplace[i], RegexOptions.IgnoreCase);
            }
            return inputName;
        }

        #region//对入库字符进行编码和转换。
        /// <summary>
        /// 对入库字符进行编码和转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeStr(string str)
        {
            str = "" + str;//防止str为NULL时出错
            str = str.Replace("&nbsp", "&amp;nbsp");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("'", "’");
            str = str.Replace("\"", "&quot;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br/>");
            return str;
        }
        #endregion

        #region//对出库字符进入显示时的转换。
        /// <summary>
        /// 对出库字符进入显示时的转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecodeStr(string str)
        {
            str = "" + str;//防止str为NULL时出错
            str = str.Replace("&amp;nbsp", "&nbsp");
            str = str.Replace("&nbsp;", " ");//用于文本框中输入的空格转化成html标记
            str = str.Replace("’", "'");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("<br/>", "\n");

            return str;
        }
        #endregion

        #region 数字字符串检查


        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Regex RegNumber = new Regex("^[0-9]+$");//正整数
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");//正负整数
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");//小数
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsChinese(string inputData)
        {
            Regex RegChinese = new Regex("[\u4e00-\u9fa5]+");//中文
            Match m = RegChinese.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 检查是否有非法字符
        /// <summary>
        /// 检查是否有非法字符
        /// </summary>
        /// 编码作者：LYT
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool ValidatorStr(string inputData)
        {
            bool isPass = false;
            if (inputData.Length > 0)
            {
                Regex RegStr = new Regex(@"^[^<>'=&*,]+$");
                Match m = RegStr.Match(inputData);
                isPass = m.Success;
            }
            else
            {
                isPass = true;
            }

            return isPass;
        }

        #endregion

        #region 邮件地址
        public static readonly Regex EmailRegex = new Regex(
            @"^([a-zA-Z0-9][_\.\-]*)+\@([A-Za-z0-9])+((\.|-|_)[A-Za-z0-9]+)*((\.[A-Za-z0-9]{2,15}){1,2})$",
            RegexOptions.Compiled);

        /// <summary>
        /// 判断合法的E-Mail
        /// </summary>
        /// <param name="email">要检查的字符串</param>
        /// <returns>True表示是合法Email,False表示不是合法Email</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// 是否是邮件地址 同IsValidEmail
        /// </summary>
        /// <param name="email">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return IsValidEmail(email);
        }
        /// <summary>
        /// 是否是邮件地址 同IsValidEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsRealEmail(string email)
        {
            return IsValidEmail(email);
        }

        #endregion

        #region URL网址
        /// <summary>
        /// 是否是带http://的网址
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsUrl(string inputData)
        {
            Regex RegUrl = new Regex("^http://([w-]+.)+[w-]+(/[w-./ %&=]*)$");//带http://的网址
            Match m = RegUrl.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 固定电话
        /// <summary>
        /// 是否是国内电话号码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsTel(string inputData)
        {
            // Regex RegTel = new Regex(@"^(\d{3}-\d{8})$|^(\d{4}-\d{7})$|^(\d{11})$");//国内电话号码 ，正确格式为：“XXXX-XXXXXXX”，“XXXX-XXXXXXXX”，“XXX-XXXXXXX”， “XXX-XXXXXXXX”，“XXXXXXX”，“XXXXXXXX”。
            //Regex RegTel = new Regex(@"^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$");
            Match m = RegTel.Match(inputData);
            return m.Success;
        }

        static readonly Regex RegTel = new Regex(@"^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$", RegexOptions.Compiled);

        #endregion

        #region QQ
        /// <summary>
        /// 是否是QQ
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsQQ(string inputData)
        {
            Regex RegQQ = new Regex("^[1-9]*[1-9][0-9]*$");//匹配腾讯QQ号
            Match m = RegQQ.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 身份证
        /// <summary>
        /// 是否是国内身份证号
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsIDCard(string inputData)
        {
            Regex RegIDCard = new Regex(@"(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$)|(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)");//匹配国内身份证号〕
            Match m = RegIDCard.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 账号
        /// <summary>
        /// 账号是否合法，字母开头，数字、26个英文字母或者下划线组成6-16位的字符串
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsUserName(string inputData)
        {
            Regex RegUserName = new Regex(@"^[a-zA-Z]\w{5,15}$");//匹配由字母开头，数字、26个英文字母或者下划线组成6-16位的字符串
            Match m = RegUserName.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 英文字母
        /// <summary>
        /// 是否是由26个英文字母组成的字符串
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsEnglish(string inputData)
        {
            Regex RegEnglish = new Regex("^[A-Za-z]+$");//由26个英文字母组成的字符串 
            Match m = RegEnglish.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 空行
        /// <summary>
        /// 是否有空行
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsTrimRow(string inputData)
        {
            Regex RegTrimRow = new Regex(@"\n[\s| ]*\r");//空行
            Match m = RegTrimRow.Match(inputData);
            return m.Success;
        }
        #endregion

        #region 手机
        /// <summary>
        /// 是否是国内手机
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsMobile(string inputData)
        {
            //Regex RegMobile = new Regex(@"^((\(\d{2,3}\))|(\d{3}\-))?1[3|5]\d{9}$");//国内手机
            Match m = RegMobile.Match(inputData);
            return m.Success;
        }

        static readonly Regex RegMobile = new Regex(@"^((\(\d{2,3}\))|(\d{3}\-))?1[3|5|4|8|7]\d{9}$", RegexOptions.Compiled);//国内手机

        #endregion

        #region  日期

        private static DateTime _TempDate;

        /// <summary>
        /// 检查是否是日期
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsDate(string inputData)
        {
            return DateTime.TryParse(inputData, out _TempDate);

            // xuyc: 优化一下效率
            //try
            //{
            //    DateTime.Parse(inputData);
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        #endregion

        #region  检查字符串最大长度，返回指定长度的串

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }


        #endregion

        #region//获得<>中的内容
        public static string filterHtm(string htmlStr)
        {
            int flag;
            if (htmlStr.IndexOf(">") < htmlStr.IndexOf("<") || htmlStr.IndexOf("<") == 0)
                flag = 0;
            else
                flag = 1;
            string filterStr = "";
            foreach (char str in htmlStr)
            {
                if (str.ToString() == "<")
                    flag = 0;
                if (flag == 1)
                    filterStr += str.ToString();
                if (str.ToString() == ">")
                    flag = 1;
            }
            return filterStr;
        }
        #endregion

        #region 去掉最后一个逗号
        /// <summary>
        /// 去掉最后一个逗号
        /// </summary>
        /// <param name="String">要做处理的字符串</param>
        /// <returns>去掉最后一个逗号的字符串</returns>
        public static string DelLastComma(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.IndexOf(",") == -1)
            {
                return str;
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        /// <summary>
        /// 根据0,1,2等数字转换为相应的字母ABC
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetChangedCharacter(int num)
        {
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();

            byte[] byteArray = new byte[] { (byte)(num + 65) };
            string strCharacter = asciiEncoding.GetString(byteArray);

            return strCharacter;
        }

        public static string GetPopJScript(string message)
        {
            return string.Format("<script type='text/javascript'>alert('{0}');</script>", message);
        }

        public static string GetRunJscript(string message)
        {
            return string.Format("<script type='text/javascript'>{0}</script>", message);
        }
        /*
        /// <summary>
        /// 截取指定字节数的字符串
        /// </summary>
        /// <param name="Str">原字符串</param>
        /// <param name="Num">要截取的字节数</param>
        /// <returns>截取后的字符串</returns>
        public static string CutStr(string Str, int Num)
        {
            if (Encoding.Default.GetBytes(Str).Length <= Num)
            {
                return Str;
            }
            else
            {
                int CutBytes = 0;
                for (int i = 0; i < Str.Length; i++)
                {
                    if (Convert.ToInt32(Str.ToCharArray().GetValue(i)) > 255)
                    {
                        CutBytes = CutBytes + 2;
                    }
                    else
                    {
                        CutBytes = CutBytes + 1;
                    }
                    if (CutBytes == Num)
                    {
                        return Str.Substring(0, i + 1);
                    }
                    if (CutBytes == Num + 1)
                    {
                        return Str.Substring(0, i);
                    }
                }
                return Str + "...";
            }
        }
        */
        public static string DownLoadUrlFile(string urlInfo)
        {
            return string.Format("<script type='text/javascript'>$('#DownLoad').></a></script>", urlInfo);
        }

        public static string Request(string url, string paras)
        {
            string[] paraString = url.Substring(url.IndexOf("?") + 1).Split('&');
            for (int i = 0; i < paraString.Length; i++)
            {
                if (paraString[i].Substring(0, paraString[i].IndexOf("=")) == paras)
                {
                    return paraString[i].Substring(paraString[i].IndexOf("=") + 1);
                }
            }
            return string.Empty;
        }


    }



    public class StringHelper<T>
    {
        #region 把List<Object>转换成以，相隔的字符串
        /// <summary>
        /// 把List转换成以，相隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetJoinCommaString(List<T> list)
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    strBuilder.Append(list[i].ToString());
                }
                else
                {
                    strBuilder.Append(",");
                    strBuilder.Append(list[i].ToString());
                }
            }
            return strBuilder.ToString();
        }
        #endregion

        public static string GetLowerString(T t)
        {
            return t.ToString().ToLower();
        }
        public static Guid GetLowerGuid(T t)
        {
            return new Guid(t.ToString().ToLower());
        }







    }





}
