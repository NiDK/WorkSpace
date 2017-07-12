using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Interface;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Storage.MongoDb.Persistance;
using PwC.C4.Metadata.Storage.MongoDB;

namespace PwC.C4.Metadata.Storage.MongoDb.Service
{
    public class EntityService : IEntityService
    {
         
        private readonly LogWrapper _log = new LogWrapper();

        #region Singleton

        private static EntityService _instance = null;
        private static readonly object LockHelper = new object();
        private static string _connName = null;
        private static string _entityName = null;
        private static string _searchProvider = null;
        public EntityService(string connectName, string entityName = null, string searchProvider = null)
        {
            _connName = connectName;
            _entityName = entityName;
            _searchProvider = searchProvider;
        }

        public static IEntityService Instance(string connectName, string entityName = null, string searchProvider = null)
        {
            
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new EntityService(connectName, entityName, searchProvider);
                }
            }
            return _instance;
        }

#if DEBUG

        public static EntityService DebugInstance(string connectName, string entityName = null, string searchProvider = null)
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new EntityService(connectName, entityName, searchProvider);
                }
            }
            return _instance;
        }

#endif

        #endregion
         


        public int SaveEntity<T>(T model, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata
        {
            var info = typeof (T);
            try
            {
                dynamic m = model;
                string tableName = m.GetEntityName(_entityName);
                string pkName = m.GetPrimaryKey(_entityName);
                string dataId = m.GetPrimaryValue(_entityName);
                Dictionary<string, object> prop = m.Properties;
                if (string.IsNullOrEmpty(pkName) || string.IsNullOrEmpty(tableName) ||
                    !prop.Any()) return (int) EntityUpdateState.None;
                dataId = dataId == string.Empty ? Guid.NewGuid().ToString() : dataId;
                var isExist = CommonDataDao.CheckRecordExist(_connName,tableName, dataId);
                if (isExist)
                {
                    return CommonDataDao.UpdateMetadata(_connName, tableName, pkName, dataId, m.ModifyBy, m.Properties, dataBeforSaveHandlingScript);
                }
                else
                {
                    prop.Set(pkName, dataId);
                    return CommonDataDao.InsertMetadata(_connName, tableName, pkName, dataId, m.ModifyBy, prop, dataBeforSaveHandlingScript);
                }
            }
            catch
            {
                throw new NoMetadataEntityException(info.FullName);
            }
        }

        public int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, object> prop, out string currentDataId,
            string dataBeforSaveHandlingScript = null) where T : DynamicMetadata
        {
            var tableName = MetadataHelper.GetEntityName<T>(_entityName);
            var pkName = MetadataHelper.GetPrimaryKey<T>(_entityName);
            return SaveEntity<T>(dataId, tableName, pkName, modifyUserId, prop, out currentDataId,
                dataBeforSaveHandlingScript);
        }

        public int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, object> prop, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata
        {
            string gid;
            return SaveEntity<T>(dataId, modifyUserId, prop, out gid, dataBeforSaveHandlingScript);
        }


        public int SaveEntity<T>(string dataId, string modifyUserId, string json, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata
        {
            var prop = JsonHelper.Deserialize<Dictionary<string, string>>(json);
            return SaveEntity<T>(dataId, modifyUserId, prop, dataBeforSaveHandlingScript);
        }

        public int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, string> prop, string dataBeforSaveHandlingScript = null)
            where T : DynamicMetadata
        {
            string gid;
            return SaveEntity<T>(dataId, modifyUserId, prop, out gid, dataBeforSaveHandlingScript);
        }

        public int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, string> prop, out string currentDataId, string dataBeforSaveHandlingScript = null)
            where T : DynamicMetadata
        {
            var tableName = MetadataHelper.GetEntityName<T>(_entityName);
            var pkName = MetadataHelper.GetPrimaryKey<T>(_entityName);
            var obj = MetadataHelper.ToObjects<T>(prop, _entityName);
            return SaveEntity<T>(dataId, tableName, pkName, modifyUserId, obj, out currentDataId,
                dataBeforSaveHandlingScript);
        }

        private int SaveEntity<T>(string dataId, string tableName, string pkName, string modifyUserId,
            Dictionary<string, object> obj, out string currentDataId, string dataBeforSaveHandlingScript = null)
            where T : DynamicMetadata
        {
            var info = typeof (T);
            try
            {

                if (obj.ContainsKey(pkName) && dataId == string.Empty)
                {
                    dataId = obj[pkName].ToString();
                }
                if (string.IsNullOrEmpty(pkName) || string.IsNullOrEmpty(tableName) ||
                    !obj.Any())
                {
                    currentDataId = string.Empty;
                    return (int) EntityUpdateState.None;
                }
                dataId = dataId == String.Empty ? Guid.NewGuid().ToString() : dataId;
                currentDataId = dataId;
                var isExist = CommonDataDao.CheckRecordExist(_connName, tableName, dataId);
                if (isExist)
                {

                    return CommonDataDao.UpdateMetadata(_connName, tableName, pkName, dataId, modifyUserId, obj,
                        dataBeforSaveHandlingScript);
                }
                else
                {
                    obj.Set(pkName, dataId);
                    return CommonDataDao.InsertMetadata(_connName, tableName, pkName, dataId, modifyUserId, obj,
                        dataBeforSaveHandlingScript);
                }

            }
            catch (Exception ee)
            {
                _log.Error(ee);
                throw new NoMetadataEntityException(info.FullName);
            }
        }



        public T GetEntity<T>(string dataId, List<string> properties = null) where T : DynamicMetadata
        {
            var tableName = MetadataHelper.GetEntityName<T>(_entityName);
            var pkName = MetadataHelper.GetPrimaryKey<T>(_entityName);
            if (properties == null)
                properties = new List<string>();
            else
            {
                if (!properties.Contains("RecordId"))
                    properties.Add("RecordId");
            }
            var prop = CommonDataDao.GetEntity(_connName, tableName, pkName, dataId, properties);
            dynamic metadata = (T)Activator.CreateInstance(typeof(T));
            metadata.Properties = MongoTypeUtilities.DatetimeFix(prop.ToDictionary(), tableName) ;
            return metadata;
        }

        public T GetEntityTranslated<T>(string dataId) where T : DynamicMetadata
        {
            return GetEntityTranslated<T>(dataId, new List<string>());
        }

        public T GetEntityTranslated<T>(string dataId, List<string> fields) where T : DynamicMetadata
        {

            try
            {
                var tableName = MetadataHelper.GetEntityName<T>(_entityName);
                var pkName = MetadataHelper.GetPrimaryKey<T>(_entityName);
                var properties = new List<string>();
                var prop = CommonDataDao.GetEntity(_connName, tableName, pkName, dataId, properties);
                dynamic metadata = (T)Activator.CreateInstance(typeof(T));
                metadata.Properties = MongoTypeUtilities.DatetimeFix(prop.ToDictionary(), tableName);
                var threadTempDic = new Dictionary<string, Dictionary<object, object>>();
                var list = new List<T>() { metadata };
                DynamicMetadataTranslator.Translate(list, fields,
                    m => new DynamicMetadata[] { m }, threadTempDic, new List<string>(), null, tableName, true);
                return list.First();
            }
            catch (Exception ee)
            {
                _log.Error(ee);
                return default(T);
            }
        }


        public List<T> GetEntitesTranslatedWithSearch<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders, List<string> columns, int pageIndex, int pageSize,
            Dictionary<string, Dictionary<object, object>> threadThempVariable, IList<string> specialFileds, Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback, out long totalCount,
            string datahandlingScript = null) where T : DynamicMetadata
        {
            try
            {
                List<string> dependentColumns;
                var data = GetEntitesWithSearch<T>(searchItems, orders, columns, pageIndex, pageSize,
                        out totalCount, out dependentColumns, datahandlingScript);

                DynamicMetadataTranslator.Translate(data, columns, m => new DynamicMetadata[] { m }, threadThempVariable,
                    specialFileds, callback, _entityName);
                dependentColumns.ForEach(c => data.ForEach(d =>
                {
                    dynamic m = d;
                    m.RemoveProperty(c);
                    d = m;
                }));
                return data;
            }
            catch (Exception ee)
            {
                _log.Error("GetEntites<T> Error", ee);
                totalCount = 0;
                return new List<T>();
            }
        }

        public List<Dictionary<string, object>> GetEntitesDicTranslatedWithSearch(IList<SearchItem> searchItems,
            Dictionary<string, OrderMethod> orders, List<string> columns, int pageIndex, int pageSize,
            Dictionary<string, Dictionary<object, object>> threadThempVariable, IList<string> specialFileds,
            Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback,
            out long totalCount,
            string datahandlingScript = null)
        {
            try
            {
                List<string> dependentColumns;
                var result = new List<Dictionary<string,object>>();
                var data = GetEntitesWithSearch<DynamicMetadata>(searchItems, orders, columns, pageIndex, pageSize,
                        out totalCount, out dependentColumns, datahandlingScript);
                DynamicMetadataTranslator.Translate(data, columns, m => new DynamicMetadata[] { m }, threadThempVariable,
                    specialFileds, callback, _entityName);
                dependentColumns.ForEach(c => data.ForEach(d =>
                {
                    dynamic m = d;
                    m.RemoveProperty(c);
                    d = m;
                }));
                data.ForEach(c =>
                {
                    result.Add(c.Properties);
                });
                return result;
            }
            catch (Exception ee)
            {
                _log.Error("GetEntites<T> Error", ee);
                totalCount = 0;
                return new List<Dictionary<string, object>>();
            }
        }

        public Dictionary<int, int> BatchSaveEntiteis<T>(IEnumerable<T> models) where T : DynamicMetadata
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, object>> GetDataByGroup(List<string> groupBy, out long totalCount, IList<SearchItem> searchItems = null, Dictionary<string, OrderMethod> sort = null, int index = 0,
            int pageSize = -1)
        {
            return CommonDataDao.GetDataByGroup(_connName, _entityName, searchItems, groupBy, sort, index, pageSize,
                out totalCount);
        }


        private List<string> GetDependentColumnWithoutCurrent<T>(List<string> columns) where T : DynamicMetadata
        {
            if(columns==null || !columns.Any())
                return new List<string>();
            _entityName = MetadataHelper.GetEntityName<T>(_entityName);
            var deDic = MetadataSettings.Instance.GetDependentColumn(_entityName);
            var result = new List<string>();
            columns.ForEach(c =>
            {
                if (!deDic.ContainsKey(c)) return;
                if (!columns.Contains(deDic[c]))
                {
                    result.Add(deDic[c]);
                }
            });
            return result;
        }

        public List<T> GetEntites<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders,
            List<string> columns, int pageIndex, int pageSize,
            out long totalCount, out List<string> dependentColumns, 
            string datahandlingScript = null) where T : DynamicMetadata
        {
            try
            {
                var result = new List<T>();
                _entityName = MetadataHelper.GetEntityName<T>(_entityName);
                dependentColumns = GetDependentColumnWithoutCurrent<T>(columns);
                var pkName = MetadataHelper.GetPrimaryKey<T>(_entityName);
                if (dependentColumns.Any())
                {
                    columns.AddRange(dependentColumns);
                }
                if (columns != null && !columns.Contains(pkName))
                {
                    columns.Add(pkName);
                }
                var ds = CommonDataDao.GetCommonData(_connName, _entityName, searchItems, orders, columns, pageIndex,
                    pageSize, out totalCount, datahandlingScript);
                ds.ForEach(c =>
                {
                    dynamic metadata = (T) Activator.CreateInstance(typeof (T));
                    metadata.Properties = MongoTypeUtilities.DatetimeFix(c.ToDictionary(), _entityName); 
                    result.Add(metadata);

                });

                return result;
            }
            catch (Exception ee)
            {
                _log.Error("GetEntites<T> Error", ee);
                totalCount = 0;
                dependentColumns = new List<string>();
                return new List<T>();
            }

        }

        public List<T> GetEntitesTranslated<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders,
            List<string> columns, int pageIndex, int pageSize, Dictionary<string, Dictionary<object, object>> threadThempVariable, IList<string> specialFileds, Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback,
            out long totalCount,string datahandlingScript = null) where T : DynamicMetadata
        {
            try
            {
                List<string> dependentColumns;
                var result = new List<T>();
                var data = GetEntites<T>(searchItems, orders, columns, pageIndex, pageSize,
                        out totalCount, out dependentColumns,datahandlingScript);
                data.ForEach(c =>
                {
                    dynamic metadata = (T)Activator.CreateInstance(typeof(T));
                    metadata.Properties = MongoTypeUtilities.DatetimeFix(c.Properties, _entityName);
                    result.Add(metadata);

                });
                DynamicMetadataTranslator.Translate(result, columns, m => new DynamicMetadata[] { m }, threadThempVariable,
                    specialFileds, callback, _entityName);
                dependentColumns.ForEach(c => result.ForEach(d =>
                {
                    dynamic m = d;
                    m.RemoveProperty(c);
                    d = m;
                }));
                return result;
            }
            catch (Exception ee)
            {
                _log.Error("GetEntites<T> Error", ee);
                totalCount = 0;
                return new List<T>();
            }

        }

        public List<T> GetEntitesWithSearch<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders, List<string> columns, int pageIndex, int pageSize,
            out long totalCount, out List<string> dependentColumns, string datahandlingScript = null) where T : DynamicMetadata
        {
            try
            {
                var result = new List<T>();
                _entityName = MetadataHelper.GetEntityName<T>(_entityName);
                dependentColumns = GetDependentColumnWithoutCurrent<T>(columns);
                var pkName = MetadataHelper.GetPrimaryKey<T>(_entityName);
                if (dependentColumns.Any())
                {
                    columns.AddRange(dependentColumns);
                }
                if (columns != null && !columns.Contains(pkName))
                {
                    columns.Add(pkName);
                }
                var ds = CommonDataDao.GetCommonDataWithSearch(_connName, _entityName, pkName, searchItems, orders, columns, pageIndex,
                    pageSize, out totalCount, datahandlingScript,_searchProvider);
                ds.ForEach(c =>
                {
                    dynamic metadata = (T)Activator.CreateInstance(typeof(T));
                    metadata.Properties = MongoTypeUtilities.DatetimeFix(c.ToDictionary(), _entityName);
                    result.Add(metadata);

                });

                return result;
            }
            catch (Exception ee)
            {
                _log.Error("GetEntites<T> Error", ee);
                totalCount = 0;
                dependentColumns = new List<string>();
                return new List<T>();
            }
        }

        public object SpecialFieldExplanation(string fieldName, DynamicMetadata fieldModel,
            Dictionary<string, Dictionary<object, object>> tDictionary)
        {
            throw new NotImplementedException();
        }

        public bool CheckDataExist<T>(string dataId)
        {
            var tableName = MetadataHelper.GetEntityName<T>(_entityName);
            return CommonDataDao.CheckRecordExist(_connName, tableName, dataId);
        }

        public bool CheckDataExist<T>(string column, string dataId)
        {
            var tableName = MetadataHelper.GetEntityName<T>(_entityName);
            return CommonDataDao.CheckRecordExist(_connName, tableName, column, dataId);
        }
    }
}
