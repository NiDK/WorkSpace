﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
﻿using PwC.C4.Configuration.Data;
﻿using PwC.C4.DataService.Model;
﻿using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    public static class WorkflowDao
    {

        internal static WorkflowBase GetActiveWorkFlow(string appcode,string entityName, int formId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<WorkflowBase>(db, "dbo.Workflow_GetActiveByFormId",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@entityName", entityName);
                    parameters.AddWithValue("@formId", formId);
                }, MapperWorkflow);
            return myentity;
        }

        internal static WorkflowBase GetActiveWorkFlow(string appcode, string entityName, string recordId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<WorkflowBase>(db, "dbo.Workflow_GetAcitveByRecordId",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@entityName", entityName);
                    parameters.AddWithValue("@recordId", recordId);
                }, MapperWorkflow);
            return myentity;
        }

        internal static WorkflowBase GetLastUnactivedWorkFlow(string appcode, string workflowCode, int instanceId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<WorkflowBase>(db, "dbo.Workflow_GetLastUnactived",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@workflowCode", workflowCode);
                    parameters.AddWithValue("@InstanceId", instanceId);
                }, MapperWorkflow);
            return myentity;
        }
        internal static WorkflowBase GetActiveByInstanceId(string appcode,int instanceId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<WorkflowBase>(db, "dbo.Workflow_GetActiveByInstanceId",
                delegate (IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", appcode);
                    parameters.AddWithValue("@instanceId", instanceId);
                }, MapperWorkflow);
            return myentity;
        }

        internal static WorkflowBase GetLastUnactivedWorkFlow(string appcode, string entityName, string recordId)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var myentity = SafeProcedure.ExecuteAndGetInstance<WorkflowBase>(db, "dbo.Workflow_GetLastUnactivedByDataId",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@entityName", entityName);
                    parameters.AddWithValue("@recordId", recordId);
                }, MapperWorkflow);
            return myentity;
        }


        internal static int InsertWorkflow(WorkflowBase model)
        {
            Int32 identityid = 0;
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db,
                "dbo.Workflow_InsertWorkflow"
                , parameters =>
                {
                    parameters.AddTypedDbNull("@Id", ParameterDirectionWrap.Output, DbType.Int32);
                    parameters.AddWithValue("@AppCode", model.AppCode);
                    parameters.AddWithValue("@entityName", model.EntityName);
                    parameters.AddWithValue("@workflowCode", model.WorkflowCode);
                    parameters.AddWithValue("@actionCode", model.ActionCode);
                    parameters.AddWithValue("@recordId", model.RecordId);
                    parameters.AddWithValue("@formId", model.FormId);
                    parameters.AddWithValue("@instanceId", model.InstanceId);
                    parameters.AddWithValue("@userId", model.UserId);
                    parameters.AddWithValue("@userRole", model.UserRole);
                    parameters.AddWithValue("@status", model.Status);
                    parameters.AddWithValue("@comment", model.Comment);
                },
                outputparameters =>
                {
                    identityid = (Int32)outputparameters.GetValue("@Id");
                });
            return identityid;
        }



        private static void MapperWorkflow(IRecord record, WorkflowBase entity)
        {
            entity.Id = record.Get<Int32>("Id");
            entity.AppCode = record.Get<string>("AppCode");
            entity.EntityName = record.Get<string>("EntityName");
            entity.WorkflowCode = record.Get<string>("WorkflowCode");
            entity.ActionCode = record.Get<string>("ActionCode");
            entity.FormId = record.Get<Int32>("FormId");
            entity.RecordId = record.Get<string>("RecordId");
            entity.InstanceId = record.Get<Int32>("InstanceId");
            entity.UserId = record.Get<string>("UserId");
            entity.UserRole = record.Get<string>("UserRole");
            entity.Status = record.Get<string>("Status");
            entity.Comment = record.Get<string>("Comment");
            entity.IsActive = record.Get<bool>("IsActive");
            entity.CreateTime = record.Get<DateTime>("CreateTime");

        }

    }
}
