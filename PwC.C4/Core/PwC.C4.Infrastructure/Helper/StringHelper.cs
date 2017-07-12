using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace PwC.C4.Infrastructure.Helper
{
    public static class StringHelper
    {

        private static readonly ThesaurusHelper ThesaurusHelper = null;
        static StringHelper()
        {
            //ThesaurusHelper = InitBlackWord();
        }

        public static bool CheckBlackWord(string text, out List<string> listBack)
        {
            listBack = new List<string>();
            var blackWords = ThesaurusHelper.SegByList(text);
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
            var blackWords = ThesaurusHelper.SegByList(text);
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
                    "PwC.C4.Infrastructure.Resources.BlackDict.txt"))
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
            if (string.IsNullOrEmpty(str)) //����ַ���Ϊ�գ�ֱ�ӷ��ء�
            {
                return str;
            }
            else
            {
                str = str.Replace("'", "��");
                //str = str.Replace("<", "");
                //str = str.Replace(">", "");
                str = str.Replace("%", "��");
                //str = str.Replace("'delete", "");
                str = str.Replace("''", "��");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "��");
                //str = str.Replace(".", "��");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace(";", "��");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                //str = str.Replace("&", "");
                str = str.Replace("/", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "��");
                //str = str.Replace(" ", "");
                return str;
            }
        }

        /// <summary>
        /// ���ַ��������в���ָ��ֵ�Ƿ����
        /// </summary>
        /// <param name="arStr">����</param>
        /// <param name="strFind">ֵ</param>
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
        /// ���һ���ַ����Ƿ����ת��Ϊ���ڡ�
        /// </summary>
        /// <param name="date">�����ַ�����</param>
        /// <returns>�Ƿ����ת����</returns>
        public static bool IsStringDate(string date)
        {
            DateTime dt;
            try
            {
                dt = DateTime.Parse(date);
            }
            catch (FormatException e)
            {
                //���ڸ�ʽ����ȷʱ
                e.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡ��ַ���ߵ��ַ���
        /// </summary>
        /// <param name="String">��Ҫ��������ַ���</param>
        /// <param name="splitChar">����ַ�</param>
        /// <returns>���ղ���ַ���ֺõ�����ַ���</returns>
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
        /// ��ȡ��ַ��ұߵ��ַ���
        /// </summary>
        /// <param name="String">��Ҫ��������ַ���</param>
        /// <param name="splitChar">����ַ�</param>
        /// <returns>���ղ���ַ���ֺŵ��Ҳ��ַ���</returns>
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
        /// ���ĳ�ַ��Ƿ�������
        /// </summary>
        /// <param name="str">Ҫ�����ַ���</param>
        /// <returns>True��ʾ������,False��ʾ��������</returns>
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
        /// ���ĳ�ַ��Ƿ�ΪӢ����ĸ
        /// </summary>
        /// <param name="str">Ҫ�����ַ���</param>
        /// <returns>True��ʾ��Ӣ����ĸ,False��ʾ����Ӣ����ĸ</returns>
        public static bool CheckEnglish(string str)
        {

            if (string.IsNullOrEmpty(str))
            {

                return false;

            }
            else
            {

                var reg = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z_]+$");//������ʽ ��֤Ӣ�ġ����֡��»��ߺ͵�Regex(@"^[0-9a-zA-Z_]+$");--^[a-zA-Z0-9_\u4e00-\u9fa5]+$ 

                return reg.IsMatch(str);
            }


        }

        /// <summary>
        /// �жϺϷ���URL
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidUrl(string strIn)
        {
            return Regex.IsMatch(strIn, @"^http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");


        }

        /// <summary>
        ///  �ж��Ƿ��зǷ��ַ�
        /// </summary>
        /// <param name="strString"></param>
        /// <returns>����TRUE��ʾ�зǷ��ַ�������FALSE��ʾû�зǷ��ַ���</returns>
        public static bool CheckBadStr(string strString)
        {
            bool outValue = false;
            if (!string.IsNullOrEmpty(strString))
            {
                var bidStrlist = new string[21];
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
        /// ת�������ַ�Ϊȫ��,��ֹSQLע�빥��
        /// </summary>
        /// <param name="str">Ҫ���˵��ַ�</param>
        /// <returns>����ȫ��ת������ַ�</returns>
        public static string ChangeFullAngle(string str)
        {
            string tempStr = str;
            if (string.IsNullOrEmpty(tempStr)) //����ַ���Ϊ�գ�ֱ�ӷ��ء�
            {
                return tempStr;
            }
            else
            {
                tempStr = str.ToLower();
                tempStr = str.Replace("'", "��");
                tempStr = str.Replace("--", "����");
                tempStr = str.Replace(";", "��");
                tempStr = str.Replace("exec", "�ţأţ�");
                tempStr = str.Replace("execute", "�ţأţãգԣ�");
                tempStr = str.Replace("declare", "�ģţạ̃��ң�");
                tempStr = str.Replace("update", "�գУģ��ԣ�");
                tempStr = str.Replace("delete", "�ģţ̣ţԣ�");
                tempStr = str.Replace("insert", "�ɣΣӣţң�");
                tempStr = str.Replace("select", "�ӣţ̣ţã�");
                tempStr = str.Replace("<", "��");
                tempStr = str.Replace(">", "��");
                tempStr = str.Replace("%", "��");
                tempStr = str.Replace(@"\", "��");
                tempStr = str.Replace(",", "��");
                tempStr = str.Replace(".", "��");
                tempStr = str.Replace("=", "����");
                tempStr = str.Replace("||", "����");
                tempStr = str.Replace("[", "��");
                tempStr = str.Replace("]", "��");
                tempStr = str.Replace("&", "��");
                tempStr = str.Replace("/", "��");
                tempStr = str.Replace("|", "��");
                tempStr = str.Replace("?", "��");
                tempStr = str.Replace("_", "��");

                return str;
            }
        }


        /// <summary>
        /// ���ĳ�ַ��Ƿ�ΪӢ����ĸ,����,�»���
        /// </summary>
        /// <param name="str">Ҫ�����ַ���</param>
        /// <returns>True��ʾ��Ӣ����ĸ,False��ʾ����Ӣ����ĸ</returns>
        public static bool CheckIsCharaterAndNumber(string str)
        {

            if (string.IsNullOrEmpty(str))
            {

                return false;

            }
            else
            {

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[0-9a-zA-Z_]+$");//������ʽ ��֤Ӣ�ġ����֡��»��ߺ͵�Regex(@"^[0-9a-zA-Z_]+$");--^[a-zA-Z0-9_\u4e00-\u9fa5]+$ 

                return reg.IsMatch(str);
            }


        }

        /// <summary>
        /// ���ĳ�ַ���ĳ�ַ����г��ֵĴ���
        /// </summary>
        /// <param name="checkStr">Ҫ�����ַ�,����"A"</param>
        /// <param name="str">Ҫ�����ַ���,����"AAABBAACCC"</param>
        /// <returns>���ش��ַ����ֵĴ���</returns>
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
        /// ���ֽ�����ȡ�ַ����ĳ���
        /// </summary>
        /// <param name="String">Ҫ������ַ���</param>
        /// <returns>�ַ������ֽ���</returns>
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
        /// ��ȡָ���ֽ������ַ���
        /// </summary>
        /// <param name="Str">ԭ�ַ���</param>
        /// <param name="Num">Ҫ��ȡ���ֽ���</param>
        /// <returns>��ȡ����ַ���</returns>
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
        /// ��ֹsqlע��
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
            string[] strReplace = { "��", "��", "����", "��", "�ţأţ�", "�ţأţãգԣ�", "�ģţạ̃��ң�", "�գУģ��ԣ�", "�ģţ̣ţԣ�", "�ɣΣӣţң�", "�ӣţ̣ţã�", "��" };
            for (int i = 0; i < strCheck.Length; i++)
            {
                inputName = Regex.Replace(inputName, strCheck[i], strReplace[i], RegexOptions.IgnoreCase);
            }
            return inputName;
        }

        #region//������ַ����б����ת����
        /// <summary>
        /// ������ַ����б����ת��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeStr(string str)
        {
            str = "" + str;//��ֹstrΪNULLʱ����
            str = str.Replace("&nbsp", "&amp;nbsp");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("'", "��");
            str = str.Replace("\"", "&quot;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br/>");
            return str;
        }
        #endregion

        #region//�Գ����ַ�������ʾʱ��ת����
        /// <summary>
        /// �Գ����ַ�������ʾʱ��ת��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecodeStr(string str)
        {
            str = "" + str;//��ֹstrΪNULLʱ����
            str = str.Replace("&amp;nbsp", "&nbsp");
            str = str.Replace("&nbsp;", " ");//�����ı���������Ŀո�ת����html���
            str = str.Replace("��", "'");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("<br/>", "\n");

            return str;
        }
        #endregion

        #region �����ַ������


        /// <summary>
        /// �Ƿ������ַ���
        /// </summary>
        /// <param name="inputData">�����ַ���</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Regex RegNumber = new Regex("^[0-9]+$");//������
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// �Ƿ������ַ��� �ɴ�������
        /// </summary>
        /// <param name="inputData">�����ַ���</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");//��������
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// �Ƿ��Ǹ�����
        /// </summary>
        /// <param name="inputData">�����ַ���</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");//С��
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// �Ƿ��Ǹ����� �ɴ�������
        /// </summary>
        /// <param name="inputData">�����ַ���</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //�ȼ���^[+-]?\d+[.]?\d+$
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region ���ļ��

        /// <summary>
        /// ����Ƿ��������ַ�
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsChinese(string inputData)
        {
            Regex RegChinese = new Regex("[\u4e00-\u9fa5]+");//����
            Match m = RegChinese.Match(inputData);
            return m.Success;
        }

        #endregion

        #region ����Ƿ��зǷ��ַ�
        /// <summary>
        /// ����Ƿ��зǷ��ַ�
        /// </summary>
        /// �������ߣ�LYT
        /// <param name="inputData">�����ַ���</param>
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

        #region �ʼ���ַ
        public static readonly Regex EmailRegex = new Regex(
            @"^([a-zA-Z0-9][_\.\-]*)+\@([A-Za-z0-9])+((\.|-|_)[A-Za-z0-9]+)*((\.[A-Za-z0-9]{2,15}){1,2})$",
            RegexOptions.Compiled);

        /// <summary>
        /// �жϺϷ���E-Mail
        /// </summary>
        /// <param name="email">Ҫ�����ַ���</param>
        /// <returns>True��ʾ�ǺϷ�Email,False��ʾ���ǺϷ�Email</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// �Ƿ����ʼ���ַ ͬIsValidEmail
        /// </summary>
        /// <param name="email">�����ַ���</param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return IsValidEmail(email);
        }
        /// <summary>
        /// �Ƿ����ʼ���ַ ͬIsValidEmail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsRealEmail(string email)
        {
            return IsValidEmail(email);
        }

        #endregion

        #region URL��ַ
        /// <summary>
        /// �Ƿ��Ǵ�http://����ַ
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsUrl(string inputData)
        {
            Regex RegUrl = new Regex("^http://([w-]+.)+[w-]+(/[w-./ %&=]*)$");//��http://����ַ
            Match m = RegUrl.Match(inputData);
            return m.Success;
        }
        #endregion

        #region �̶��绰
        /// <summary>
        /// �Ƿ��ǹ��ڵ绰����
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsTel(string inputData)
        {
            // Regex RegTel = new Regex(@"^(\d{3}-\d{8})$|^(\d{4}-\d{7})$|^(\d{11})$");//���ڵ绰���� ����ȷ��ʽΪ����XXXX-XXXXXXX������XXXX-XXXXXXXX������XXX-XXXXXXX���� ��XXX-XXXXXXXX������XXXXXXX������XXXXXXXX����
            //Regex RegTel = new Regex(@"^(\d{2,4}[-_����]?)?\d{3,8}([-_����]?\d{3,8})?([-_����]?\d{1,7})?$");
            Match m = RegTel.Match(inputData);
            return m.Success;
        }

        static readonly Regex RegTel = new Regex(@"^(\d{2,4}[-_����]?)?\d{3,8}([-_����]?\d{3,8})?([-_����]?\d{1,7})?$", RegexOptions.Compiled);

        #endregion

        #region QQ
        /// <summary>
        /// �Ƿ���QQ
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsQQ(string inputData)
        {
            Regex RegQQ = new Regex("^[1-9]*[1-9][0-9]*$");//ƥ����ѶQQ��
            Match m = RegQQ.Match(inputData);
            return m.Success;
        }
        #endregion

        #region ���֤
        /// <summary>
        /// �Ƿ��ǹ������֤��
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsIDCard(string inputData)
        {
            Regex RegIDCard = new Regex(@"(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$)|(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)");//ƥ��������֤�š�
            Match m = RegIDCard.Match(inputData);
            return m.Success;
        }
        #endregion

        #region �˺�
        /// <summary>
        /// �˺��Ƿ�Ϸ�����ĸ��ͷ�����֡�26��Ӣ����ĸ�����»������6-16λ���ַ���
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsUserName(string inputData)
        {
            Regex RegUserName = new Regex(@"^[a-zA-Z]\w{5,15}$");//ƥ������ĸ��ͷ�����֡�26��Ӣ����ĸ�����»������6-16λ���ַ���
            Match m = RegUserName.Match(inputData);
            return m.Success;
        }
        #endregion

        #region Ӣ����ĸ
        /// <summary>
        /// �Ƿ�����26��Ӣ����ĸ��ɵ��ַ���
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsEnglish(string inputData)
        {
            Regex RegEnglish = new Regex("^[A-Za-z]+$");//��26��Ӣ����ĸ��ɵ��ַ��� 
            Match m = RegEnglish.Match(inputData);
            return m.Success;
        }
        #endregion

        #region ����
        /// <summary>
        /// �Ƿ��п���
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsTrimRow(string inputData)
        {
            Regex RegTrimRow = new Regex(@"\n[\s| ]*\r");//����
            Match m = RegTrimRow.Match(inputData);
            return m.Success;
        }
        #endregion

        #region �ֻ�
        /// <summary>
        /// �Ƿ��ǹ����ֻ�
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsMobile(string inputData)
        {
            //Regex RegMobile = new Regex(@"^((\(\d{2,3}\))|(\d{3}\-))?1[3|5]\d{9}$");//�����ֻ�
            Match m = RegMobile.Match(inputData);
            return m.Success;
        }

        static readonly Regex RegMobile = new Regex(@"^((\(\d{2,3}\))|(\d{3}\-))?1[3|5|4|8|7]\d{9}$", RegexOptions.Compiled);//�����ֻ�

        #endregion

        #region  ����

        private static DateTime _TempDate;

        /// <summary>
        /// ����Ƿ�������
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsDate(string inputData)
        {
            return DateTime.TryParse(inputData, out _TempDate);

            // xuyc: �Ż�һ��Ч��
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

        #region  ����ַ�����󳤶ȣ�����ָ�����ȵĴ�

        /// <summary>
        /// ����ַ�����󳤶ȣ�����ָ�����ȵĴ�
        /// </summary>
        /// <param name="sqlInput">�����ַ���</param>
        /// <param name="maxLength">��󳤶�</param>
        /// <returns></returns>			
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//����󳤶Ƚ�ȡ�ַ���
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }


        #endregion

        #region//���<>�е�����
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

        #region ȥ�����һ������

        /// <summary>
        /// ȥ�����һ������
        /// </summary>
        /// <param name="str"></param>
        /// <returns>ȥ�����һ�����ŵ��ַ���</returns>
        public static string DelLastComma(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.IndexOf(",", System.StringComparison.Ordinal) == -1)
            {
                return str;
            }
            return str.Substring(0, str.LastIndexOf(",", System.StringComparison.Ordinal));
        }
        #endregion

        /// <summary>
        /// ����0,1,2������ת��Ϊ��Ӧ����ĸABC
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetChangedCharacter(int num)
        {
            var asciiEncoding = new System.Text.ASCIIEncoding();

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
        /// ��ȡָ���ֽ������ַ���
        /// </summary>
        /// <param name="Str">ԭ�ַ���</param>
        /// <param name="Num">Ҫ��ȡ���ֽ���</param>
        /// <returns>��ȡ����ַ���</returns>
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
            string[] paraString = url.Substring(url.IndexOf("?", System.StringComparison.Ordinal) + 1).Split('&');
            for (int i = 0; i < paraString.Length; i++)
            {
                if (paraString[i].Substring(0, paraString[i].IndexOf("=", System.StringComparison.Ordinal)) == paras)
                {
                    return paraString[i].Substring(paraString[i].IndexOf("=", System.StringComparison.Ordinal) + 1);
                }
            }
            return string.Empty;
        }

        public static List<string> ToLowerList(this IEnumerable<string> source)
        {
            var newlist = new List<string>(source);
            return newlist.ConvertAll(d => d.ToLower());
        }

        public static List<string> ToToUpperList(this IEnumerable<string> source)
        {
            var newlist = new List<string>(source);
            return newlist.ConvertAll(d => d.ToUpper());
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}