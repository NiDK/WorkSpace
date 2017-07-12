using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using PwC.C4.Common.Model;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Helper;

namespace System.Web.Mvc
{
    public class ImageCheck : ActionResult
    {

        string _backimage;
        string _cookieName;
        private UserSession _session;
        public ImageCheck(string backimage, string cookieName)
        { 
        _backimage=backimage;
        _cookieName = cookieName;
        }
        public ImageCheck(string backimage)
        {
            _backimage = backimage;
        }
        public ImageCheck(string backimage,UserSession session)
        {
            _backimage = backimage;
            _session = session;
        }
        /// <summary>ImageCheck
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
        {
            int number;
            char code;
            string checkCode = String.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('1' + (char)(number % 9));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        /// <param name="Backgrund">背景图片位置</param>
        /// @"G:\工作文档\常用\UED页面\验证码背景图.gif"
        public void CreateValidateGraphic(string validateCode, HttpResponseBase response,string Backgrund)
        {
            Bitmap image = new Bitmap(90, 27);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                Image backgruound = Image.FromFile(Backgrund);
                g.DrawImage(backgruound, new Point());
                Font font = new Font("Snap ITC", 17f, (FontStyle.Bold));
                LinearGradientBrush brush = new  LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.FromArgb(148, 87, 0), Color.FromArgb(148, 87, 0), 1.2f, true);//过度色
                g.DrawString(validateCode, font, brush, 0, 0);
                //画图片的边框线         
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Gif);
                response.ClearContent();
                response.ContentType = "image/gif";
                response.BinaryWrite(stream.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
          if(_session==null)
              _session=new UserSession(){SessionKey = Guid.NewGuid().ToString()};
            string code = CreateValidateCode(4);
            if (string.IsNullOrEmpty(_cookieName))
            {
                _cookieName = "VK_";
            }
            HttpCookie cookie = new HttpCookie(_cookieName);
            string eNcode = EncryptHelper.Encode(code);
            cookie.Value = eNcode;
            context.HttpContext.Response.Cookies.Add(cookie);
            _session.ValidataCode = eNcode;
            _session.LastActiveTime = DateTime.Now;
            Preference.Set(_session.SessionKey,_session);
            CreateValidateGraphic(code, context.HttpContext.Response, _backimage);
        }
    }
}
