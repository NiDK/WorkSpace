using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Config
{

    public static class MetadataSettingsExtension
    {

        private static Dictionary<string, Dictionary<string, string>> _shortNameDic;
        private static Dictionary<string, Dictionary<string, string>> _arrayDic;
        private static Dictionary<string, Dictionary<string, string>> _mappingDic;
        private static Dictionary<string, List<string>> _translatorColumnDic;
        private static Dictionary<string, List<string>> _stringFormatColumnDic;
        private static Dictionary<string, List<string>> _joinFormatColumnDic;
        private static Dictionary<string, MetadataEntityColumn> _entityColumnsDic;
        private static Dictionary<string, Tuple<DataSourceType, string, string>> _entityColumnDataSourceDic;
        private static Dictionary<string, Dictionary<string, string>> _dependentColumns;
        private static Dictionary<string, MetadataEntityWorkflow> _workflowDic;
        private static Dictionary<string, string> _entityPkDic;

        static readonly LogWrapper _log = new LogWrapper();
        public static MetadataEntity GetEntity(this MetadataSettings settings, string entityName=null)
        {
            if (settings != null && settings.Entitys != null) 
            {
                return (from g in settings.Entitys
                        where g.EntityName.Equals(entityName, StringComparison.CurrentCultureIgnoreCase)
                        select g).
                    FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public static Dictionary<string, string> GetEntityShortNameMapping<T>(this MetadataSettings settings, string entityName) where T: DynamicMetadata
        {
            var tableName = MetadataHelper.GetEntityName<T>(entityName);
            return GetEntityShortNameMapping(settings,tableName);
        }

        public static Dictionary<string, string> GetDependentColumn(this MetadataSettings settings, string entityName)
        {
            if (_dependentColumns == null)
            {
                _dependentColumns = new Dictionary<string, Dictionary<string, string>>();
            }
            if (_dependentColumns.ContainsKey(entityName))
                return _dependentColumns[entityName];
            else
            {
                var dic = new Dictionary<string, string>();
                var entity = GetEntity(settings, entityName);
                var columns = entity.Columns;
                columns.ForEach(col =>
                {
                    var prop = col.BaseProperties.FirstOrDefault(c => c.Key == "DataSourceGroupBy");
                    if (prop != null && !string.IsNullOrEmpty(prop.Value))
                        dic.Add(col.Name, prop.Value);
                });
                _dependentColumns.Add(entityName, dic);
                return dic;
            }
        }

        public static Dictionary<string,string> GetEntityShortNameMapping(this MetadataSettings settings, string entityName)
        {
            if (_shortNameDic == null)
            {
                _shortNameDic = new Dictionary<string, Dictionary<string, string>>();
            }
            if (_shortNameDic.ContainsKey(entityName))
                return _shortNameDic[entityName];
            else
            {
                var dic = new Dictionary<string, string>();
                var entity = GetEntity(settings, entityName);
                var columns = entity.Columns;
                var duplicateColName = new List<string>();
                columns.ForEach(col =>
                {
                    var prop = col.BaseProperties.FirstOrDefault(c => c.Key == "ShortName");
                    if (prop != null && !string.IsNullOrEmpty(prop.Value))
                    {
                        if (!dic.ContainsKey(col.Name))
                        {
                            dic.Add(col.Name, prop.Value);
                        }
                        else
                        {
                            duplicateColName.Add(col.Name);
                        }
                    }
                        
                });
                if (duplicateColName.Any())
                {
                    _log.Error("GetEntityShortNameMapping Error , there are duplicate column name:" +
                               JsonHelper.Serialize(duplicateColName));
                }
                _shortNameDic.Add(entityName, dic);
                return dic;
            }
        }

        public static List<string> GetTranslatorColumns(this MetadataSettings settings, string entityName,bool isEntity)
        {
            if (_translatorColumnDic == null)
            {
                _translatorColumnDic = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            }
            var key = string.Format("{0}-{1}", entityName, isEntity ? "Entity" : "List");
            if (_translatorColumnDic.ContainsKey(key))
                return _translatorColumnDic[key];
            else
            {
                var entity = GetEntity(settings, entityName);
                var columns = isEntity
                    ? entity.EntityTranslatedColumn.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : entity.ListTranslatedColumn.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (_translatorColumnDic.ContainsKey(entityName))
                {
                    _translatorColumnDic[key] = columns;
                }
                else
                {
                    _translatorColumnDic.Add(key, columns);
                }
                
                return columns;
            }
        }

        public static List<string> GetStringFormatColumns(this MetadataSettings settings, string entityName)
        {
            if (_stringFormatColumnDic == null)
            {
                _stringFormatColumnDic = new Dictionary<string, List<string>>();
            }
            if (_stringFormatColumnDic.ContainsKey(entityName))
                return _stringFormatColumnDic[entityName];
            else
            {
                var entity = GetEntity(settings, entityName);
                var columns =
                    entity.StringFormatColumn.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();
     
                if (_stringFormatColumnDic.ContainsKey(entityName))
                {
                    _stringFormatColumnDic[entityName] = columns;
                }
                else
                {
                    _stringFormatColumnDic.Add(entityName, columns);
                }

                return columns;
            }
        }

        public static Dictionary<string,string> GetArrayColumns(this MetadataSettings settings, string entityName)
        {
            return GetSubEntity(settings, entityName, "array", _arrayDic);
        }

        public static Dictionary<string, string> GetMappingColumns(this MetadataSettings settings, string entityName)
        {
            return GetSubEntity(settings, entityName, "mapping", _mappingDic);
        }

        private static Dictionary<string, string> GetSubEntity(this MetadataSettings settings, string entityName, string type, Dictionary<string, Dictionary<string, string>> staticDic)
        {
            if (staticDic == null)
            {
                staticDic = new Dictionary<string, Dictionary<string, string>>();
            }
            if (staticDic.ContainsKey(entityName))
                return staticDic[entityName];
            else
            {
                var entity = GetEntity(settings, entityName);
                if (entity != null)
                {
                    var columns =
                        entity.ArrayColumn.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var dic = new Dictionary<string, string>();
                    columns.ForEach(c =>
                    {
                        if (!dic.ContainsKey(c))
                        {
                            var col = GetColumn(settings, entityName, c);
                            if (col != null)
                            {
                                if (col.Type.Contains(type, StringComparison.OrdinalIgnoreCase) &&
                                    col.Type.IndexOf(":", StringComparison.Ordinal) > 0)
                                {
                                    var d = col.Type.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (d.Length == 2)
                                    {
                                        dic.Add(c, d[1] ?? "string");
                                    }

                                }
                            }
                        }
                    });
                    if (dic.Any())
                    {
                        if (staticDic.ContainsKey(entityName))
                        {
                            staticDic[entityName] = dic;
                        }
                        else
                        {
                            staticDic.Add(entityName, dic);
                        }
                    }
                    return dic;
                }
                else
                {
                    return new Dictionary<String, String>();
                }
            }
        }

        public static List<string> GetJoinFormatColumns(this MetadataSettings settings, string entityName)
        {
            if (_joinFormatColumnDic == null)
            {
                _joinFormatColumnDic = new Dictionary<string, List<string>>();
            }
            if (_joinFormatColumnDic.ContainsKey(entityName))
                return _joinFormatColumnDic[entityName];
            else
            {
                var entity = GetEntity(settings, entityName);
                var columns =
                    entity.JoinFormatColumn.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (_joinFormatColumnDic.ContainsKey(entityName))
                {
                    _joinFormatColumnDic[entityName] = columns;
                }
                else
                {
                    _joinFormatColumnDic.Add(entityName, columns);
                }

                return columns;
            }
        }

        public static Tuple<DataSourceType, string, string> GetColumnDataSource(this MetadataSettings settings, string entityName, string columnName)
        {
            if (_entityColumnDataSourceDic == null)
            {
                _entityColumnDataSourceDic = new Dictionary<string, Tuple<DataSourceType, string, string>>();
            }
            var key = string.Format("{0}-{1}", entityName, columnName);
            if (_entityColumnDataSourceDic.ContainsKey(key))
            {
                return _entityColumnDataSourceDic[key];
            }
            else
            {
                var col = GetColumn(settings, entityName, columnName);
                var result =
                    new Tuple<DataSourceType, string, string>(
                        (DataSourceType)int.Parse(col.Properties["DataSourceType"]), col.Properties["DataSource"], col.Properties["DataSourceGroupBy"]);
                if (_entityColumnDataSourceDic.ContainsKey(key))
                {
                    _entityColumnDataSourceDic[key] = result;
                }
                else
                {
                     _entityColumnDataSourceDic.Add(key,result);
                }
                return result;
            }
            
        }

        public static MetadataEntityWorkflow GetWorkflowSettings<T>(this MetadataSettings settings)
        {
            var entityName = MetadataHelper.GetEntityName<T>();
            if (_workflowDic == null)
            {
                _workflowDic = new Dictionary<string, MetadataEntityWorkflow>(StringComparer.InvariantCultureIgnoreCase);
            }
            if (_workflowDic.ContainsKey(entityName))
            {
                return _workflowDic[entityName];
            }
            else
            {
                var entity = settings.GetEntity(entityName);
                if (entity != null)
                {
                    _workflowDic.Add(entityName, entity.Workflow);
                    return entity.Workflow;
                }
                return new MetadataEntityWorkflow() {Enable = false};
            }
        }

        public static MetadataEntityColumn GetColumn(this MetadataSettings settings,string entityName, string columnName)
        {
            if (_entityColumnsDic == null)
            {
                _entityColumnsDic = new Dictionary<string, MetadataEntityColumn>();
            }
            var keyName = string.Format("{0}-{1}", entityName, columnName);
            if (_entityColumnsDic.ContainsKey(keyName))
            {
                return _entityColumnsDic[keyName];
            }
            else
            {
                var entity = GetEntity(settings, entityName);
                if (entity == null || entity.Columns == null)
                {
                    _log.Error("Metadata-setting error,entity name:"+entityName+" not found,please check metadata-settings config file");
                    throw new NoMetadataEntityException("Metadata-setting error,entity name:" + entityName + " not found,please check metadata-settings config file");
                }
               var col = entity.Columns.FirstOrDefault(c => String.Equals(c.Name, columnName, StringComparison.CurrentCultureIgnoreCase));
                if (col == null)
                {
                    _log.Error(new MetadataColumnNotExistException("Column \"" + columnName + "\" not in entity \"" +
                                                             entityName + "\""));
                    return null;
                }
                else
                {
                    if (_entityColumnsDic.ContainsKey(keyName))
                    {
                        _entityColumnsDic[keyName] = col;
                    }
                    else
                    {
                        _entityColumnsDic.Add(keyName, col);
                    }
                    return col;
                }
               
            }
        }

        public static string GetPrimaryKey(this MetadataSettings settings, string entityName)
        {
            if (_entityPkDic == null)
            {
                _entityPkDic = new Dictionary<string, string>();
            }
            if (_entityPkDic.ContainsKey(entityName))
            {
                return _entityPkDic[entityName];
            }
            else
            {
                var cols = GetEntity(settings, entityName);
                if (cols == null)
                {
                    return "RecordId";
                }
                else
                {
                    var pks = cols.Columns.Where(c => c.IsPk != null && c.IsPk == true).ToList();
                    if (pks.Count() == 1)
                    {
                        var pk = pks.FirstOrDefault();
                        if (pk != null)
                        {
                            _entityPkDic.Set(entityName, pk.Name);
                            return pk.Name;
                        }
                        else
                        {
                            _log.Error("GetPrimaryKey from metadata-settings error,pk columns is null");
                            return null;

                        }
                    }
                    else
                    {
                        _log.Error("GetPrimaryKey from metadata-settings error,pk columns count: " + pks.Count() +
                                   ", columns:" + JsonHelper.Serialize(pks));
                        return null;
                    }
                }
               
            }
        }

        public static void ReloadConfig()
        {
            _mappingDic = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            _arrayDic = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            _shortNameDic = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            _translatorColumnDic = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
            _entityColumnsDic = new Dictionary<string, MetadataEntityColumn>(StringComparer.InvariantCultureIgnoreCase);
            _entityColumnDataSourceDic = new Dictionary<string, Tuple<DataSourceType, string, string>>(StringComparer.InvariantCultureIgnoreCase);
            _dependentColumns = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
            _stringFormatColumnDic = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
            _workflowDic = new Dictionary<string, MetadataEntityWorkflow>(StringComparer.InvariantCultureIgnoreCase);
            _entityPkDic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
