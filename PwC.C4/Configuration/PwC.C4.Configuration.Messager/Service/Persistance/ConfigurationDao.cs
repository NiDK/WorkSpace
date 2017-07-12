using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using PwC.C4.Configuration.Data;
using PwC.C4.Configuration.Messager.Model;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Data.MapperDelegates;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Configuration.Messager.Service.Persistance
{
    internal static class ConfigurationDao
    {
        static readonly LogWrapper _log = new LogWrapper();
        public static int ConfigurationDetail_Create(ConfigurationDetail detail)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            return (int)SafeProcedure.ExecuteScalar(db, "dbo.ConfigurationDetail_Create",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@ConfigId", detail.ConfigId);
                    parameters.AddWithValue("@AppCode", detail.AppCode);
                    parameters.AddWithValue("@Major", detail.Major);
                    parameters.AddWithValue("@Minor", detail.Minor);
                    parameters.AddWithValue("@Content", detail.Content.OuterXml);
                    parameters.AddWithValue("@Creator", detail.Creator);
                    parameters.AddWithValue("@Status", detail.Status);

                });
        }

        public static int ConfigurationDetail_GetMinor(Guid configId,string appCode,short major)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            return (int)SafeProcedure.ExecuteScalar(db, "dbo.ConfigurationDetail_GetMinor", delegate (IParameterSet parameters)
            {
                parameters.AddWithValue("@ConfigId", configId);
                parameters.AddWithValue("@AppCode", appCode);
                parameters.AddWithValue("@Major", major);
            });
        }

        public static int ConfigurationType_Create(ConfigurationType type)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var data = (int)SafeProcedure.ExecuteScalar(db, "dbo.ConfigurationType_Create",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@Id", type.Id);
                    parameters.AddWithValue("@Name", type.Name);
                    parameters.AddWithValue("@Desc", type.Desc);
                    parameters.AddWithValue("@Creator", type.Creator);
                    parameters.AddWithValue("@Status", type.Status);
                });
            return data;
        }

        public static List<ConfigurationType> ConfigurationType_ListByPaging(int index, int size, out int totalCount)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var rowcountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var result = SafeProcedure.ExecuteAndGetInstanceList<ConfigurationType>(db,
                "dbo.ConfigurationType_ListByPaging", MapperConfigurationType, new SqlParameter[]
                {
                    new SqlParameter("@PageIndex", index),
                    new SqlParameter("@PageSize", size),
                    rowcountParameter
                }
                );
            totalCount = (int) rowcountParameter.Value;
            return result;
        }

        private static void MapperConfigurationType(IRecord record, ConfigurationType entity)
        {
            entity.CreateTime = record.Get<DateTime>("CreateTime");
            entity.Creator = record.Get<string>("Creator");
            entity.Desc = record.Get<string>("Desc");
            entity.Id = record.Get<Guid>("Id");
            entity.Name = record.Get<string>("Name");
            entity.Status = record.Get<int>("Status");
            entity.IdentityId = record.Get<int>("IdentityId");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
        }

        public static List<ConfigurationDetail> ConfigurationDetail_ListByPaging(Guid configId, int index, int size,
            out int totalCount)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var rowcountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var result = SafeProcedure.ExecuteAndGetInstanceList<ConfigurationDetail>(db,
                    "dbo.ConfigurationDetail_ListByPaging", delegate(IRecord record, ConfigurationDetail instance)
                    {
                        MapperConfigurationDetail(record, instance, false);
                    }, new SqlParameter[]
                    {
                        new SqlParameter("@ConfigId", configId),
                        new SqlParameter("@PageIndex", index),
                        new SqlParameter("@PageSize", size),
                        rowcountParameter
                    }
                    );
                totalCount = (int) rowcountParameter.Value;
                return result;
            }
            catch (Exception ex)
            {
                _log.Error("ConfigurationDetail_ListByPaging error", ex);
                totalCount = 0;
                return new List<ConfigurationDetail>();
            }

        }

        public static List<ConfigurationDetail> Notification_ForConfigurationUpdate(string machine,string key)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var result = SafeProcedure.ExecuteAndGetInstanceList<ConfigurationDetail>(db,
                    "dbo.Notification_ForConfigurationUpdate", delegate (IRecord record, ConfigurationDetail instance)
                    {
                        MapperConfigurationDetail(record, instance, true);
                    }, new SqlParameter[]
                    {
                        new SqlParameter("@AppCode",AppSettings.Instance.GetAppCode()),
                        new SqlParameter("@Machine", machine),
                        new SqlParameter("@Key", key)
                    }
                    );

                return result;
            }
            catch (Exception ex)
            {
                _log.Error("Notification_ForConfigurationUpdate error", ex);
                return new List<ConfigurationDetail>();
            }
        }

        public static List<ConfigurationDetail> ConfigurationDetail_ListByAppCode(Guid configId, string appcode,
            int major, int index, int size, out int totalCount)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var rowcountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var result = SafeProcedure.ExecuteAndGetInstanceList<ConfigurationDetail>(db,
                "dbo.ConfigurationDetail_ListByAppCode", delegate(IRecord record, ConfigurationDetail instance)
                {
                    MapperConfigurationDetail(record, instance,false);
                }, new SqlParameter[]
                {
                    new SqlParameter("@ConfigId", configId),
                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Major", major),
                    new SqlParameter("@PageIndex", index),
                    new SqlParameter("@PageSize", size),
                    rowcountParameter
                }
                );
            totalCount = (int) rowcountParameter.Value;
            return result;
        }

        public static ConfigurationDetail ConfigurationDetail_Entity(Guid configId, string appcode,
            short major,int minor)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<ConfigurationDetail>(db,
             "dbo.ConfigurationDetail_Entity",
             delegate (IParameterSet parameters)
             {
                 parameters.AddWithValue("@ConfigId", configId);
                 parameters.AddWithValue("@AppCode", appcode);
                 parameters.AddWithValue("@Major", major);
                 parameters.AddWithValue("@Minor", minor);
             }, delegate (IRecord record, ConfigurationDetail instance)
             {
                 MapperConfigurationDetail(record, instance, true);
             });
            return myentity;
        }

        public static ConfigurationDetail ConfigurationDetail_Entity(Guid id)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstance<ConfigurationDetail>(db,
                "dbo.ConfigurationDetail_EntityById",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@Id", id);
                }, delegate(IRecord record, ConfigurationDetail instance)
                {
                    MapperConfigurationDetail(record, instance, true);
                });
            return myentity;
        }

        public static void ConfigurationTruncateTable()
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db, "dbo.Configuration_Truncate");
        }

        public static int ConfigurationDetail_CreateApplication(ConfigurationDetail detail)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            return (int)SafeProcedure.ExecuteScalar(db, "dbo.ConfigurationDetail_CreateApplication",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@ConfigId", detail.ConfigId);
                    parameters.AddWithValue("@AppCode", detail.AppCode);
                    parameters.AddWithValue("@Creator", detail.Creator);
                    parameters.AddWithValue("@Status", detail.Status);

                });
        }


        private static void MapperConfigurationDetail(IRecord record, ConfigurationDetail entity, bool isNeedXml)
        {
            entity.ConfigName = record.GetExist("Name") ? record.GetOrDefault<string>("Name", "") : "";
            entity.CreateTime = record.GetOrDefault<DateTime>("CreateTime", DateTime.MinValue);
            entity.Creator = record.GetExist("Creator") ? record.GetOrDefault<string>("Creator", "System") : "System";
            if (isNeedXml && record.GetExist("Content"))
            {

                entity.Xml = record.GetOrDefault<string>("Content", "");

                if (!string.IsNullOrEmpty(entity.Xml))
                {
                    var m = new XmlDocument();
                    m.LoadXml(entity.Xml);
                    entity.Content = m;
                }
            }

            entity.Id = record.GetExist("Id") ? record.GetOrDefault<Guid>("Id", Guid.Empty) : Guid.Empty;
            entity.ConfigId = record.GetExist("ConfigId")
                ? record.GetOrDefault<Guid>("ConfigId", Guid.Empty)
                : Guid.Empty;
            entity.Status = record.GetExist("Status") ? record.GetOrDefault<int>("Status", 0) : 0;
            entity.Major = record.GetOrDefault<short>("Major", 0);
            entity.Minor = record.GetOrDefault<int>("Minor", 0);
            entity.AppCode = record.GetOrDefault<string>("AppCode", "General");
            entity.IsDeleted = record.GetExist("IsDeleted") && record.GetOrDefault<bool>("IsDeleted", false);
        }
    }
}
