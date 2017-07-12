using System.IO;
using System.Web;
using PwC.C4.Dfs.Common.Model;

namespace PwC.C4.Dfs.Web.Services
{
    public class DfsHelper
    {
        public static bool ParseRequest(HttpContext context, 
                                        out string  keyspace, 
                                        out string  appcode,
                                        out string  fileId, 
                                        out string  extension)
        {
            keyspace = context.Request.QueryString["ks"];
            fileId = context.Request.QueryString["fid"];
            extension = context.Request.QueryString["ext"];
            appcode = context.Request.QueryString["a"];
            return (
                    !string.IsNullOrEmpty(appcode)
                    && !string.IsNullOrEmpty(keyspace)
                    && !string.IsNullOrEmpty(fileId)
                    && !string.IsNullOrEmpty(extension)
                );
        }

        public static string EncodeFileName(string agent, string file)
        {
            if (!string.IsNullOrWhiteSpace(agent) && agent.Contains("MSIE"))
            {
                return HttpUtility.UrlPathEncode(file);
            }
            return file;
        }

        public static void SendErrorResponse(HttpContext context, int status)
        {
            context.Response.Clear();
            context.Response.StatusCode = status;
        }


        public static DfsPath BuildPath(string keyspace, string appCode, string fileId, string extension)
        {
            return new DfsPath(keyspace, appCode, fileId, extension);
        }

        public static void Pipe(Stream input, Stream output)
        {
            var buffer = new byte[8192];

            long read = 0;
            while (read < input.Length)
            {
                int count = input.Read(buffer, 0, buffer.Length);
                output.Write(buffer, 0, count);
                read += count;
            }
        }
    }
}
