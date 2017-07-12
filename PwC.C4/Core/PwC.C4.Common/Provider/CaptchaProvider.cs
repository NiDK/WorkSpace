
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using PwC.C4.Common.Service;

namespace PwC.C4.Common.Provider
{
    public static class CaptchaProvider
    {


        public static Bitmap CreateCaptcha(string code, int padding = 2, int fontSize = 12)
        {
            var passcode = new CaptchaService {Padding = padding, FontSize = fontSize};
            if (string.IsNullOrEmpty(code))
            {
                code = passcode.CreateVerifyCode();
            }
            var image = passcode.CreateImageCode(code);
            return image;
        }


        public static string CreateCaptchaCode(int length = 4)
        {
            var passcode = new CaptchaService();
            return passcode.CreateVerifyCode(length);
        }

    }
}
