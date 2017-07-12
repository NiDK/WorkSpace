using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Persistance;

namespace PwC.C4.DataService
{
    public class C4DataService : IC4DataService
    {

        public List<DataSourceObject> DataSourceObjects_Get(string appcode, string dataSourceType, string @group = "")
        {
            return DataSourceDao.GetDataSourceObjects(appcode, dataSourceType, group);
        }

        public List<DataSourceBase> DataSourceBase_Get(string appCode)
        {
            return DataSourceDao.GetDataSourceBase(appCode);
        }

        public List<DataSourceTypeInfo> DataSource_GetType(string appCode)
        {
            return DataSourceDao.GetDataSourceType(appCode);
        }

        public bool UpdateDataSourceType(DataSourceTypeInfo type)
        {
            if (type.Id == Guid.Empty)
                type.Id = Guid.NewGuid();
            return DataSourceDao.UpdateDataSourceType(type);
        }

        public bool UpdateDataSourceDetail(DataSourceDetail detail)
        {
            if (detail.Id == Guid.Empty)
                detail.Id = Guid.NewGuid();
            return DataSourceDao.UpdateDataSourceDetail(detail);
        }

        public bool DeleteDataSource(DataSourceDelete delete)
        {
            return DataSourceDao.DeleteDataSource(delete);
        }

        public List<DataSourceDetail> GetDataSourceDetails(string appCode, string code, string @group, Guid? typeId, int page, int size,
            out int totalCount)
        {
            return DataSourceDao.GetDataSourceDetails(appCode, code, @group, typeId, page, size, out totalCount);
        }

        public HtmlSnippet HtmlSnippet_Get(string appCode, string code)
        {
            return HtmlSnippetDao.GetHtmlSnippet(appCode, code);
        }

        public HtmlCategory GetHtmlCategory_ByCode(string appCode, string group, string code)
        {
            return HtmlCategoryDao.GetHtmlCategory_ByCode(appCode, group, code);
        }

        public List<HtmlCategory> GetHtmlCategory_ListByRole(string appCode, string group, List<string> roles)
        {
            return HtmlCategoryDao.GetHtmlCategory_ListByRoles(appCode, group, roles);
        }

        public HtmlCategory GetHtmlCategory_ById(string appCode, string group, Guid id)
        {
            return HtmlCategoryDao.GetHtmlCategory_ById(appCode, group, id);
        }

        public List<HtmlCategory> GetHtmlCategory_ListByAppCode(string appCode, string group)
        {
            return HtmlCategoryDao.GetHtmlCategory_ListByAppCode(appCode, group);
        }

        public List<HtmlCategory> GetHtmlCategory_ListByParentId(string appCode, string group, Guid parentId)
        {
            return HtmlCategoryDao.GetHtmlCategory_ListByParentId(appCode, group, parentId);
        }

        public bool HtmlCategory_Delete(string appCode, string group, List<Guid> ids, string modifyBy)
        {
            return HtmlCategoryDao.HtmlCategory_Delete(appCode, group, ids, modifyBy);
        }

        public int HtmlCategory_Update(HtmlCategory entity)
        {
            return HtmlCategoryDao.HtmlCategory_Update(entity);
        }

        public List<HtmlCategory> HtmlCategory_GetByGroup(string appCode, string group, List<Guid> collapseIds)
        {
            var list = GetHtmlCategory_ListByAppCode(appCode, group);

            if (collapseIds!=null && collapseIds.Any())
            {
                foreach (var collapse in list.Where(c => collapseIds.Contains(c.Id)))
                {
                    collapse.IsCollapse = false;
                }
                foreach (var collapse in list.Where(c => !collapseIds.Contains(c.Id)))
                {
                    collapse.IsCollapse = true;
                }
            }

            var topCategories = list.Where(c => c.ParentId == Guid.Empty).OrderBy(c=>c.Order);

            foreach (var htmlCategory in topCategories)
            {
                htmlCategory.Level = 0;
                GetSubItems(list, htmlCategory);
            }

            return topCategories.ToList();
        }

        private void GetSubItems(List<HtmlCategory> oList, HtmlCategory html, int menuLevel = 1)
        {
            html.SubCategories = oList.Where(c => c.ParentId == html.Id).OrderBy(c => c.Order).ToList();
            foreach (var htmlCategory in html.SubCategories)
            {
                htmlCategory.Level = menuLevel;
            }
            if (html.SubCategories.Any())
            {
                foreach (var htmlCategory in html.SubCategories)
                {
                    GetSubItems(oList, htmlCategory, menuLevel + 1);
                }
            }

        }
    }
}
