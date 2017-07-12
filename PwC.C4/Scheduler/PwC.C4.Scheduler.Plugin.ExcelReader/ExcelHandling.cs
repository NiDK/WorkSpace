using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ClosedXML.Excel;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Scheduler.Plugin.ExcelReader.Model;

namespace PwC.C4.Scheduler.Plugin.ExcelReader
{
    public static class ExcelHandling
    {

        public static Dictionary<string, List<DynamicMetadata>> GetMetadata(string path, string instanceName)
        {

            var workBook = new XLWorkbook(path);
            var data = new Dictionary<string, List<DynamicMetadata>>();
            var typeMapping = new Dictionary<string, Dictionary<string, string>>();
            workBook.Worksheets.ForEach(sheet =>
            {
                Dictionary<string, string> tm;
                var c = GetColumns(sheet, 1);
                var list = new List<DynamicMetadata>();
                var entityName = sheet.Name;
                if (typeMapping.ContainsKey(entityName) && typeMapping[entityName] != null)
                {
                    tm = typeMapping[entityName];
                }
                else
                {
                    tm = ExcelSettingsConfig.GetColumnTypeStr(instanceName, entityName);
                    typeMapping.Add(entityName, tm);
                }
                var sc = ExcelSettingsConfig.GetColumnSettings(instanceName, entityName)
                    .ToDictionary(k => k.Name, v => v);
                foreach (var row in sheet.Rows())
                {
                    var errorMsg = "";
                    if (row.RowNumber() == 1) continue;
                    var dicIndex = 0;
                    var dic = new Dictionary<string, object>();
                    foreach (var cell in row.Cells())
                    {
                        var type = tm[c[dicIndex]];
                        var d = cell.RichText.Text.Trim();
                        object value;
                        if (type.Replace("?", "").ToLower() == "datetime")
                        {
                            value = TypeHelper.TypeConverter(type, ConvertToDateTime(d));
                        }
                        else
                        {
                            value = TypeHelper.TypeConverter(type, d);
                        }
                        errorMsg += CheckData(sc[c[dicIndex]], c[dicIndex], value);
                        dic[c[dicIndex]] = value;
                        dicIndex++;
                    }
                    errorMsg = errorMsg == "" ? "Ok" : errorMsg;
                    dic.Add("ImportExcelStatus", errorMsg);
                    var mo = new DynamicMetadata {Properties = dic};
                    list.Add(mo);
                }
                data.Add(entityName, list);
            });

            return data;
        }

        public static Dictionary<string, List<string>> GetJson(string path, string instanceName)
        {

            var workBook = new XLWorkbook(path);
            var data = new Dictionary<string, List<string>>();
            var typeMapping = new Dictionary<string, Dictionary<string, string>>();
            workBook.Worksheets.ForEach(sheet =>
            {
                Dictionary<string, string> tm;
                var c = GetColumns(sheet, 1);
                var list = new List<string>();
                var entityName = sheet.Name;
                if (typeMapping.ContainsKey(entityName) && typeMapping[entityName] != null)
                {
                    tm = typeMapping[entityName];
                }
                else
                {
                    tm = ExcelSettingsConfig.GetColumnTypeStr(instanceName, entityName);
                    typeMapping.Add(entityName, tm);
                }
                var sc = ExcelSettingsConfig.GetColumnSettings(instanceName, entityName)
                    .ToDictionary(k => k.Name, v => v);
                foreach (var row in sheet.Rows())
                {
                    var errorMsg = "";
                    var itemobject = new List<string>();
                    if (row.RowNumber() == 1) continue;
                    var dicIndex = 0;
                    foreach (var cell in row.Cells())
                    {
                        var type = tm[c[dicIndex]];
                        var d = cell.RichText.Text.Trim();
                        object value;
                        if (type.Replace("?", "").ToLower() == "datetime")
                        {
                            value = TypeHelper.TypeConverter(type, ConvertToDateTime(d));
                        }
                        else
                        {
                            value = TypeHelper.TypeConverter(type, d);
                        }
                        errorMsg += CheckData(sc[c[dicIndex]], c[dicIndex], value);
                        itemobject.Add(JsonFormat(c[dicIndex], JsonHelper.Serialize(value)));
                        dicIndex++;
                    }
                    errorMsg = errorMsg == "" ? "Ok" : errorMsg;
                    itemobject.Add("{\"ImportExcelStatus\",\"" + errorMsg + "\"");
                    var item = "{" + string.Join(",", itemobject) + "}";
                    list.Add(item);
                }
                data.Add(entityName, list);
            });

            return data;
        }

        public static string JsonFormat(string key, object value)
        {
            var format = "\"{0}\":\"{1}\"";
            return string.Format(format, key, value);
        }

        public static string ConvertToDateTime(string datetime)
        {
            string newTime;
            var arrStr = datetime.Split('/');

            var year = arrStr[2];
            var month = arrStr[0];
            var date = arrStr[1];
            var arr = year.Split(' ');
            if (Convert.ToInt32(month) < 10)
                month = "0" + month;
            if (Convert.ToInt32(date) < 10)
                date = "0" + date;
            if (arr.Length > 1)
            {
                newTime = date + "/" + month + "/" + arr[0] + " " + arr[1];
            }
            else
            {
                newTime = date + "/" + month + "/" + year;
            }
            return newTime;
        }

        public static List<string> GetColumns(IXLWorksheet workSheet, int row)
        {
            return workSheet.Columns().Select(ixlCol => ixlCol.Cell(row).RichText.Text.Trim()).ToList();
        }

        private static string CheckData(ColumnSettings setting, string colName, object value)
        {
            if (setting.IsRequire && (value == null || value.ToString() == ""))
            {
                return "Column : " + colName + " is empty;";
            }
            else if (!(value is DateTime)&& !(value is bool))
            {
                if (value.ToString().Length > setting.Length)
                {
                    return "Column : " + colName + " length over range;";
                }
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}
