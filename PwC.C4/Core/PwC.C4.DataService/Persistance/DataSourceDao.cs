using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    internal static class DataSourceDao
    {
        public static List<DataSourceObject> GetDataSourceObjects(string appcode,string dataSourceType, string group = "")
        {
            group = group ?? "";
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            List<DataSourceObject> list = SafeProcedure.ExecuteAndGetInstanceList<DataSourceObject>(db,
                "dbo.DataSource_GetDetails",
                (record, entity) =>
                {
                    entity.Id = record.Get<Guid>("Id");
                    entity.Key = record.Get<string>("Key");
                    entity.Value = record.Get<string>("Value");
                    entity.Desc = record.Get<string>("Desc");
                },
                new SqlParameter[]
                {
                    new SqlParameter("@appCode", appcode),
                    new SqlParameter("@dataSourceType", dataSourceType),
                    new SqlParameter("@group", group)
                }
                );

            return list;
        }

        public static List<DataSourceBase> GetDataSourceBase(string appCode)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            List<DataSourceBase> list = SafeProcedure.ExecuteAndGetInstanceList<DataSourceBase>(db,
                 "dbo.DataSource_GetDataSource",
                 (record, entity) =>
                 {
                     entity.Key = record.Get<string>("Key");
                     entity.Value = record.Get<string>("Value");
                     entity.Name = record.Get<string>("Name");
                     entity.Order = record.GetOrDefault<int>("Order",0);
                 },
                 new SqlParameter[]
                 {
                     new SqlParameter("@AppCode",appCode)
                 }
              );

            return list;
        }

        public static List<DataSourceTypeInfo> GetDataSourceType(string appCode)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            List<DataSourceTypeInfo> list = SafeProcedure.ExecuteAndGetInstanceList<DataSourceTypeInfo>(db,
                 "dbo.DataSource_GetType",
                 (record, type) =>
                 {
                     type.Id = record.Get<Guid>("Id");
                     type.AppCode = record.Get<string>("AppCode");
                     type.Type = record.Get<string>("Type");
                     type.Name = record.Get<string>("Name");
                     type.Desc = record.Get<string>("Desc");
                     type.Order = record.Get<int>("Order");
                     type.State = record.Get<int>("State");
                     type.CreateBy = record.Get<string>("CreateBy");
                 },
                 new SqlParameter[]
                 {
                     new SqlParameter("@AppCode",appCode)
                 }
              );

            return list;
        }

        public static bool UpdateDataSourceType(DataSourceTypeInfo type)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteNonQuery(db, "dbo.DataSource_UpdateType",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@Id", type.Id);
                    parameters.AddWithValue("@AppCode", type.AppCode);
                    parameters.AddWithValue("@Type", type.Type);
                    parameters.AddWithValue("@Name", type.Name);
                    parameters.AddWithValue("@Desc", type.Desc);
                    parameters.AddWithValue("@Order", type.Order);
                    parameters.AddWithValue("@State", type.State);
                    parameters.AddWithValue("@ModifyBy", type.CreateBy);
                });
            return myentity > 0;
        }

        public static bool UpdateDataSourceDetail(DataSourceDetail detail)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteNonQuery(db, "dbo.DataSource_UpdateDetail",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@Id", detail.Id);
                    parameters.AddWithValue("@AppCode", detail.AppCode);
                    parameters.AddWithValue("@DataSourceTypeId", detail.DataSourceTypeId);
                    parameters.AddWithValue("@DataSourceTypeName", detail.DataSourceTypeName);
                    parameters.AddWithValue("@Group", detail.Group ?? "");
                    parameters.AddWithValue("@Key", detail.Key);
                    parameters.AddWithValue("@Value", detail.Value);
                    parameters.AddWithValue("@Order", detail.Order);
                    parameters.AddWithValue("@State", detail.State);
                    parameters.AddWithValue("@ModifyBy", detail.CreateBy);
                });
            return myentity > 0;
        }


        public static bool DeleteDataSource(DataSourceDelete delete)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteNonQuery(db, "dbo.DataSource_Delete",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@Id", delete.Id);
                    parameters.AddWithValue("@AppCode", delete.AppCode);
                    parameters.AddWithValue("@Type",(int)delete.Type);
                    parameters.AddWithValue("@ModifyBy", delete.ModifyBy);
                });
            return myentity > 0;
        }

        public static List<DataSourceDetail> GetDataSourceDetails(string appCode, string code, string group, Guid? typeId,
            int page, int size,
            out int totalCount)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            var rowcountParameter = new SqlParameter("@rowcount", SqlDbType.Int) {Direction = ParameterDirection.Output};
            var groupParameter = new SqlParameter("@group", DBNull.Value);
            if (group != null)
            {
                groupParameter = new SqlParameter("@group", group);
            }
            List<DataSourceDetail> list = SafeProcedure.ExecuteAndGetInstanceList<DataSourceDetail>(db,
                "dbo.DataSource_GetDetailsForMaintance",
                MapperTemplate,
                new SqlParameter[]
                {

                    new SqlParameter("@appCode", appCode),
                    groupParameter,
                    new SqlParameter("@typeId", typeId),
                    new SqlParameter("@typeName", code),
                    new SqlParameter("@pageIndex", page),
                    new SqlParameter("@size", size),
                    rowcountParameter
                }
                );
            totalCount = (int) rowcountParameter.Value;
            return list;
        }

        private static void MapperTemplate(IRecord record, DataSourceDetail entity)
        {
            entity.DataSourceTypeId = record.Get<Guid>("DataSourceTypeId");
            entity.AppCode = record.Get<string>("AppCode");
            entity.DataSourceTypeName = record.Get<string>("Name");
            entity.Group = record.Get<string>("Group");
            entity.Id = record.Get<Guid>("Id");
            entity.Key = record.Get<string>("Key");
            entity.Order = record.Get<int>("Order");
            entity.Value = record.Get<string>("Value");
            entity.State = record.Get<int>("State");
            entity.CreateTime = record.Get<DateTime>("CreateTime");
            entity.ModifyTime = record.Get<DateTime>("ModifyTime");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.ModifyBy = record.Get<string>("ModifyBy");
            
        }
    }
}
