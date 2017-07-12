using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Metadata.Interface
{
    public interface IExcelManageService
    {
        MemoryStream GetExcel(List<Dictionary<string, object>> metadata, string type, IList<string> hideColumns, IList<string> columns = null);

        MemoryStream GetExcel<T>(List<T> data);

        MemoryStream GetExcel(string orderCol, string orderMethod,
            string searchItems, string page, int start,
            int length, string ids, string type, string customColumns);

        MemoryStream GetExcel<T>(string orderCol, string orderMethod,
            string searchItems,  string ids, string customColumns) where T:DynamicMetadata;
    }
}
