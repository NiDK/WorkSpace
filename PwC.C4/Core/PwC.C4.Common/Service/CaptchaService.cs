using System;
using System.Collections.Generic;
using System.Drawing;

namespace PwC.C4.Common.Service
{
    public class CaptchaService
    {
        #region 验证码长度
        int length = 4;
        /// <summary>
        /// 验证码长度
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        #endregion

        #region 验证码字体大小
        int fontSize = 12;
        /// <summary>
        /// 验证码字体大小
        /// </summary>
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int padding = 2;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool chaos = true;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color chaosColor = Color.LightGray;
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        public Color[] Colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

        #endregion

        #region 自定义字体数组
        /// <summary>
        /// 自定义字体数组
        /// </summary>
        public string[] Fonts = { "Arial", "Georgia" };
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)

        /// <summary>
        /// 自定义随机码字符串序列(使用逗号分隔)
        /// </summary>
        public string[] CodeSerial =
        {
            "3", "4", "5", "6", "7", "8", "a", "b", "c", "d", "e", "f", "h", "j", "k",
            "m", "n", "p", "r", "t", "u", "w", "x", "y"
        };
        #endregion

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片 
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            //// 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * j) / dBaseAxisLen : (PI2 * i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
        #endregion

        #region 生成校验码图片
        /// <summary>
        /// 生成校验码图片
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight))
            {

                Graphics g = Graphics.FromImage(image);

                g.Clear(BackgroundColor);

                MyRandom rand = new MyRandom();

                //给背景添加随机生成的燥点
                if (this.Chaos)
                {

                    Pen pen = new Pen(ChaosColor, 0);
                    int c = Length * 10;

                    for (int i = 0; i < c; i++)
                    {
                        int x = rand.Next(image.Width);
                        int y = rand.Next(image.Height);

                        g.DrawRectangle(pen, x, y, 1, 1);
                    }
                }

                int left = 0, top = 0, top1 = 1, top2 = 1;

                int n1 = (imageHeight - FontSize - Padding * 2);
                int n2 = n1 / 4;
                top1 = n2;
                top2 = n2 * 2;

                int cindex, findex;

                //随机字体和颜色的验证码字符
                for (int i = 0; i < code.Length; i++)
                {
                    cindex = rand.Next(Colors.Length - 1);
                    findex = rand.Next(Fonts.Length - 1);

                    using (Font f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))
                    {
                        using (Brush b = new System.Drawing.SolidBrush(Colors[cindex]))
                        {
                            if (i % 2 == 1)
                            {
                                top = top2;
                            }
                            else
                            {
                                top = top1;
                            }

                            left = i * fWidth;

                            g.DrawString(code.Substring(i, 1), f, b, left, top);
                        }
                    }
                }

                //画一个边框 边框颜色为Color.Gainsboro
                using (Pen PP = new Pen(Color.Gainsboro, 0))
                {
                    g.DrawRectangle(PP, 0, 0, image.Width - 1, image.Height - 1);
                }
                g.Dispose();

                //产生波形 
                return TwistImage(image, true, 3, 4); ;
            }
        }
        #endregion

        #region 生成随机字符码
        /// <summary>
        /// 生成随机字符码
        /// </summary>
        /// <param name="codeLen"></param>
        /// <returns></returns>
        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, CodeSerial.Length - 1);

                code += CodeSerial[randValue];
            }

            return code;
        }
        /// <summary>
        /// 生成随机字符码
        /// </summary>
        /// <returns></returns>
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class MyRandom : System.Random
    {
        private List<int> list = new List<int>();

        /// <summary>
        /// 返回一个有限，非负数。
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public override int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
        /// <summary>
        /// 返回一个有限，非负数。
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public override int Next(int minValue, int maxValue)
        {
            if (minValue == maxValue)
                return minValue;

            int next = base.Next(minValue, maxValue);

            if (list.Count >= maxValue - minValue)
                return next;

            if (list.Contains(next))
                return Next(minValue, maxValue);

            list.Add(next);

            return next;
        }
    }
}
