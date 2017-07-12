using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Interface;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;

namespace PwC.C4.Metadata.Service
{
    public class ExcelManageService : IExcelManageService
    {

        LogWrapper _log = new LogWrapper();
        #region Singleton

        private static ExcelManageService _instance = null;
        private static readonly object LockHelper = new object();

        public ExcelManageService()
        {
        }

        public static IExcelManageService Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new ExcelManageService();
                }
            }
            return _instance;
        }
#if DEBUG

        public static ExcelManageService DebugInstance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new ExcelManageService();
                }
            }
            return _instance;
        }

#endif

        #endregion

        public MemoryStream GetExcel(List<Dictionary<string, object>> metadata, string type,IList<string> hideColumns,IList<string> columns = null)
        {
            var table = new DataTable {TableName = "ExportExcel"};
            var dic = metadata.FirstOrDefault();
            var entity = ColumnService.Instance().GetAllColumns<DynamicMetadata>(false, type);
            if (entity != null)
            {
                
                if (columns != null && columns.Any())
                {
                    var list = new List<string>();
                    var lableList = new List<string>();
                    columns.ForEach(c =>
                    {
                        var colinfo = entity.FirstOrDefault(m => m.Name == c);
                        if (colinfo != null)
                        {
                            if (!table.Columns.Contains(colinfo.Label))
                            {
                               table.Columns.Add(colinfo.Label, typeof (string)); 
                            }
                            else
                            {
                                table.Columns.Add(colinfo.Name, typeof(string));
                                lableList.Add(colinfo.Label);
                            }
                            
                        }
                        else
                        {
                            list.Add(c);
                        }
                    });
                    if (lableList.Any())
                    {
                        _log.Error("Excel export warming,this column labels exist in table :" + type + ", below:" +
                                   JsonHelper.Serialize(lableList));
                    }
                    if (list.Any())
                    {
                        _log.Error("Excel export error,this columns dose not exist in entity :" + type + ", below:" +
                                   JsonHelper.Serialize(list));
                        return new MemoryStream();
                    }
                    

                }
                else
                {
                    if (dic != null)
                    {
                        foreach (var colName in from o in dic
                            where !hideColumns.Contains(o.Key)
                            let colName = ""
                            let col = entity.FirstOrDefault(c => c.Name == o.Key)
                            select col != null ? col.Label : o.Key)
                        {
                            table.Columns.Add(colName, typeof (string));
                        }
                    }
                }


                foreach (var staffBankForMetadata in metadata)
                {
                    var dr = table.NewRow();
                    var n = 0;
                    if (columns != null && columns.Any())
                    {
                        foreach (var column in columns)
                        {
                            dr[n] = staffBankForMetadata.ContainsKey(column) ? staffBankForMetadata[column] : "";
                            n++;
                        }
                    }
                    else
                    {
                        foreach (var d in staffBankForMetadata.Where(d => !hideColumns.Contains(d.Key)))
                        {
                            dr[n] = d.Value;
                            n++;
                        }
                    }

                    table.Rows.Add(dr);
                }
                var wb = new XLWorkbook();
                wb.Worksheets.Add(table);
                var ms = new MemoryStream();
                wb.SaveAs(ms);
                return ms;
            }
            else
            {
                _log.Error("GetExcel entity is null " + type + " not exist!");
                return new MemoryStream();
            }

        }

        public MemoryStream GetExcel<T>(List<T> data)
        {
            throw new NotImplementedException();
        }

        public MemoryStream GetExcel(string orderCol, string orderMethod, string searchItems, string page, int start, int length,
            string ids, string type, string customColumns)
        {
            throw new NotImplementedException();
        }

        public MemoryStream GetExcel<T>(string orderCol, string orderMethod, string searchItems, string ids, string customColumns) where T : DynamicMetadata
        {
            throw new NotImplementedException();
        }
    }
}
