using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search;

namespace PwC.C4.Metadata.Storage.Mssql.Persistance
{
    internal static class CommonDataDao
    {
        static readonly LogWrapper Log = new LogWrapper();

        public static bool CheckRecordExist(string conn,string tableName, string pkName, string dataId)
        {
            var db = Database.GetDatabase(conn);
            var data = (int)SafeProcedure.ExecuteScalar(db, "dbo.Metadata_CheckDataExist", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@tableName", tableName);
                parameters.AddWithValue("@pkColumn", pkName);
                parameters.AddWithValue("@dataId", dataId);
            });
            return data > 0;
        }

        public static int InsertMetadata(string conn, string tableName, string dataId, string modifyUserId,Dictionary<string, object> datas)
        {
            try
            {
                var cols = new List<string>();
                var values = new List<string>();
                foreach (var data in datas)
                {
                    cols.Add($"[{data.Key}]");
                    values.Add(GetSqlDataStr(data.Value));
                }
                if (!datas.ContainsKey("CreateDate"))
                {
                    cols.Add("[CreateDate]");
                    values.Add("getdate()");
                }
                if (!datas.ContainsKey("CreateBy"))
                {
                    cols.Add("[CreateBy]");
                    values.Add($"N'{modifyUserId}'");
                }
                if (!datas.ContainsKey("ModifyDate"))
                {
                    cols.Add("[ModifyDate]");
                    values.Add("getdate()");
                }
                if (!datas.ContainsKey("ModifyBy"))
                {
                    cols.Add("[ModifyBy]");
                    values.Add($"N'{modifyUserId}'");
                }
                if (!datas.ContainsKey("IsDeleted"))
                {
                    cols.Add("[IsDeleted]");
                    values.Add("0");
                }
                
                var colsStr = string.Join(",", cols);
                var valuesStr = string.Join(",", values);

                if (Log.IsDebugEnabled)
                {
                    var sql = $"insert into {tableName} ({colsStr}) values ({valuesStr})";
                    Log.Debug("InsertMetadata Debug info,Sql:" + sql);
                }
                var db = Database.GetDatabase(conn);
                var rest = SafeProcedure.ExecuteNonQuery(db, "dbo.Metadata_Insert", delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@tableName", tableName);
                    parameters.AddWithValue("@cols", colsStr);
                    parameters.AddWithValue("@vals", valuesStr);
                });
                if (rest > 0)
                {
                    CommonService.InsertMetadataLog(AppSettings.Instance.GetAppCode(), tableName, dataId, MetadataLogType.Add, JsonHelper.Serialize(datas), modifyUserId);
                }
                return rest;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"Save metadata error,table name:{tableName},datas:{JsonHelper.Serialize(datas)}";
                Log.Error(errorMessage, ee);
                return (int)EntityUpdateState.SystemError;
            }

        }

        public static int UpdateMetadata(string conn, string tableName, string pkName, string dataId, string modifyUserId,
            Dictionary<string, object> datas)
        {
            try
            {
                if (!datas.ContainsKey("ModifyBy"))
                {
                    datas.Add("ModifyBy", modifyUserId);
                }
                var updateCols =
                    (from data in datas
                        where data.Key != pkName
                        let value = GetSqlDataStr(data.Value)
                        select $"[{data.Key}]={value}").ToList(); 
                if (!updateCols.Contains("[ModifyDate]=Getdate()"))
                    updateCols.Add("[ModifyDate]=Getdate()");

                var updateInfo = string.Join(",", updateCols);
                if (Log.IsDebugEnabled)
                {
                    var sql = $"update {tableName} set {updateInfo} where {pkName}='{dataId}' and IsDeleted=0";
                    Log.Error("UpdateMetadata Debug info,Sql:" + sql);
                }
                var db = Database.GetDatabase(conn);
                var rest = SafeProcedure.ExecuteNonQuery(db, "dbo.Metadata_Update", delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@tableName", tableName);
                    parameters.AddWithValue("@pkName", pkName);
                    parameters.AddWithValue("@dataId", dataId);
                    parameters.AddWithValue("@vals", updateInfo);
                });
                if (rest > 0)
                {
                    CommonService.InsertMetadataLog(AppSettings.Instance.GetAppCode(), tableName, dataId, MetadataLogType.Edit,
                        JsonHelper.Serialize(datas), modifyUserId);
                }
                return rest;
            }
            catch (Exception ee)
            {
                var errorMessage =
                    $"Save metadata error,table name:{tableName},PK Name:{pkName},DataId:{dataId},datas:{JsonHelper.Serialize(datas)}";
                Log.Error(errorMessage, ee);
                return (int) EntityUpdateState.SystemError;
            }

        }

        public static List<T> GetCommonData<T>(string conn, string tableName, IList<SearchItem> searchItems,
           Dictionary<string,OrderMethod> orders,
            List<string> columns, int pageIndex, int pageSize, out long totalCount)
        {
            var select = "*";
            if (columns != null && columns.Count > 0)
            {
                var newlist = new List<string>();
                columns.ForEach(c => newlist.Add($"[{c}]"));
                select = string.Join(",", newlist);
            }

            var where = "";
            if (searchItems != null && searchItems.Count > 0)
            {
                where = searchItems.ToSql();
            }
            var orderInfo = orders.ToSql();
            var size = pageIndex + pageSize;
            var orderSql = string.IsNullOrEmpty(orderInfo) ? "ModifyDate Desc" : orderInfo;
            if (Log.IsDebugEnabled)
            {
                var sql = size == -1
                    ? $"select {columns} from {tableName}  where IsDeleted=0  {@where} order by {orderSql}"
                    : $"select {columns}  from (select *,ROW_NUMBER() over(order by {orderSql}) as rowNumber from {tableName} where IsDeleted=0  {@where})  temp where rowNumber between {pageIndex} and {size}";
                sql = sql + $"\r\nselect count(0) from {tableName} where IsDeleted=0 {where}";
                Log.Debug("GetCommonData Debug info,Sql:" + sql);
            }
            var db = Database.GetDatabase(conn);
            var rowcountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var list = SafeProcedure.ExecuteAndMetadata<T>(db,
                "dbo.Metadata_GetDataSet",
                (record, entity) =>
                {
                    dynamic meta = entity;
                    if (columns != null && columns.Any())
                    {
                        foreach (var col in columns)
                        {
                            meta.SetProperty(col, record[col]);
                        }
                    }
                    else
                    {
                        for (int n = 0; n < record.FieldCount; n++)
                        {
                            var name = record.GetName(n);
                            var value = record.GetValue(n).ToString();
                            meta.SetProperty(name, value);
                        }
                    }
                    
   
                }, new SqlParameter[]
                {

                    new SqlParameter("@TableName", tableName),
                    new SqlParameter("@Columns", select),
                    new SqlParameter("@Where", where),
                    new SqlParameter("@OrderCol", ""),
                    new SqlParameter("@OrderType", ""),
                    new SqlParameter("@OtherOrder", orderSql),
                    new SqlParameter("@Start", pageIndex),
                    new SqlParameter("@Size",size ),
                    rowcountParameter,
                }
                );
            totalCount = (int) rowcountParameter.Value;
            return list;
        }

        public static List<T> GetCommonDataWithSearch<T>(string conn, string tableName,string keyColumn, IList<SearchItem> searchItems,
           Dictionary<string, OrderMethod> orders,
            List<string> columns, int pageIndex, int pageSize, out long totalCount,string searchProvider =null)
        {
            var select = "*";
            if (columns != null && columns.Count > 0)
            {
                var newlist = new List<string>();
                columns.ForEach(c => newlist.Add($"[{c}]"));
                select = string.Join(",", newlist);
            }

            var sm = new SearchManager(conn, tableName,null,searchProvider);
            var datas = sm.GetDataIds(keyColumn, searchItems, orders, pageIndex, pageSize, out totalCount);
            var newArray = new List<string>();
            datas.ForEach(c => newArray.Add("'" + c + "'"));
            var queryFormat = "{0} {1} in ({2}) ";
            var where = string.Format(queryFormat, "And", keyColumn, string.Join(",", newArray));

            var db = Database.GetDatabase(conn);

            var list = SafeProcedure.ExecuteAndMetadata<T>(db,
                "dbo.Metadata_GetDataSetWithSearch",
                (record, entity) =>
                {
                    dynamic meta = entity;
                    if (columns != null && columns.Any())
                    {
                        foreach (var col in columns)
                        {
                            meta.SetProperty(col, record[col]);
                        }
                    }
                    else
                    {
                        for (int n = 0; n < record.FieldCount; n++)
                        {
                            var name = record.GetName(n);
                            var value = record.GetValue(n).ToString();
                            meta.SetProperty(name, value);
                        }
                    }


                }, new SqlParameter[]
                {

                    new SqlParameter("@TableName", tableName),
                    new SqlParameter("@Columns", select),
                    new SqlParameter("@Where", where)
                }
                );
            return list;
        }

        public static Dictionary<string,object> GetEntity(string conn, string tableName, string pkName, string dataId, IList<string> properties)
        {
            try
            {
                var columns = "*";
                if (properties.Count > 0)
                {
                    columns = string.Join(",", properties);
                }

                if (Log.IsDebugEnabled)
                {
                    var sql = $"select {columns} from {tableName} where {pkName}='{dataId}' and IsDeleted=0";
                    Log.Debug("GetEntity Debug info,Sql:" + sql);
                }

                var db = Database.GetDatabase(conn);
                return SafeProcedure.ExecuteAndGetInstance<Dictionary<string,object>>(db, "dbo.Metadata_GetEntity", delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@tableName", tableName);
                    parameters.AddWithValue("@pkColumn", pkName);
                    parameters.AddWithValue("@dataId", dataId);
                    parameters.AddWithValue("@columns", columns);
                }, (record, entity) =>
                {
                    if (properties.Any())
                    {
                        foreach (var v in properties)
                        {
                            entity.Set(v, Convert.IsDBNull(record[v]) ? null : record[v]);
                        }
                    }
                    else
                    {
                        for (var n = 0; n < record.FieldCount; n++)
                        {
                            var v = record.GetName(n);
                            entity.Set(v, Convert.IsDBNull(record[v]) ? null : record[v]);
                        }
                    }

                });
            }
            catch (Exception ee)
            {
                var errorMessage =
                    string.Format("get GetEntity error,table name:{0},PK Name:{1},DataId:{2},columns:{3}",
                        tableName, pkName, dataId, JsonHelper.Serialize(properties));
                Log.Error(errorMessage, ee);
                return new Dictionary<string, object>();
            }
        }


        private static string GetSqlDataStr(object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            var t = obj.GetType();
            var objs = obj.ToString();
            objs = objs.Replace("'", "''");
            switch (t.Name.ToLower())
            {
                case "string":
                case "guid":
                case "char":
                case "xml":
                    return $"N'{objs}'";
                case "datetime":
                    if (Convert.ToDateTime(objs) == DateTime.MinValue)
                    {
                        return "NULL";
                    }
                    else
                    {
                        return $"N'{((DateTime)obj).ToString(CultureInfo.InvariantCulture)}'"; 
                    }
                case "bool":
                case "boolean":
                    var bo = Convert.ToBoolean(objs);
                    return bo ? "1" : "0";
                case "int16":
                case "int32":
                case "int64":
                case "float":
                case "decimal":
                case "double":
                    return objs;
                default:
                    return objs;
            }
        }

    }
}
