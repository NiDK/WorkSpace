using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Interface;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.DataService.Model;

namespace PwC.C4.Metadata.Service
{
    public class HtmlSnippetService : IHtmlSnippetService
    {
        readonly LogWrapper _log= new LogWrapper();
        private static C4DataServiceClient _c4Client = null;
        private static string _appCode = null;

        #region Singleton

        private static HtmlSnippetService _instance = null;
        private static readonly object LockHelper = new object();

        public HtmlSnippetService()
        {
        }

        public static IHtmlSnippetService Instance()
        {
            if (_instance == null || _c4Client == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _c4Client != null && _appCode == null) return _instance;
                    _instance = new HtmlSnippetService();
                    _c4Client = new C4DataServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                }
            }
            return _instance;
        }

        #endregion

        public HtmlSnippet GetHtmlSnippet(string code)
        {
            return _c4Client.HtmlSnippet_Get(_appCode, code);
        }
    }
}
