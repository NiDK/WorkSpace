using System.Collections.Generic;

namespace PwC.C4.Scheduler.Plugin.ExcelReader.Model
{
   public class ExcelSettings
    {
        public List<ExcelInstance> ExcelInstances { get; set; } 
    }

    public class ExcelInstance
    {
        public string Name { get; set; }

        public bool Enable { get; set; }

        public string Desc { get; set; }

        public string ExecuteType { get; set; }

        public string Method { get; set; }

        public string Parameters { get; set; }

        public List<Sheet> Sheets { get; set; } 

    }

    public class Sheet
    {
        public string SheetName { get; set; }

        public List<ColumnSettings> Columns { get; set; } 
    }

    public class ColumnSettings
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsRequire { get; set; }

        public int Length { get; set; }
    }

}
