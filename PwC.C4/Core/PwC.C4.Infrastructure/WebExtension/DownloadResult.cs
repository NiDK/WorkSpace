using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PwC.C4.Infrastructure.WebExtension
{
    public class DownloadResult : ActionResult
    {

        public string FileName { get; set; }
        public MemoryStream Ms { get; set; }
        public string ContentType { get; set; }
        public string TempPath { get; set; }

        public DownloadResult(MemoryStream ms, string fileName, string contentType,string tempPath)
        {
            Ms = ms;
            ContentType = contentType;
            FileName = fileName;
            TempPath = tempPath;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            curContext.Response.Charset = "";
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = ContentType;

            #region 以下代码不堪入目

            var dumpFile = new FileStream(TempPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Ms.WriteTo(dumpFile);
            Ms.Close();
            Ms.Dispose();
            dumpFile.Close();
            dumpFile.Dispose();
            var fs = new FileStream(TempPath, FileMode.Open);
            var bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            #endregion

            curContext.Response.BinaryWrite(bytes);
            curContext.Response.Flush();
            curContext.Response.End();

        }
    }
}
