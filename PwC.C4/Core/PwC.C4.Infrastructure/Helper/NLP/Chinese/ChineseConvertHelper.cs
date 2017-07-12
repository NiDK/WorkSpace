using System;
using System.Text;
using System.Text.RegularExpressions;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.BaseLogger;

namespace PwC.C4.Infrastructure.Helper.NLP.Chinese
{

    
    ///<summary>
    /// ���ļ��廥ת��
    ///</summary>
    public class ChineseConvertHelper
    {
        private static Regex reg = new Regex("^[\u4e00-\u9fa5]$"); //��֤�Ƿ����뺺��  

        private static readonly LogWrapper Logger = new LogWrapper();

		
        ///<summary>
        /// ����ת����
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

				if( ChineseDict.Instance.GetTraditionalDic().ContainsKey(str)) //�����ǰ�ַ��Ƿ����֣�ת�ɼ�����
				{
					result.Append(ChineseDict.Instance.GetTraditionalDic()[str]);
				}
				else
				{
					result.Append(str);
					//���������Ǻ���
					if (reg.IsMatch(str))
					{
						Logger.Info(String.Format("ָ���ķ�����[{0}]û���ҵ���Ӧ�ļ�����", str));
					}
				}

            }
            return result.ToString();


        }


		
		
        ///<summary>
        /// ����ת����--����ת����᲻̫׼ȷ����Ϊһ�������ֿ��ܶ�Ӧһ�����ϵķ����֡�
        /// �� ������ ת�� "�l","�"
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

				if (ChineseDict.Instance.GetSimplifiedDic().ContainsKey(str)) //�����ǰ�ַ��Ǽ����֣�ת�ɷ�����
				{
					result.Append(ChineseDict.Instance.GetSimplifiedDic()[str]);
				}
                else
                {
                    result.Append(str);
                    //���������Ǻ���
                    if (reg.IsMatch(str))
                    {
                        Logger.Info(String.Format("ָ���ļ�����[{0}]û���ҵ���Ӧ�ķ�����", str));
                    }
                }
            }
            return result.ToString();
        }
    }
}