using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Infrastructure.Helper
{
    public static class EncryptHelper
    {
        private static string _key64;
        private static string _iv64;

        private static void InitKey()
        {
            if (_key64 == null || _key64.Length != 8)
            {
                _key64 = string.Format("{0}-{1}", "C4",
                    AppSettings.Instance.GetNode(ConfigConstValues.SystemNodeName, "EncryptKey").Value);
                if (_key64.Length == 8) return;
                if (_key64.Length > 8)
                    _key64 = _key64.Substring(0, 8);
                else
                {
                    _key64 = _key64 + "C4C4C4C4";
                    _key64 = _key64.Substring(0, 8);
                }

            }
            if (_iv64 == null || _iv64.Length != 8)
            {
                _iv64 = string.Format("{0}-{1}", "C4",
                    AppSettings.Instance.GetNode(ConfigConstValues.SystemNodeName, "EncryptIV").Value);
                if (_iv64.Length == 8) return;
                if (_iv64.Length > 8)
                    _iv64 = _iv64.Substring(0, 8);
                else
                {
                    _iv64 = _iv64 + "C4C4C4C4";
                    _iv64 = _iv64.Substring(0, 8);
                }
            }
        }

        public static string Encode(string data)
        {

            InitKey();
            byte[] byKey = System.Text.Encoding.ASCII.GetBytes(_key64);
            byte[] byIv = System.Text.Encoding.ASCII.GetBytes(_iv64);
            var cryptoProvider = new DESCryptoServiceProvider();

            var ms = new MemoryStream();
            var cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIv), CryptoStreamMode.Write);
            var sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int) ms.Length);
        }

        public static string Decode(string data)
        {
            InitKey();
            byte[] byKey = System.Text.Encoding.ASCII.GetBytes(_key64);
            byte[] byIv = System.Text.Encoding.ASCII.GetBytes(_iv64);
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream(byEnc);
            var cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIv), CryptoStreamMode.Read);
            var sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
    }
}
