using System;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Exceptions;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Common.Model
{
    /// <summary>
    /// Dfs Path Format:
    /// dfs://{keyspace}/{appcode}/{file id}.{file extension}
    /// </summary>
    /// <example>
    /// dfs://image/AppCode/b97c0a5a349e0288ba15b07cd3778a70.jpg
    /// dfs://report/AppCode/b97c0a5a349e0288ba15b07cd3778a70.pdf
    /// dfs://audio/AppCode/b97c0a5a349e0288ba15b07cd3778a70.mp3
    /// dfs://video/AppCode/b97c0a5a349e0288ba15b07cd3778a70.avi
    /// dfs://flash/AppCode/b97c0a5a349e0288ba15b07cd3778a70.swf
    /// </example>
    public class DfsPath
    {
        public string Keyspace { get; set; }
        public string AppCode { get; set; }
        public string FileId { get; set; }
        public string FileExtension { get; set; }

        #region ctor

        public DfsPath() { }

        public DfsPath(string keyspace, string appCode, string fileId, string fileExtension)
        {
            ArgumentHelper.AssertValuesNotEmpty(keyspace, fileId, fileExtension);

            this.Keyspace = keyspace;
            this.AppCode = appCode;
            this.FileId = fileId;
            this.FileExtension = fileExtension;
        }

        internal DfsPath Clone()
        {
            return new DfsPath(Keyspace, AppCode, FileId, FileExtension);
        }

        #endregion

        public string ToClientUrl()
        {
            string pathString = string.Format("{0}/{1}/{2}.{3}", Keyspace, AppCode, FileId, FileExtension);
            return new Uri(DfsConfig.Instance.GetClientUrlDomain(Keyspace), pathString).ToString();
        }

        public string ToDownloadUrl()
        {
            string pathString = string.Format("download/{0}/{1}/{2}.{3}", Keyspace, AppCode, FileId, FileExtension);
            return new Uri(DfsConfig.Instance.GetClientUrlDomain(Keyspace), pathString).ToString();
        }

        public override string ToString()
        {
            return string.Format("dfs://{0}/{1}/{2}.{3}", Keyspace, AppCode, FileId, FileExtension);
        }

        public static bool TryParse(string path, out DfsPath dfsPath)
        {
            ArgumentHelper.AssertNotEmpty(path);

            string[] segments = path.Split(new char[] { ':', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length == 5 && segments[0] == "dfs" && !string.IsNullOrEmpty(segments[2]))
            {
                dfsPath = new DfsPath(segments[1], segments[2], segments[3], segments[4]);
                return true;
            }

            dfsPath = null;
            return false;
        }

        public static DfsPath Parse(string path)
        {
            ArgumentHelper.AssertNotEmpty(path);
            DfsPath result;
            if (TryParse(path, out result))
                return result;
            throw new DfsException("Invalid DfsPath: " + path);
        }

        public static DfsPath FromUrl(string url)
        {
            ArgumentHelper.AssertNotEmpty(url, "url");

            try
            {
                var uri = new Uri(url);
                var path = "dfs://" + string.Join("", uri.Segments, uri.Segments.Length - 3, 3);
                return Parse(path);
            }
            catch (Exception ex)
            {
                throw new DfsException("Invalid url: " + url, ex);
            }
        }
    }
}
