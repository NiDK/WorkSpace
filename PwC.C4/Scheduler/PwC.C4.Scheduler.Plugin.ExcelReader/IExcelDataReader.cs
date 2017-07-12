using System.Collections.Generic;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Scheduler.Plugin.ExcelReader
{

    public interface IExcelDataHandlingByMetadata
    {
        Dictionary<string, List<DynamicMetadata>> Execute(Dictionary<string, List<DynamicMetadata>> data);
    }

    public interface IExcelDataHandlingByJson
    {
        Dictionary<string, List<string>> Execute(Dictionary<string, List<string>> data);
    }
}
