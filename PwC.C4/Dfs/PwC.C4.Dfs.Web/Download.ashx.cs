using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Web;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Web.Services;
using PwC.C4.Infrastructure.Logger;
using Executor = PwC.C4.Dfs.Web.Services.Executor;

namespace PwC.C4.Dfs.Web
{
    public class Download : BaseHandler
    {
        private static LogWrapper _logger = new LogWrapper();

        protected override void HandleRequest()
        {
            Executor.Execute(
                () =>
                {
                    var path = DfsHelper.BuildPath(_keyspace, _appCode, _file, _extension);
                    DownloadImpl(_context, path);
                },
                ex =>
                {
                    _logger.HandleException(ex, "Download");
                    DfsHelper.SendErrorResponse(_context, 500);
                });
        }

        private void DownloadImpl(HttpContext context, DfsPath path)
        {
            var start = Stopwatch.GetTimestamp();
            var item = Dfs.Client.Dfs.Get(path);
            var end = Stopwatch.GetTimestamp();

            Executor.Execute(
                item != null,
                () =>
                {
                    PerfCounters.Instance.CountDownload(item.Length, end - start);

                    SetAttachment(item.FileName);
                    SetContentType(item.FileExtension);
                    SetEncoding(item.Encoding);

                    DfsHelper.Pipe(item.FileDataStream, context.Response.OutputStream);
                },
                () =>
                {
                    PerfCounters.Instance.CountFileNotFound();
                    DfsHelper.SendErrorResponse(context, 404);
                });
        }
    }
}
