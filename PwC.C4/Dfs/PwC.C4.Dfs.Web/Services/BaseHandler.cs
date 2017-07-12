using System;
using System.Web;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Dfs.Web.Auth;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Web.Services
{
    public abstract class BaseHandler : IHttpHandler
    {
        private static LogWrapper _logger = new LogWrapper();

        #region Fields

        protected HttpContext _context;

        protected string _keyspace;
        protected string _appCode;
        protected string _file;
        protected string _extension;

        private string _originalUrl
        {
            get
            {
                var scheme = _context.Request.Url.Scheme;
                var host = _context.Request.Url.Host;
                var url = _context.Request.RawUrl;

                return string.Format("{0}://{1}{2}", scheme, host, url);
            }
        }

        private SecurityLevel _securityLevel
        {
            get
            {
                return DfsConfig.Instance.GetSecurityLevel(_keyspace, _appCode);
            }
        }

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            _context = context;

            if (Initialize() && CheckAccess())
            {
                HandleRequest();
            }
        }

        private bool Initialize()
        {
            bool valid = DfsHelper.ParseRequest(_context, out _keyspace, out _appCode, out _file, out _extension);
            if (!valid)
            {
                PerfCounters.Instance.CountInvalidRequest();
                DfsHelper.SendErrorResponse(_context, 400);
            }

            return valid;
        }

        private bool CheckAccess()
        {
            return Authorizator.CheckAccess(_securityLevel, _context, _originalUrl, _appCode);
        }

        protected abstract void HandleRequest();

        #region Helper

        protected void SetContentType(string extension)
        {
            var mime = DfsConfig.Instance.GetMimeType(extension);
            _context.Response.ContentType = mime;
        }

        protected void SetEncoding(string encoding)
        {
            if (!string.IsNullOrWhiteSpace(encoding))
            {
                _context.Response.Charset = encoding;
            }
        }

        protected void SetLastModified(DateTime timestamp)
        {
            var now = DateTime.UtcNow;

            var lastModified = timestamp;
            if (lastModified > now)
            {
                _logger.ErrorFormat("Invalid last modified time found, value: {0}, now: {1}, url: {2}", 
                    timestamp, now, _originalUrl);
                lastModified = now;
            }

            // work around for front-end caches, e.g. varnish
            _context.Response.Cache.SetLastModified(lastModified);
            _context.Response.Cache.SetMaxAge(TimeSpan.FromDays(365));
        }

        protected void SetAttachment(string file)
        {
            string encoded = DfsHelper.EncodeFileName(_context.Request.UserAgent, file);
            _context.Response.AddHeader("Content-Disposition", "Attachment; Filename=\"" + encoded + "\"");
        }

        #endregion

        public virtual bool IsReusable
        {
            get { return false; }
        }
    }
}
