using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Scheduler.Plugin.ExcelReader.Dao;
using PwC.C4.Scheduler.Plugin.ExcelReader.Model;
using Quartz;

namespace PwC.C4.Scheduler.Plugin.ExcelReader
{
    public class SchedulerJob: IJob
    {
        LogWrapper _log = new LogWrapper();
        private string _inputPath;
        private  string _outputPath;
        private  string _backupPath;
        private static Dictionary<string, ExcelInstance> _instances;
        public void Execute(IJobExecutionContext context)
        {
            if (_instances == null)
            {
                _instances = new Dictionary<string, ExcelInstance>();
            }
            var para = context.JobDetail.JobDataMap;
            _inputPath = para["InputPath"].ToString();
            _outputPath = para["OutputPath"].ToString();
            _backupPath = para["BackupPath"].ToString();
            _log.Error("Excel import starting..." + DateTime.Now.ToString("yyyy MM dd HH:mm:ss"));
            try
            {
                var folder = new DirectoryInfo(_inputPath);
                var enableInstance = ExcelSettingsConfig.GetEnableInstance();
                var ds = folder.GetDirectories();
                foreach (var directoryInfo in ds)
                {
                    var instanceName = directoryInfo.Name;
                    if (!enableInstance.Contains(instanceName)) continue;
                    var files = directoryInfo.GetFiles("*.xlsx").OrderBy(x => x.Name).ToArray();
                    foreach (var fileInfo in files)
                    {
                        SingleExcelHandle(fileInfo.Name, instanceName);
                    }
                }

            }
            catch (Exception ex)
            {
                _log.Error("Excel import error", ex);
            }
            _log.Error("Excel import completed..." + DateTime.Now.ToString("yyyy MM dd HH:mm:ss"));
        }


        private void SingleExcelHandle(string fileName, string instanceName)
        {
            string unreadPath = Path.Combine(_inputPath, instanceName, fileName);
            string failPath = Path.Combine(_outputPath, instanceName, fileName);
            string backUp = Path.Combine(_backupPath, instanceName, fileName);

            ExcelInstance excelInstance;

            if (_instances.ContainsKey(instanceName))
            {
                excelInstance = _instances[instanceName];
            }
            else
            {
                var instance = ExcelSettingsConfig.LoadConfig();
                excelInstance = instance.ExcelInstances.FirstOrDefault(c => c.Name == instanceName);
                if (excelInstance != null)
                {
                    _instances.Add(instanceName, excelInstance);
                }
            }

            if (excelInstance != null)
            {
                var executeType = excelInstance.ExecuteType;
                var method = excelInstance.Method;
                var parameter = excelInstance.Parameters;
                var resultTables = new Dictionary<string, DataTable>();
                switch (executeType.ToLower())
                {
                    case "interface":
                        var assemblyInfo = parameter.Split(new string[] { "," },
                            StringSplitOptions.RemoveEmptyEntries);
                        var assembly = Assembly.Load(assemblyInfo[0]);
                        var type = assembly.GetTypes().FirstOrDefault(t => t.FullName == assemblyInfo[1]);
                        if (type != null)
                        {
                            switch (method.ToLower())
                            {
                                case "metadata":
                                    var meta = (IExcelDataHandlingByMetadata)Activator.CreateInstance(type);
                                    var metadata = ExcelHandling.GetMetadata(unreadPath, instanceName);
                                    var metaresult = meta.Execute(metadata);
                                    resultTables = metaresult.ToDataTable(instanceName);
                                    break;
                                case "json":
                                    var json = (IExcelDataHandlingByJson)Activator.CreateInstance(type);
                                    var jsondata = ExcelHandling.GetJson(unreadPath, instanceName);
                                    var jsonresult = json.Execute(jsondata);
                                    resultTables = jsonresult.ToDataTable(instanceName);
                                    break;
                                default:

                                    break;
                            }
                        }

                        break;
                    case "stored procedure":
                        var metadataForProc = ExcelHandling.GetMetadata(unreadPath, instanceName);
                        var tables = metadataForProc.ToDataTable(instanceName);
                        resultTables = ExcelImportDao.ExecuteProc(tables, instanceName, parameter, method);
                        break;
                    default:

                        break;
                }

                var workBook = new XLWorkbook();
                resultTables.ForEach(newTable =>
                {
                    workBook.Worksheets.Add(newTable.Value, newTable.Key);
                });
                workBook.SaveAs(backUp);

            }
            if (File.Exists(unreadPath))
            {
                File.Delete(unreadPath);
            }


        }


    }
}
