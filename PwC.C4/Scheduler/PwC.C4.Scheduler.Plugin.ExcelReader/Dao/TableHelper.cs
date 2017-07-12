using System;
using System.Collections.Generic;
using System.Data;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Scheduler.Plugin.ExcelReader.Dao
{
    public static class TableHelper
    {

        public static Dictionary<string, DataTable> ToDataTable(this Dictionary<string, List<DynamicMetadata>> meta,string instanceName)
        {
            var tableDic = new Dictionary<string,DataTable>();
            foreach (var t in meta)
            {
                var table = new DataTable(t.Key);
                var colsettings = ExcelSettingsConfig.GetColumnType(instanceName, t.Key);
                foreach (var colsetting in colsettings)
                {
                    table.Columns.Add(colsetting.Key, colsetting.Value);
                }
               
                foreach (var col in t.Value)
                {
                    var dataRow = table.NewRow();
                    foreach (var cc in colsettings)
                    {
                        var v = col[cc.Key] ?? DBNull.Value;
                        dataRow[cc.Key] = v;
                    }
                    table.Rows.Add(dataRow);
                }
                tableDic.Add(t.Key,table);
            }
            return tableDic;
        }

        public static Dictionary<string, DataTable> ToDataTable(this Dictionary<string, List<string>> json, string instanceName)
        {
            var tableDic = new Dictionary<string, DataTable>();
            foreach (var t in json)
            {
                var table = new DataTable(t.Key);
                var colsettings = ExcelSettingsConfig.GetColumnType(instanceName, t.Key);
                foreach (var colsetting in colsettings)
                {
                    table.Columns.Add(colsetting.Key, colsetting.Value);
                }

                foreach (var col in t.Value)
                {
                    var dataRow = table.NewRow();
                    var myclass = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(col);
                    foreach (var cc in colsettings)
                    {
                        dataRow[cc.Key] = myclass[cc.Key];
                    }
                    table.Rows.Add(dataRow);
                }
                tableDic.Add(t.Key, table);
            }
            return tableDic;
        }
    }
}
