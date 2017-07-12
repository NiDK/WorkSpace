using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Scheduler.Plugin.ExcelReader.Model;

namespace PwC.C4.Scheduler.Plugin.ExcelReader
{
    public static class ExcelSettingsConfig
    {
        public static ExcelSettings LoadConfig()
        {
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, "ExcelSettings.config");
            return PwC.C4.Configuration.XmlSerializer<ExcelSettings>.DeserializeFromFile(filePath);
        }

        public static List<ColumnSettings> GetColumnSettings(string instanceName, string sheetName)
        {
            var ex = ExcelSettingsConfig.LoadConfig();
            var d = ex.ExcelInstances.FirstOrDefault(instance => instance.Name == instanceName);
            var sheetColums = d?.Sheets.FirstOrDefault(sheetInfo => sheetInfo.SheetName == sheetName);
            return sheetColums?.Columns;
        }

        public static Dictionary<string, string> GetColumnTypeStr(string instanceName, string sheetName)
        {
            var t = new Dictionary<string,string>();
            var colums = GetColumnSettings(instanceName, sheetName);
            colums?.ForEach(c =>
            {
                    t.Add(c.Name,c.Type);
            });
            return t;
        }

        public static Dictionary<string, Type> GetColumnType(string instanceName, string sheetName)
        {
            var t = new Dictionary<string, Type>();
            var colums = GetColumnSettings(instanceName, sheetName);
            colums?.ForEach(c =>
            {
                t.Add(c.Name, TypeHelper.GetType(c.Type));
            });
            return t;
        }

        public static List<string> GetEnableInstance()
        {
            var e = LoadConfig();
            var enables = new List<string>();
            e.ExcelInstances.ForEach(c =>
            {
                if(c.Enable)
                    enables.Add(c.Name);
            });
            return enables;
        } 
    }
}
