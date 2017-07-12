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
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.Metadata.Service
{
    public class DataSourceService:IDataSourceService
    {
        readonly LogWrapper _log= new LogWrapper();
        private static Dictionary<string, List<DataSourceObject>> _dataSouceDic;
        private static C4DataServiceClient _c4Client = null;
        private static string _appCode = null;

        #region Singleton

        private static DataSourceService _instance = null;
        private static readonly object LockHelper = new object();

        public DataSourceService()
        {
        }

        public static IDataSourceService Instance()
        {
            if (_instance == null || _c4Client == null || _appCode == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null && _c4Client != null && _appCode != null) return _instance;
                    _instance = new DataSourceService();
                    _c4Client = new C4DataServiceClient();
                    _appCode = AppSettings.Instance.GetAppCode();
                }
            }
            return _instance;
        }
#if DEBUG

        public static DataSourceService DebugInstance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new DataSourceService();
                }
            }
            return _instance;
        }

#endif

        #endregion

        public List<DataSourceObject> GetDataSourceBy(DataSourceType type, string key, string group = "")
        {
            try
            {
                var dicKey = $"{type}-{key}-{@group}";
                var data = GetDataSourceFromCache(dicKey);
                if (data != null && data.Count > 0 && type != DataSourceType.Interface)
                    return data;
                else
                {
                    switch (type)
                    {
                        case DataSourceType.System:
                            data = GetSystemDataSource(key, group);
                            break;
                        case DataSourceType.Mapping:
                            data = GetMappingDataSource(key, group);
                            break;
                        case DataSourceType.Interface:
                            data = GetInterfaceDataSource(key);
                            break;
                        default:
                            data = new List<DataSourceObject>();
                            break;

                    }
                    if (data != null)
                    {
                        if (_dataSouceDic.ContainsKey(dicKey))
                        {
                            _dataSouceDic[dicKey] = data;
                        }
                        else
                        {
                            _dataSouceDic.Add(dicKey, data);
                        }
                    }
                    return data;
                }
            }
            catch (Exception ee)
            {
                var msg = "GetDataSource Error,key:" + key + ",Group:" + group;
                _log.Error(msg, ee);
                throw new GetDataSourceException(msg, ee);
            }


        }

        public List<DataSourceObject> GetSystemDataSource(string code, string group = "")
        {
            throw new NotImplementedException();
        }

        public List<DataSourceObject> GetMappingDataSource(string code, string group = "")
        {
            return _c4Client.DataSourceObjects_Get(_appCode,code, group);
        }

        public List<DataSourceObject> GetInterfaceDataSource(string assemblyFullname)
        {
            var assemblyInfo = assemblyFullname.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            if (assemblyInfo.Count() != 2) return new List<DataSourceObject>();
            IDataSource datasource = null;
            var assembly = Assembly.Load(assemblyInfo[0]);
            var types = assembly.GetTypes();
            foreach (var type in types.Where(type => type.FullName == assemblyInfo[1]))
            {
                datasource = (IDataSource) Activator.CreateInstance(type);
            }
            if (datasource != null)
            {
                var context = new DataSourceContext() {AppCode = AppSettings.Instance.GetAppCode()};
                var data = datasource.GetDataSource(context);
                return data;
            }
            else
            {
                return new List<DataSourceObject>();
            }
        }

        public Dictionary<string, Dictionary<string, string>> GetAppDataSource()
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var baseData = _c4Client.DataSourceBase_Get(_appCode);

            var typeList = baseData.Select(c => c.Name).Distinct().ToList();
            typeList.ForEach(n =>
            {
                var dic = baseData.Where(c => c.Name == n).ToDictionary(c => c.Key, c => c.Value);
                result.Add(n, dic);
            });
            return result;
        }

        public bool UpdateDataSourceType(DataSourceTypeInfo type)
        {
            type.AppCode = _appCode;
            return _c4Client.UpdateDataSourceType(type);
        }

        public bool UpdateDateSourceDetail(DataSourceDetail detail)
        {
            if (string.IsNullOrEmpty(detail.DataSourceTypeName) && detail.DataSourceTypeId!=Guid.Empty)
            {
                var types = _c4Client.DataSource_GetType(_appCode);
                if (types != null && types.Any())
                {
                    detail.DataSourceTypeName =
                    types.Where(c => c.Id == detail.DataSourceTypeId).Select(c => c.Name).FirstOrDefault();
                }
            }
            var dicKey = $"{DataSourceType.Mapping}-{detail.DataSourceTypeName}-{detail.Group}";
            RemoveDataSourceFromCache(dicKey);
            detail.AppCode = _appCode;
            return _c4Client.UpdateDataSourceDetail(detail);
        }

        public List<DataSourceTypeInfo> GetDataSourceTypeList()
        {
            return _c4Client.DataSource_GetType(_appCode);
        }

        public bool DeleteDataSource(DataSource type, Guid id, string modifyBy)
        {
            var delete = new DataSourceDelete()
            {
                AppCode = _appCode,
                Id =id,
                Type = type,
                ModifyBy = modifyBy
            };
           return _c4Client.DeleteDataSource(delete);
        }

        public List<DataSourceDetail> GetDataSourceDetailsByPaging(string code, out int totalCount,
            string appcode = null, int index = 0, int size = -1, string @group = "")
        {
            return _c4Client.GetDataSourceDetails(appcode ?? _appCode, code, group, Guid.Empty, index, size, out totalCount);
        }

        public List<DataSourceDetail> GetDataSourceDetailsByPaging(Guid typeId, out int totalCount,
            string appcode = null, int index = 0, int size = -1, string @group = "")
        {
            return _c4Client.GetDataSourceDetails(appcode ?? _appCode, "", group, typeId, index, size, out totalCount);
        }

        private List<DataSourceObject> GetDataSourceFromCache(string key)
        {
            if (_dataSouceDic == null)
            {
                _dataSouceDic=new Dictionary<string, List<DataSourceObject>>(StringComparer.InvariantCultureIgnoreCase);
                return null;
            }
            else
            {

                return _dataSouceDic.ContainsKey(key) ? _dataSouceDic[key] : null;
            }
        }

        private void RemoveDataSourceFromCache(string key)
        {
            if (_dataSouceDic == null)
            {
                _dataSouceDic = new Dictionary<string, List<DataSourceObject>>(StringComparer.InvariantCultureIgnoreCase);
            }
            else
            {
                if (_dataSouceDic.ContainsKey(key))
                {
                    _dataSouceDic[key] = null;
                }
            }
        }

    }
}
