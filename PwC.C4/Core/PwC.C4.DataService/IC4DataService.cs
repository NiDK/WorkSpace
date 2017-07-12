using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;

namespace PwC.C4.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IC4DataService
    {


        #region Datasource

        [OperationContract]
        List<DataSourceObject> DataSourceObjects_Get(string appcode, string dataSourceType, string group = "");

        [OperationContract]
        List<DataSourceBase> DataSourceBase_Get(string appCode);

        [OperationContract]
        List<DataSourceTypeInfo> DataSource_GetType(string appCode);

        [OperationContract]
        bool UpdateDataSourceType(DataSourceTypeInfo type);

        [OperationContract]
        bool UpdateDataSourceDetail(DataSourceDetail detail);

        [OperationContract]
        bool DeleteDataSource(DataSourceDelete delete);

        [OperationContract]
        List<DataSourceDetail> GetDataSourceDetails(string appCode, string code, string group, Guid? typeId,
            int page, int size,out int totalCount);
        #endregion

        [OperationContract]
        HtmlSnippet HtmlSnippet_Get(string appCode, string code);

        #region HtmlCategory

        [OperationContract]
        HtmlCategory GetHtmlCategory_ByCode(string appCode, string group, string code);

        [OperationContract]
        HtmlCategory GetHtmlCategory_ById(string appCode, string group, Guid id);

        [OperationContract]
        List<HtmlCategory> GetHtmlCategory_ListByAppCode(string appCode, string group);

        [OperationContract]
        List<HtmlCategory> GetHtmlCategory_ListByRole(string appCode, string group, List<string> roles);

        [OperationContract]
        List<HtmlCategory> GetHtmlCategory_ListByParentId(string appCode, string group, Guid parentId);

        [OperationContract]
        bool HtmlCategory_Delete(string appCode, string group, List<Guid> ids, string modifyBy);

        [OperationContract]
        int HtmlCategory_Update(HtmlCategory entity);

        [OperationContract]
        List<HtmlCategory> HtmlCategory_GetByGroup(string appCode, string group, List<Guid> collapseIds);

        #endregion

    }


}
