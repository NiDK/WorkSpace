using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Membership.WebExtension;
using PwC.C4.Rush.Models;
using PwC.C4.TemplateEngine.ServiceImp;

namespace PwC.C4.Rush.Areas.App.Controllers
{
    [AllowAnonymous]
    [RouteArea("App", AreaPrefix = "Apps")]
    public class DefaultController : C4Controller
    {
        [Route("{aliasName}/{dataId}")]
        public ActionResult Index(string aliasName, string dataId)
        {
            var client = new RushServiceClient();
            ViewBag.AppName = aliasName;
            ViewBag.DataId = dataId;
            var form = client.RenderForm(dataId, aliasName, null);
            return View("~/Views/Shared/Index.cshtml", form);
        }

        [Route("{aliasName}/LinkTracking")]
        public ActionResult LinkTracking(string notesUrl)
        {
            try
            {
                var client = new RushServiceClient();
                var url = client.GetLinkTrackingUrl(notesUrl);
                if (url.StartsWith("Notes"))
                {
                    //Response.Redirect(notesUrl);
                    return View();
                }
                Response.Redirect(url);
                return new EmptyResult(); 
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Route("{aliasName}/DownloadFile")]
        public FileStreamResult DownloadFile(string aliasName, string fileid)
        {
            try
            {

                var client = new RushServiceClient();
                var tuple = client.DownloadFile(aliasName, fileid);

                var fileconentType = tuple.Item2;
                var filename = tuple.Item1;


                if (tuple.Item3 != null)
                {
                    var downloadLink = "";
                    downloadLink = AppDomain.CurrentDomain.BaseDirectory + AppSettings.Instance.GetDownloadLink();
                    string absoluFilePath = Path.Combine(downloadLink, fileid + fileconentType);

                    ////not exist directory, create.
                    if (!System.IO.File.Exists(downloadLink))
                    {
                        System.IO.Directory.CreateDirectory(downloadLink);
                    }

                    if (!System.IO.File.Exists(absoluFilePath))
                    {
                        FileStream fs = new FileStream(absoluFilePath, FileMode.OpenOrCreate);
                        tuple.Item3.WriteTo(fs);
                        fs.Flush();
                        fs.Dispose();
                    }
                    var mime = MimeMapping.GetMimeMapping(fileconentType);
                    if (string.IsNullOrEmpty(mime))
                    {
                        mime = "application/octet-stream";
                    }
                    var fsfromfile = new FileStream(absoluFilePath, FileMode.Open);
                    return File(fsfromfile, mime, Server.UrlEncode(filename));

                }
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Route("{aliasName}/DownloadContentFile")]
        public FileStreamResult DownloadContentFile(string aliasName, string fileid)
        {
            try
            {

                var client = new RushServiceClient();
                var tuple = client.DownloadContentFile(aliasName,fileid);

                var fileconentType = tuple.Item2;
                var filename = tuple.Item1;


                var downloadLink = AppDomain.CurrentDomain.BaseDirectory + "\\" + AppSettings.Instance.GetDownloadLink() + "\\" + aliasName;
                filename = Regex.Replace(filename, ".+\\\\", "", RegexOptions.IgnoreCase);
                if (!Directory.Exists(downloadLink))
                {
                    Directory.CreateDirectory(downloadLink);
                }

                var filearray = filename.Split('.');
                var fileextension = "";
                if (filearray.Length > 1)
                {
                    fileextension = filearray[1];
                }
                var guidfilename = fileid + (string.IsNullOrEmpty(fileextension) ? "" : ('.' + fileextension));

                var savePath = downloadLink + guidfilename;

                if (!System.IO.File.Exists(savePath))
                {
                    FileStream streamm = new FileStream(downloadLink + guidfilename, FileMode.Create);
                    MemoryStream memorystream = tuple.Item3;
                    memorystream.WriteTo(streamm);

                    var downloadfilename = filename;
                    streamm.Close();
                }
               
                if (System.IO.File.Exists(savePath))
                {
                    var fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                    return File(fs, fileconentType, filename);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}