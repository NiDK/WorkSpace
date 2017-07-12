using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PwC.C4.DataService.Model;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Interface
{
    public interface IHtmlCategoryService
    {

        HtmlCategory GetHtmlCategory_ByCode(string group, string code);

        HtmlCategory GetHtmlCategory_ById(string group, Guid id);

        List<HtmlCategory> GetHtmlCategory_ListByAppCode(string group);

        List<HtmlCategory> GetHtmlCategory_ListByAppCode(string @group, List<string> roles);

        List<HtmlCategory> GetHtmlCategory_ListByParentId(string group, Guid parentId);

        bool HtmlCategory_Delete(string group, List<Guid> ids, string modifyBy);

        int HtmlCategory_Update(HtmlCategory entity);

        List<HtmlCategory> HtmlCategory_GetByGroup(string group, string key = null);

        void UpdateCategoryCollaspseStatus(string key, List<Guid> collapseIds);
    }
}
