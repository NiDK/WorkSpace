using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Rush.WcfService.Models;

namespace PwC.C4.Rush.WcfService.Service.Persistance
{
    internal static class FormDao
    {
        private static readonly LogWrapper Log = new LogWrapper();

        internal static List<FormMain> GetFormList(int pageSize, int pageIndex, string orderBy, string where, out int totalCount)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var rowcountParameter = new SqlParameter("@recordTotal", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var result = SafeProcedure.ExecuteAndGetInstanceList<FormMain>(db,
                    "dbo.Common_Paging", MapperFormMain, new SqlParameter[]
                    {
                        new SqlParameter("@viewName", "vwForm"),
                        new SqlParameter("@fieldName", "Id,FormName,ConnName,EntityName,AliasName,ConnectionString,Layout,CreateBy,CreateTime,ModifyBy,ModifyTime,Status,IsDeleted,LayoutName,LayoutCode"),
                        new SqlParameter("@pageSize", pageSize),
                        new SqlParameter("@pageNo", pageIndex),
                        new SqlParameter("@orderString", orderBy),
                        new SqlParameter("@whereString", where),
                        new SqlParameter("@keyName", "Id"),
                        rowcountParameter
                    }
                    );
                totalCount = (int)rowcountParameter.Value;
                return result;
            }
            catch (Exception ex)
            {
                totalCount = 0;
                Log.Error(
                    "GetFormList(int pageSize, int pageIndex, string orderBy, string where, out int totalCount) error",
                    ex);
                return new List<FormMain>();
            }
        }

        internal static FormMain GetFormBaseInfo(Guid formId)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var result = SafeProcedure.ExecuteAndGetInstance<FormMain>(db,
                    "dbo.Form_Main_Get", delegate(IParameterSet parameters)
                    {
                        parameters.AddWithValue("@formId", formId);

                    }, MapperFormMain);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(
                    "GetFormBaseInfo error",
                    ex);
                return new FormMain();
            }
        }

        internal static FormMain GetFormBaseInfoByAlias(string alias)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var result = SafeProcedure.ExecuteAndGetInstance<FormMain>(db,
                    "dbo.Form_Main_GetByAliasName", delegate (IParameterSet parameters)
                    {
                        parameters.AddWithValue("@AliasName", alias);

                    }, MapperFormMain);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(
                    "GetFormBaseInfoByAlias error",
                    ex);
                return new FormMain();
            }
        }

        internal static int SaveFormBaseInfo(FormMain form)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                return SafeProcedure.ExecuteNonQuery(db,
                "dbo.Form_Main_Save"
                , parameters =>
                {
                    parameters.AddWithValue("@Id", form.Id);
                    parameters.AddWithValue("@FormName", form.FormName);
                    parameters.AddWithValue("@ConnName", form.ConnName);
                    parameters.AddWithValue("@Layout", form.Layout);
                    parameters.AddWithValue("@EntityName", form.EntityName);
                    parameters.AddWithValue("@AliasName", form.AliasName);
                    parameters.AddWithValue("@ConnectionString", form.LinkTrackingConnName);
                    parameters.AddWithValue("@CreateBy", form.CreateBy);
                    parameters.AddWithValue("@ModifyBy", form.ModifyBy);
                    parameters.AddWithValue("@Status", form.Status);
                    parameters.AddWithValue("@IsDeleted", form.IsDeleted);
                });
            }
            catch (Exception ex)
            {
                Log.Error(
                    "SaveFormBaseInfo error",
                    ex);
                return -1;
            }
        }

        internal static int UpdateStructure(Guid formId,string userId,string prop, string javascript, string styles, List<FormControl> formControls)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                return SafeProcedure.ExecuteNonQuery(db,
                "dbo.Form_Main_UpdateStructure"
                , parameters =>
                {
                    parameters.AddWithValue("@formId", formId);
                    parameters.AddWithValue("@Props", prop);
                    parameters.AddWithValue("@Structure", JsonHelper.Serialize(formControls));
                    parameters.AddWithValue("@modifyBy", userId);
                    parameters.AddWithValue("@JavaScript", javascript);
                    parameters.AddWithValue("@Styles", styles);
                });
            }
            catch (Exception ex)
            {
                Log.Error(
                    "UpdateStructure error",
                    ex);
                return -1;
            }
        }

        internal static int DeleteFormBaseInfo(Guid formId,string modifyBy)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                return SafeProcedure.ExecuteNonQuery(db,
                "dbo.Form_Main_Delete"
                , parameters =>
                {
                    parameters.AddWithValue("@formId", formId);
                    parameters.AddWithValue("@modifyBy", modifyBy);
                });
            }
            catch (Exception ex)
            {
                Log.Error(
                    "DeleteFormBaseInfo error",
                    ex);
                return -1;
            }
        }

        internal static List<FormLayout> GetFormLayoutList()
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var result = SafeProcedure.ExecuteAndGetInstanceList<FormLayout>(db,
                    "dbo.Form_Layout_ListForSelect", MapperFormLayout);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(
                    "GetFormLayoutList error",
                    ex);
                return new List<FormLayout>();
            }
        }

        internal static string GetLayoutHtml(Guid layoutId)
        {
            try
            {
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var data = SafeProcedure.ExecuteScalar(db, "dbo.Form_Layout_GetHtml", delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@layout", layoutId);
                });
                return data?.ToString() ?? "Layout error";
            }
            catch (Exception ex)
            {
                Log.Error(
                    "GetLayoutHtml error",
                    ex);
                return "Layout error";
            }
        }

        private static void MapperFormMain(IRecord record, FormMain entity)
        {
            entity.Id = record.Get<Guid>("Id");
            entity.FormName = record.Get<string>("FormName");
            entity.ConnName = record.Get<string>("ConnName");
            entity.EntityName = record.Get<string>("EntityName");
            entity.AliasName = record.Get<string>("AliasName");
            entity.LinkTrackingConnName = record.Get<string>("ConnectionString");
            entity.Layout = record.Get<Guid>("Layout");
            entity.Props = record.GetExist("Props") ? record.Get<string>("Props").Trim() : ""; 
            entity.Structure = record.GetExist("Structure") ? record.Get<string>("Structure").Trim() : "";
            entity.JavaScript = record.GetExist("JavaScript") ? record.Get<string>("JavaScript") : "";
            entity.Styles = record.GetExist("Styles") ? record.Get<string>("Styles") : "";
            entity.LayoutCode = record.GetExist("LayoutCode") ? record.Get<string>("LayoutCode").Trim() : ""; 
            entity.LayoutName = record.GetExist("LayoutName") ? record.Get<string>("LayoutName").Trim() : ""; 
            entity.ModifyBy = record.Get<string>("ModifyBy");
            entity.ModifyTime = record.Get<DateTime>("ModifyTime");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.CreateTime = record.Get<DateTime>("CreateTime");
            entity.Status = record.Get<int>("Status");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
        }

        private static void MapperFormLayout(IRecord record, FormLayout entity)
        {
            entity.Id = record.Get<Guid>("Id");
            entity.Name = record.Get<string>("Name");
            entity.Code = record.Get<string>("Code");
            entity.Html = record.Get<string>("Html");
            entity.Status = record.Get<int>("Status");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
        }
    }
}
