using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PwC.C4.Infrastructure.BaseLogger;

namespace PwC.C4.Scheduler.Plugin.ExcelReader.Dao
{
    internal static class ExcelImportDao
    {
        private static readonly LogWrapper Log=new LogWrapper();
        public static Dictionary<string, DataTable> ExecuteProc(Dictionary<string,DataTable> tables,string instanceName,string conntectString,string proc)
        {
            try
            {
                var dic = new Dictionary<string, DataTable>();
                var conStr = new SqlConnection(conntectString);
                var comStr = new SqlCommand(proc, conStr) {CommandType = CommandType.StoredProcedure};

                foreach (var dataTable in tables)
                {
                    comStr.Parameters.Add("@" + dataTable.Key, SqlDbType.Structured);
                    comStr.Parameters["@" + dataTable.Key].Value = dataTable.Value;
                }
                 var param = new SqlParameter("@tableNames", SqlDbType.VarChar,200)
                {
                    Direction = ParameterDirection.Output
                };
                comStr.Parameters.Add(param);

                var ds = new DataSet();

                var adapter = new SqlDataAdapter(comStr);

                adapter.Fill(ds);

                var strTableNames = comStr.Parameters["@tableNames"].Value.ToString();
                var tableNames = strTableNames.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < tableNames.Length; i++)
                {
                    ds.Tables[i].TableName = tableNames[i];
                    dic.Add(tableNames[i], ds.Tables[i]);
                }
                return dic;
            }
            catch(Exception ee)
            {
                Log.Error(
                    "ExecuteProc error,instance name:" + instanceName + ",conntectString:" + conntectString +
                    ",proc name:" + proc, ee);
                return null;
            }
        }

    }
}
