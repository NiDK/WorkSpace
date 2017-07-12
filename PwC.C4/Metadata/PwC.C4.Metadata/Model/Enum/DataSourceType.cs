using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Model.Enum
{
    public enum DataSourceType
    {
        /// <summary>
        /// None:no translated,no data source 
        /// </summary>
        None=0,
        /// <summary>
        /// System:translated,data source base on staffbank or other system defined
        /// </summary>
        System = 1,
        /// <summary>
        /// Mapping:translated,data source base on DataSource table
        /// </summary>
        Mapping = 2,
        /// <summary>
        /// Custom:translated,data source base on IDataSource
        /// </summary>
        Interface = 3
    }
}
