﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
﻿using PwC.C4.Configuration.Data;
﻿using PwC.C4.DataService.Model;
﻿using PwC.C4.DataService.Model.Enum;
﻿using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    public static class EmailDao
    {
        #region Template

        internal static EmailTemplate GetTemplate(string appcode ,int tId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<EmailTemplate>(db, "dbo.EmailTemplate_GetById",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@TemplateId", tId);
                }, MapperTemplate);
            return myentity;
        }

        internal static List<EmailTemplate> GetTemplates(string appcode, List<string> templateCode, string group = null)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            List<EmailTemplate> list = SafeProcedure.ExecuteAndGetInstanceList<EmailTemplate>(db,
                "dbo.EmailTemplate_GetByCode",
                MapperTemplate,
                new SqlParameter[]
                {
                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Group", group ?? ""),
                    new SqlParameter("@TemplateCode", templateCode.ToStrIdTable())
                }
                );
            return list;
        }

        internal static List<EmailTemplate> GetTemplates(string appcode, string gourpName = null)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            List<EmailTemplate> list = SafeProcedure.ExecuteAndGetInstanceList<EmailTemplate>(db,
                "dbo.EmailTemplate_List",
                MapperTemplate,
                new SqlParameter[]
                {
                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Group", gourpName ?? "")
                }
                );
            return list;
        }

        internal static List<EmailTemplate> GetTemplates(string appcode, string name, int pageIndex, int pageSize,
            out int totalCount, string groupName = null)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            var rowcountParameter = new SqlParameter("@rowcount", SqlDbType.Int);
            rowcountParameter.Direction = ParameterDirection.Output;
            List<EmailTemplate> list = SafeProcedure.ExecuteAndGetInstanceList<EmailTemplate>(db,
                "dbo.EmailTemplate_ListByPaging",
                MapperTemplate,
                new SqlParameter[]
                {

                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Group", groupName ?? ""),
                    new SqlParameter("@TemplateName", name),
                    new SqlParameter("@PageIndex", pageSize),
                    new SqlParameter("@PageSize", pageSize),
                    rowcountParameter
                }
                );
            totalCount = (int)rowcountParameter.Value;
            return list;
        }

        internal static int CreateTemplate(EmailTemplate model)
        {
            Int32 identityid = 0;
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db,
                "dbo.EmailTemplate_Create"
                , parameters =>
                {
                    parameters.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
                    parameters.AddWithValue("@AppCode", model.AppCode);
                    parameters.AddWithValue("@Group", model.Group);
                    parameters.AddWithValue("@TemplateCode", model.TemplateCode);
                    parameters.AddWithValue("@TemplateName", model.TemplateName);
                    parameters.AddWithValue("@MailBcc", model.MailBcc);
                    parameters.AddWithValue("@MailCc", model.MailCc);
                    parameters.AddWithValue("@MailContent", model.MailContent);
                    parameters.AddWithValue("@MailFrom", model.MailFrom);
                    parameters.AddWithValue("@MailOrganisation", model.MailOrganisation);
                    parameters.AddWithValue("@MailReplyTo", model.MailReplyTo);
                    parameters.AddWithValue("@MailSubject", model.MailSubject);
                    parameters.AddWithValue("@MailSubmitBy", model.MailSubmitBy);
                    parameters.AddWithValue("@IsImmediate", model.IsImmediate);
                    parameters.AddWithValue("@ModifyBy", model.ModifyBy);
                    parameters.AddWithValue("@CreateBy", model.CreateBy);
                },
                outputparameters =>
                {
                    identityid = (Int32)outputparameters.GetValue("@Id");
                });
            return identityid;
        }

        internal static int UpdateTemplate(EmailTemplate model)
        {

            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            return (int)SafeProcedure.ExecuteScalar(db, "dbo.EmailTemplate_Update", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@TemplateID", model.TemplateId);
                parameters.AddWithValue("@AppCode", model.AppCode);
                parameters.AddWithValue("@TemplateCode", model.TemplateCode);
                parameters.AddWithValue("@TemplateName", model.TemplateName);
                parameters.AddWithValue("@MailBcc", model.MailBcc);
                parameters.AddWithValue("@MailCc", model.MailCc);
                parameters.AddWithValue("@MailContent", model.MailContent);
                parameters.AddWithValue("@MailFrom", model.MailFrom);
                parameters.AddWithValue("@MailOrganisation", model.MailOrganisation);
                parameters.AddWithValue("@MailReplyTo", model.MailReplyTo);
                parameters.AddWithValue("@MailSubject", model.MailSubject);
                parameters.AddWithValue("@MailSubmitBy", model.MailSubmitBy);
                parameters.AddWithValue("@IsImmediate", model.IsImmediate);
                parameters.AddWithValue("@ModifyBy", model.ModifyBy);
            });


        }

        internal static bool DeleteTemplate(string appcode, int templateId, int modifyBy)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            return SafeProcedure.ExecuteNonQuery(db,
                "dbo.EmailTemplate_Delete"
                , parameters =>
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@TemplateId", templateId);
                    parameters.AddWithValue("@ModifyBy", modifyBy);
                }) > 0;
        }

        private static void MapperTemplate(IRecord record, EmailTemplate entity)
        {
            entity.TemplateId = record.Get<Int32>("TemplateID");
            entity.AppCode = record.Get<string>("AppCode");
            entity.Group = record.Get<string>("Group");
            entity.TemplateCode = record.Get<string>("TemplateCode");
            entity.TemplateName = record.Get<string>("TemplateName");
            entity.MailFrom = record.Get<string>("MailFrom");
            entity.MailReplyTo = record.Get<string>("MailReplyTo");
            entity.MailCc = record.Get<string>("MailCc");
            entity.MailBcc = record.Get<string>("MailBcc");
            entity.MailOrganisation = record.Get<string>("MailOrganisation");
            entity.MailSubject = record.Get<string>("MailSubject");
            entity.MailContent = record.Get<string>("MailContent");
            entity.MailSubmitBy = record.Get<string>("MailSubmitBy");
            entity.IsImmediate = record.Get<bool>("IsImmediate");
            entity.CreateDate = record.Get<DateTime>("CreateDate");
            entity.ModifyDate = record.Get<DateTime>("ModifyDate");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.ModifyBy = record.Get<string>("ModifyBy");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
        }

        #endregion


        #region Parameter

        internal static EmailParameter GetParameter(string appcode, int pId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<EmailParameter>(db, "dbo.EmailParameters_GetById",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@ParameterId", pId);
                }, MapperParameter);
            return myentity;
        }

        internal static List<EmailParameter> GetParameters(string appcode, List<string> parameterCode, string group = null)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            List<EmailParameter> list = SafeProcedure.ExecuteAndGetInstanceList<EmailParameter>(db,
                "dbo.EmailParameters_GetByCode",
                MapperParameter,
                new SqlParameter[]
                {
                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Group", group ?? ""),
                    new SqlParameter("@ParameterCode", parameterCode.ToStrIdTable())
                }
                );
            return list;
        }

        internal static List<EmailParameter> GetEmailParameters(string appcode,string gourpName = null)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            List<EmailParameter> list = SafeProcedure.ExecuteAndGetInstanceList<EmailParameter>(db,
                "dbo.EmailParameters_List",
                MapperParameter,
                new SqlParameter[]
                {
                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Group", gourpName ?? "")
                }
                );
            return list;
        }

        internal static List<EmailParameter> GetEmailParameters(string appcode, string name, int pageIndex, int pageSize,
            out int totalCount, string groupName = null)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            var rowcountParameter = new SqlParameter("@rowcount", SqlDbType.Int);
            rowcountParameter.Direction = ParameterDirection.Output;
            List<EmailParameter> list = SafeProcedure.ExecuteAndGetInstanceList<EmailParameter>(db,
                "dbo.EmailParameters_ListByPaging",
                MapperParameter,
                new SqlParameter[]
                {

                    new SqlParameter("@AppCode", appcode),
                    new SqlParameter("@Group", groupName ?? ""),
                    new SqlParameter("@ParameterName", name),
                    new SqlParameter("@PageIndex", pageSize),
                    new SqlParameter("@PageSize", pageSize),
                    rowcountParameter
                }
                );
            totalCount = (int)rowcountParameter.Value;
            return list;
        }

        internal static int CreateParameter(EmailParameter model)
        {
            Int32 identityid = 0;
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db,
                "dbo.EmailParameters_Create"
                , parameters =>
                {
                    parameters.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
                    parameters.AddWithValue("@AppCode", model.AppCode);
                    parameters.AddWithValue("@Group", model.Group);
                    parameters.AddWithValue("@ParameterCode", model.ParameterCode);
                    parameters.AddWithValue("@ParameterName", model.ParameterName);
                    parameters.AddWithValue("@Assembly", model.Assembly);
                    parameters.AddWithValue("@ParameterType", model.ParameterType);
                    parameters.AddWithValue("@Content", model.Content);
                    parameters.AddWithValue("@CreateBy", model.CreateBy);
                    parameters.AddWithValue("@ModifyBy", model.CreateBy);
                },
                outputparameters =>
                {
                    identityid = (Int32)outputparameters.GetValue("@Id");
                });
            return identityid;
        }

        internal static int UpdateParameter(EmailParameter model)
        {

            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            return (int)SafeProcedure.ExecuteScalar(db, "dbo.EmailParameters_Update", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@ParameterID", model.ParameterId);
                parameters.AddWithValue("@AppCode", model.AppCode);
                parameters.AddWithValue("@Group", model.Group);
                parameters.AddWithValue("@ParameterCode", model.ParameterCode);
                parameters.AddWithValue("@ParameterName", model.ParameterName);
                parameters.AddWithValue("@Assembly", model.Assembly);
                parameters.AddWithValue("@ParameterType", model.ParameterType);
                parameters.AddWithValue("@Content", model.Content);
                parameters.AddWithValue("@ModifyBy", model.ModifyBy);
            });


        }

        internal static bool DeleteEmailParameter(string appcode, int parameterId, int modifyBy)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            return SafeProcedure.ExecuteNonQuery(db,
                "dbo.EmailParameters_Delete"
                , parameters =>
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@ParameterID", parameterId);
                    parameters.AddWithValue("@ModifyBy", modifyBy);
                }) > 0;
        }

        private static void MapperParameter(IRecord record, EmailParameter entity)
        {
            entity.AppCode = record.Get<string>("AppCode");
            entity.ParameterId = record.Get<Int32>("ParameterId");
            entity.ParameterType = (ParameterType)record.Get<Int32>("ParameterType");
            entity.ParameterCode = record.Get<string>("ParameterCode");
            entity.ParameterName = record.Get<string>("ParameterName");
            entity.Content = record.Get<string>("Content");
            entity.Assembly = record.Get<string>("Assembly");
            entity.IsDeleted = record.Get<bool>("IsDeleted");
            entity.Group = record.Get<string>("Group");
            entity.ModifyBy = record.Get<string>("ModifyBy");
            entity.ModifyDate = record.Get<DateTime>("ModifyDate");
            entity.CreateBy = record.Get<string>("CreateBy");
            entity.CreateDate = record.Get<DateTime>("CreateDate");

        }

        #endregion
    }
}
