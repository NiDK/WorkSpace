using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    internal static class HtmlCategoryDao
    {
        public static HtmlCategory GetHtmlCategory_ByCode(string appCode, string group, string code)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstance<HtmlCategory>(db, "dbo.HtmlCategory_GetByCode",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@code", code);
                    parameters.AddWithValue("@Group", group);
                }, MapperParameter);
            return myentity;
        }

        public static HtmlCategory GetHtmlCategory_ById(string appCode, string group, Guid id)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstance<HtmlCategory>(db, "dbo.HtmlCategory_GetById",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@Id", id);
                    parameters.AddWithValue("@Group", group);
                }, MapperParameter);
            return myentity;
        }

        public static List<HtmlCategory> GetHtmlCategory_ListByAppCode(string appCode, string group)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstanceList<HtmlCategory>(db, "dbo.HtmlCategory_ListByAppCode",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@Group", group);
                }, MapperParameter);
            return myentity;
        }

        public static List<HtmlCategory> GetHtmlCategory_ListByRoles(string appCode, string group,List<string> roles )
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstanceList<HtmlCategory>(db, "dbo.[HtmlCategory_ListByRole]",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@Group", group);
                    parameters.AddWithValue("@Roles", roles.ToStrIdTable());
                }, MapperParameter);
            return myentity;
        }

        public static List<HtmlCategory> GetHtmlCategory_ListByParentId(string appCode, string group, Guid parentId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstanceList<HtmlCategory>(db, "dbo.HtmlCategory_ListByParentId",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@Group", group);
                    parameters.AddWithValue("@ParentId", parentId);
                }, MapperParameter);
            return myentity;
        }

        public static bool HtmlCategory_Delete(string appCode,string group,List<Guid> ids,string modifyBy)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteNonQuery(db, "dbo.HtmlCategory_Delete",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@Group", group);
                    parameters.AddWithValue("@Ids", ids.ToGuidIdTable());
                    parameters.AddWithValue("@ModifyBy", modifyBy);
                });
            return myentity > 0;
        }

        public static int HtmlCategory_Update(HtmlCategory entity)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteNonQuery(db, "dbo.HtmlCategory_Update",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@Id", entity.Id);
                    parameters.AddWithValue("@AppCode", entity.AppCode);
                    parameters.AddWithValue("@Group", entity.Group);
                    parameters.AddWithValue("@ParentId", entity.ParentId);
                    parameters.AddWithValue("@Code", entity.Code);
                    parameters.AddWithValue("@DisplayName", entity.DisplayName);
                    parameters.AddWithValue("@Description", entity.Description);
                    parameters.AddWithValue("@Type", entity.Type);
                    parameters.AddWithValue("@Order", entity.Order);
                    parameters.AddWithValue("@Func", entity.Func);
                    parameters.AddWithValue("@Url", entity.Url);
                    parameters.AddWithValue("@Parameters", entity.Parameters);
                    parameters.AddWithValue("@Icon", entity.Icon);
                    parameters.AddWithValue("@ModifyBy", entity.ModifyBy);
                    parameters.AddWithValue("@ModifyTime", DateTime.Now);
                    parameters.AddWithValue("@CreateBy", entity.CreateBy);
                    parameters.AddWithValue("@CreateTime", DateTime.Now);
                    parameters.AddWithValue("@IsCollapse", entity.IsCollapse);
                    parameters.AddWithValue("@Status", entity.Status);
                    parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
                });
            return myentity;
        }

        private static void MapperParameter(IRecord record, HtmlCategory entity)
        {
            entity.Id = record.Get<Guid>("Id");
            entity.AppCode = record.Get<string>("AppCode");
            entity.Group = record.Get<string>("Group");
            entity.ParentId = record.Get<Guid>("ParentId");
            entity.Code = record.Get<string>("Code");
            entity.DisplayName = record.Get<string>("DisplayName");
            entity.Description = record.Get<string>("Description");
            entity.Type = record.Get<int>("Type");
            entity.Order = record.Get<int>("Order");
            entity.Url = record.Get<string>("Url");
            entity.Func = record.Get<string>("Func");
            entity.Parameters = record.Get<string>("Parameters");
            entity.ParameterDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(entity.Parameters))
            {
                var s = entity.Parameters.Split(new string[] {"&"}, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length > 0)
                {
                    foreach (var s1 in s)
                    {
                        var keyPaire = s1.Split(new string[] {"="}, StringSplitOptions.RemoveEmptyEntries);
                        if (keyPaire.Length == 2)
                        {
                            entity.ParameterDic.Add(keyPaire[0], keyPaire[1]);
                        }
                    }
                }
            }
            entity.Icon = record.Get<string>("Icon");
            entity.ModifyBy = record.Get<string>("ModifyBy");
            entity.ModifyTime = record.Get<DateTime>("ModifyTime");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.CreateTime = record.Get<DateTime>("CreateTime");
            entity.IsCollapse = record.GetOrDefault<bool>("IsCollapse",true);
            entity.Status = record.Get<int>("Status");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
        }
    }
}
