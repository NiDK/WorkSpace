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
    internal static class HtmlSnippetDao
    {
        public static HtmlSnippet GetHtmlSnippet(string appCode, string code)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteAndGetInstance<HtmlSnippet>(db, "dbo.HtmlSnippet_Get",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@code", code);
                }, MapperParameter);
            return myentity;
        }

        private static void MapperParameter(IRecord record, HtmlSnippet entity)
        {
            entity.AppCode = record.Get<string>("AppCode");
            entity.Code = record.Get<string>("Code");
            entity.ControlType = (HtmlSnippetType)record.Get<Int32>("ControlType");
            entity.Id = record.Get<Guid>("Id");
            entity.Html = record.Get<string>("Html");
            entity.Description = record.Get<string>("Description");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
            entity.Group = record.Get<string>("Group");
            entity.ModifyBy = record.Get<string>("ModifyBy");
            entity.ModifyTime = record.Get<DateTime>("ModifyTime");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.CreateTime = record.Get<DateTime>("CreateTime");

        }
    }
}
