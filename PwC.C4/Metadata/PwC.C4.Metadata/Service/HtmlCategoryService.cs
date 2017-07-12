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
using PwC.C4.Infrastructure.Cache;

namespace PwC.C4.Metadata.Service
{
    public class HtmlCategoryService : IHtmlCategoryService
    {
        readonly LogWrapper _log = new LogWrapper();
        private static C4DataServiceClient _c4Client = null;
        private static string _appCode = null;
        private static readonly string CategoryStatusFix = "categoryStatus-";

        #region Singleton

        private static HtmlCategoryService _instance = null;
        private static readonly object LockHelper = new object();

        public HtmlCategoryService()
        {
        }

        public static IHtmlCategoryService Instance()
        {
            if (_instance == null || _c4Client == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _c4Client != null && _appCode == null) return _instance;
                    _instance = new HtmlCategoryService();
                    _c4Client = new C4DataServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                }
            }
            return _instance;
        }

        #endregion

        public HtmlCategory GetHtmlCategory_ByCode(string @group, string code)
        {
            return _c4Client.GetHtmlCategory_ByCode(_appCode, group, code);
        }

        public HtmlCategory GetHtmlCategory_ById(string @group, Guid id)
        {
            return _c4Client.GetHtmlCategory_ById(_appCode, group, id);
        }

        public List<HtmlCategory> GetHtmlCategory_ListByAppCode(string @group)
        {
            return _c4Client.GetHtmlCategory_ListByAppCode(_appCode, group);
        }

        public List<HtmlCategory> GetHtmlCategory_ListByAppCode(string @group,List<string> roles)
        {
            return _c4Client.GetHtmlCategory_ListByRole(_appCode, group, roles);
        }

        public List<HtmlCategory> GetHtmlCategory_ListByParentId( string @group, Guid parentId)
        {
            return _c4Client.GetHtmlCategory_ListByParentId(_appCode, group, parentId);
        }

        public bool HtmlCategory_Delete(string @group, List<Guid> ids, string modifyBy)
        {
            return _c4Client.HtmlCategory_Delete(_appCode, group, ids, modifyBy);
        }

        public int HtmlCategory_Update(HtmlCategory entity)
        {
            return _c4Client.HtmlCategory_Update(entity);
        }


        public void UpdateCategoryCollaspseStatus(string key, List<Guid> collapseIds)
        {
            Preference.Set(CategoryStatusFix + key, collapseIds);
        }

        public List<HtmlCategory> HtmlCategory_GetByGroup(string group,string key = null)
        {
            var collapseIds = new List<Guid>();
            if (!string.IsNullOrEmpty(key))
            {
                collapseIds = Preference.Get<List<Guid>>(CategoryStatusFix + key);
            }
            return _c4Client.HtmlCategory_GetByGroup(_appCode, group, collapseIds);
        }
    }
}
