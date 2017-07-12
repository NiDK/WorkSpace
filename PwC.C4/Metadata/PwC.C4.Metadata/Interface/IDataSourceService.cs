using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Interface
{
    public interface IDataSourceService
    {
        List<DataSourceObject> GetDataSourceBy(DataSourceType type, string key, string group = "");

        List<DataSourceObject> GetSystemDataSource(string code, string group = "");

        List<DataSourceObject> GetMappingDataSource(string code, string group = "");

        List<DataSourceObject> GetInterfaceDataSource(string assembly);

        Dictionary<string, Dictionary<string, string>> GetAppDataSource();

        bool UpdateDataSourceType(DataSourceTypeInfo type);

        bool UpdateDateSourceDetail(DataSourceDetail detail);

        List<DataSourceTypeInfo> GetDataSourceTypeList();

        bool DeleteDataSource(DataSource type, Guid id, string modifyBy);

        List<DataSourceDetail> GetDataSourceDetailsByPaging(string typeName, out int totalCount, string appcode = null,
            int index = 0, int size = -1,
            string group = "");

        List<DataSourceDetail> GetDataSourceDetailsByPaging(Guid typeId, out int totalCount, string appcode = null,
            int index = 0, int size = -1,
            string group = "");

    }
}
