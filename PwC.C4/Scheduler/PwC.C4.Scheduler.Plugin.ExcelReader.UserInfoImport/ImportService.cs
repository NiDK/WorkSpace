using System.Collections.Generic;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Scheduler.Plugin.ExcelReader.UserInfoImport
{
    public class MetaImportService: IExcelDataHandlingByMetadata
    {
        public Dictionary<string, List<DynamicMetadata>> Execute(Dictionary<string, List<DynamicMetadata>> data)
        {
            var str = data;
            foreach (var keyValuePair in str)
            {

                var items = keyValuePair.Value;
                items.ForEach(i =>
                {
                    if (keyValuePair.Key == "TestModel")
                        i.SafeSet("TestColumn3", false);
                });
            }
            return str;
        }
    }
}
