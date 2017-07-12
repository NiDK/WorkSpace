using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;

namespace PwC.C4.Metadata.Interface
{
    public interface IDataSource
    {
        List<DataSourceObject> GetDataSource(object obj = null);
    }
}
