using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Interface;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Const;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.Cache;

namespace PwC.C4.Metadata.Service
{
    public class ColumnService : IColumnService
    {
        LogWrapper _log = new LogWrapper();

        #region Singleton

        private static ColumnService _instance = null;
        private static readonly object LockHelper = new object();

        public ColumnService()
        {
        }

        public static IColumnService Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new ColumnService();
                }
            }
            return _instance;
        }

#if DEBUG

        public static ColumnService DebugInstance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new ColumnService();
                }
            }
            return _instance;
        }

#endif

        #endregion

        private static string BuildCacheKey(string entityName, string middleWord, string key)
        {
            return string.Format("{0}-{1}-{2}", entityName, middleWord, key);
        }

        public IList<EntityColumn> GetEntityDefaultColumns<T>(string entityName = null) where T : DynamicMetadata
        {
            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var dc = MetadataSettings.Instance.GetEntity(entityName).DefaultColumn;
            if (!string.IsNullOrEmpty(dc))
            {
                var colnames = dc.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                var cols = GetEntityColumns<T>(colnames, entityName);
                return cols;
            }
            return new List<EntityColumn>();
        }

        public IList<EntityColumn> GetEntityColumns<T>(string key, string entityName = null) where T : DynamicMetadata
        {
            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var data =
                Preference.Get<IList<EntityColumn>>(BuildCacheKey(entityName, CacheTypeMiddleWord.EntityColumn,
                    key));
            if (data != null && data.Any()) return data;
            data = GetEntityDefaultColumns<T>(entityName);
            SetEntityColumns<T>(key, data, entityName);
            return data;
        }

        public IList<EntityColumn> GetEntityColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var entity = MetadataSettings.Instance.GetEntity(entityName);
            var entityColumns = new List<EntityColumn>();
            columnsName.ForEach(c =>
            {
                var cole = entity.Columns.FirstOrDefault(m => m.Name == c);
                if (cole == null)
                {
                    _log.Error(string.Format("Entity {0} doesn't contain column {1}", entityName, c));
                }
                else
                {
                    var col = new EntityColumn();
                    col.SetProperty("Name", cole.Name);
                    col.SetProperty("Type", cole.Type);
                    col.SetProperty("Label", cole.Properties["Label"]);
                    col.SetProperty("SortName", cole.Properties["SortName"]);
                    col.SetProperty("ShortName", cole.Properties["ShortName"]);
                    col.SetProperty("Width", cole.Properties["Width"]);
                    col.SetProperty("Order", int.Parse(cole.Properties["Order"]));
                    col.SetProperty("Sortable", bool.Parse(cole.Properties["Sortable"]));
                    col.SetProperty("Searchable", bool.Parse(cole.Properties["Searchable"]));
                    col.SetProperty("Visable", bool.Parse(cole.Properties["Visable"]));
                    if (!entityColumns.Contains(col))
                    {
                        entityColumns.Add(col);
                    }
                }

            });
            return entityColumns;
        }

        public void SetEntityColumns<T>(string key, IList<EntityColumn> columns, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntityColumn, key);
            Preference.Set(key, columns);
        }

        public IList<string> GetEntityColumnsName<T>(string key = null, string entityName = null)
            where T : DynamicMetadata
        {


            entityName = MetadataHelper.GetEntityName<T>(entityName);
            if (key == null)
            {
                return GetDefaultColumnNames(entityName);
            }
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntityColumnName, key);
            var data = Preference.Get<IList<string>>(key);
            if (data != null)
            {
                return data;
            }
            else
            {
                data = GetDefaultColumnNames(entityName);
                Preference.Set(key, data);
                return data;
            }

        }

        private IList<string> GetDefaultColumnNames(string entityName)
        {
            var dc = MetadataSettings.Instance.GetEntity(entityName).DefaultColumn;
            return !string.IsNullOrEmpty(dc)
                ? (IList<string>) dc.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries)
                : new List<string>();
        }

        public void SetEntityColumnsName<T>(string key, IList<string> columns, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntityColumnName, key);
            Preference.Set(key, columns);
        }

        public IList<EntityConfigColumn> GetEntityConfigColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var allCol = GetAllColumns<T>(true, entityName);
            var result = allCol.Select(entityColumn => new EntityConfigColumn()
            {
                IsChecked = columnsName.Contains(entityColumn.Name),
                Name = entityColumn.Name,
                Label = entityColumn.Label
            }).ToList();
            return result;
        }

        public IList<EntityConfigColumn> GetEntityConfigColumns<T>(string key, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var configList =
                Preference.Get<IList<EntityConfigColumn>>(BuildCacheKey(entityName,
                    CacheTypeMiddleWord.EntityConfigColumn, key));
            if (configList == null)
            {
                var defaultList = GetEntityDefaultColumns<T>(entityName);
                var colName = defaultList.Select(c => c.Name).ToList();
                configList = GetEntityConfigColumns<T>(colName, entityName);
                SetEntityConfigColumns<T>(key, configList, entityName);
            }
            return configList;
        }

        public void SetEntityConfigColumns<T>(string key, IList<EntityConfigColumn> columns, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntityConfigColumn, key);
            Preference.Set(key, columns);
        }

        public void SetEntityConfigColumns<T>(string key, IList<string> columns, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var cols = GetEntityConfigColumns<T>(columns, entityName);
            SetEntityConfigColumns<T>(key, cols, entityName);
            var colsEntity = GetEntityColumns<T>(columns, entityName);
            SetEntityColumns<T>(key, colsEntity, entityName);
            SetEntityColumnsName<T>(key, columns, entityName);
        }

        public IList<EntitySearchColumn> GetEntitySearchColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var scol = new List<EntitySearchColumn>();
            foreach (var coln in columnsName)
            {
                var col = MetadataSettings.Instance.GetColumn(entityName, coln);
                if (col == null)
                {
                    _log.Error("GetColumn is null,entity name:" + entityName + ",column name:" + coln);
                }
                else
                {
                    var searchCol = new EntitySearchColumn
                    {

                        Name = col.Name,
                        Label = col.Properties["Label"],
                        Order = int.Parse(col.Properties["Order"]),
                        DataSourceName = col.Properties["DataSource"],
                        DataSourceType = (DataSourceType) int.Parse(col.Properties["DataSourceType"]),
                        Type = (BaseControlType) int.Parse(col.Properties["DataControlType"])
                    };
                    searchCol.DataSource = DataSourceService.Instance()
                        .GetDataSourceBy(searchCol.DataSourceType, searchCol.DataSourceName);
                    scol.Add(searchCol);

                }

            }
            return scol;
        }

        public IList<EntitySearchColumn> GetEntitySearchColumns<T>(string key, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntitySearchColumn, key);
            var scol = Preference.Get<IList<EntitySearchColumn>>(key);
            return scol ?? new List<EntitySearchColumn>();
        }

        public void SetEntitySearchColumns<T>(string key, IList<EntitySearchColumn> columns, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntitySearchColumn, key);
            ;
            Preference.Set(key, columns);
        }


        public IList<EntityConfigColumn> GetSearchConfigColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var allCol = GetAllSearchColumns<T>(entityName);
            var result = allCol.Select(entityColumn => new EntityConfigColumn()
            {
                IsChecked = columnsName.Contains(entityColumn.Name),
                Name = entityColumn.Name,
                Label = entityColumn.Label
            }).ToList();
            return result;
        }

        public IList<EntityConfigColumn> GetSearchConfigColumns<T>(string key, string entityName = null)
            where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntitySearchConfigColumn, key);
            var searchConfig = Preference.Get<IList<EntityConfigColumn>>(key) ?? new List<EntityConfigColumn>();
            if (searchConfig.Count != 0) return searchConfig;
            var allSearchCol = GetAllSearchColumns<T>(entityName);
            allSearchCol.ToList().ForEach(c =>
            {
                var col = new EntityConfigColumn()
                {
                    IsChecked = false,
                    Label = c.Label,
                    Name = c.Name
                };
                searchConfig.Add(col);
            });
            return searchConfig;

        }

        public void SetSearchConfigColumns<T>(string key, IList<EntityConfigColumn> columns, string entityName = null)
            where T : DynamicMetadata
        {
            entityName = MetadataHelper.GetEntityName<T>(entityName);
            key = BuildCacheKey(entityName, CacheTypeMiddleWord.EntitySearchConfigColumn, key);
            Preference.Set(key, columns);
        }

        public void SetSearchConfigColumns<T>(string key, IList<string> columns, string entityName = null)
            where T : DynamicMetadata
        {
            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var cols = GetSearchConfigColumns<T>(columns, entityName);
            SetSearchConfigColumns<T>(key, cols, entityName);
        }


        public IList<EntityColumn> GetAllColumns<T>(bool isVisable, string entityName = null) where T : DynamicMetadata
        {
            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var entity = MetadataSettings.Instance.GetEntity(entityName);
            var cols = entity.Columns.OrderBy(c => c.Properties["Order"]).ToList();
            if (isVisable)
                cols = cols.Where(c => c.Properties["Visable"].ToLower() == "true").ToList();
            var colsName = cols.Select(c => c.Name).ToList();
            return GetEntityColumns<T>(colsName, entityName);
        }

        public IList<EntityColumn> GetAllSearchColumns<T>(string entityName = null) where T : DynamicMetadata
        {

            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var entity = MetadataSettings.Instance.GetEntity(entityName);
            var searchCols = entity.SearchColumn.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            var col = GetEntityColumns<T>(searchCols, entityName);
            return col;
        }

        public IList<string> GetAllDatetimeColumn<T>(string entityName = null) where T : DynamicMetadata
        {
            entityName = MetadataHelper.GetEntityName<T>(entityName);
            var entity = MetadataSettings.Instance.GetEntity(entityName);
            if (entity == null)
                return new List<string>();
            return entity.Columns.Where(c => c.Type.ToLower() == "datetime").Select(c => c.Name).ToList();
        }

        public bool UpdateColumnsOrder<T>(string key, string orderInfo, string entityName = null)
            where T : DynamicMetadata
        {
            try
            {

                entityName = MetadataHelper.GetEntityName<T>(entityName);
                var columns = GetEntityColumns<T>(key, entityName);
                var clist = orderInfo.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                Array.ForEach(clist, c =>
                {
                    var data = c.Split('-');
                    var col = data[0];
                    var ord = data[1];
                    if (columns.Any(m => m.Name == col))
                    {
                        var firstOrDefault = columns.FirstOrDefault(m => m.Name == col);
                        if (firstOrDefault != null)
                            firstOrDefault.Order = int.Parse(ord);
                    }
                    else
                    {
                        var colsm = GetAllColumns<T>(false, entityName).FirstOrDefault(m => m.Name == col);
                        if (colsm == null) return;
                        colsm.Order = int.Parse(ord);
                        columns.Add(colsm);
                    }
                });
                columns = columns.OrderBy(c => c.Order).ToList();
                var colName = columns.Select(c => c.Name).ToList();
                SetEntityColumns<T>(key, columns, entityName);
                SetEntityColumnsName<T>(key, colName, entityName);
                return true;
            }
            catch (Exception ee)
            {
                _log.Error(
                    string.Format("UpdateColumnsOrder Error,entityName:{0},key:{1},orderinfo:{2}", entityName, key,
                        orderInfo), ee);
                return false;
            }

        }

        public IList<DataSourceObject> GetDataSourceObjects(string column, string groupValue,
            string entityName = null)
        {
            var col = MetadataSettings.Instance.GetColumn(entityName, column);
            var dataSourceType = (DataSourceType) int.Parse(col.Properties["DataSourceType"]);
            var datasourceCode = col.Properties["DataSource"];
            return DataSourceService.Instance().GetDataSourceBy(dataSourceType, datasourceCode, groupValue);
        }

        public void CheckMetadataSettings(string entityName)
        {
            var entity = MetadataSettings.Instance.GetEntity(entityName);
            var errorNameList = new List<string>();
            var errorShortName = new List<string>();
            var errorDatasource = new List<string>();
            var existName = new List<string>();
            var existShortName = new List<string>();
            entity.Columns.ForEach(col =>
            {
                if (col.Properties.ContainsKey("ShortName"))
                {
                    if (existShortName.Contains(col.Properties["ShortName"]))
                    {
                        errorShortName.Add(col.Properties["ShortName"]);
                    }
                    else
                    {
                        existShortName.Add(col.Properties["ShortName"]);
                    }
                }
                if (col.Properties.ContainsKey("DataSourceType") && col.Properties.ContainsKey("DataSource"))
                {
                    if (col.Properties["DataSourceType"] != "0" && string.IsNullOrEmpty(col.Properties["DataSource"]))
                    {
                        errorDatasource.Add(col.Name);
                    }
                }
                if (existName.Contains(col.Name))
                {
                    errorNameList.Add(col.Name);
                }
                else
                {
                    existName.Add(col.Name);
                }

            });

            _log.Error(
                string.Format(
                    "Check metadata settings result:total column count:{0},\nduplicate column names:{1},\r\nduplicate short name:{2},\r\nerror datasource:{3}",
                    entity.Columns.Count, JsonHelper.Serialize(errorNameList), JsonHelper.Serialize(errorShortName),
                    JsonHelper.Serialize(errorDatasource)));
        }
    }
}
