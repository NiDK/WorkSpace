using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageResizer;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Web.ApiHelper.Service
{
    public static class ImageResizeService
    {
        public static string GetImageBase64(byte[] data,string iconSetting)
        {
            var ins = StreamHelper.BytesToStream(data);
            ins.Seek(0, 0);
            Stream st = new MemoryStream(); ;
            var i = new ImageJob(ins, st, new Instructions(iconSetting));
            i.Build();
            return Convert.ToBase64String(StreamHelper.StreamToBytes(st));
        }
    }
}
