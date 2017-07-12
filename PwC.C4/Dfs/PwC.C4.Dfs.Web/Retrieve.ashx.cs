using System.Diagnostics;
using System.Web;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Web.Services;
using PwC.C4.Infrastructure.Logger;
namespace PwC.C4.Dfs.Web
{
    public class Retrieve : BaseHandler
    {
        private static readonly LogWrapper _logger = new LogWrapper();

        protected override void HandleRequest()
        {
            Executor.Execute(
                () =>
                {
                    var path = DfsHelper.BuildPath(_keyspace, _appCode, _file, _extension);
                    RetrieveImpl(_context, path);
                },
                ex =>
                {
                    _logger.HandleException(ex, "Retrieve");
                    DfsHelper.SendErrorResponse(_context, 500);
                });
        }

        private void RetrieveImpl(HttpContext context, DfsPath path)
        {
            var start = Stopwatch.GetTimestamp();
            var item = Dfs.Client.Dfs.Get(path);
            var end = Stopwatch.GetTimestamp();

            Executor.Execute(
                item != null,
                () =>
                {
                    PerfCounters.Instance.CountDownload(item.Length, end - start);

                    SetContentType(item.FileExtension);
                    SetEncoding(item.Encoding);
                    SetLastModified(item.Timestamp);

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
