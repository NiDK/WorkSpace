using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.DataService.Model;

namespace PwC.C4.Metadata.Interface
{
    public interface IColumnService
    {
        IList<EntityColumn> GetEntityDefaultColumns<T>(string entityName = null) where T : DynamicMetadata;


        IList<EntityColumn> GetEntityColumns<T>(string key, string entityName = null) where T : DynamicMetadata;

        IList<EntityColumn> GetEntityColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata;

        void SetEntityColumns<T>(string key, IList<EntityColumn> columns, string entityName = null)
            where T : DynamicMetadata;


        IList<string> GetEntityColumnsName<T>(string key=null, string entityName = null) where T : DynamicMetadata;

        void SetEntityColumnsName<T>(string key, IList<string> columns, string entityName = null)
            where T : DynamicMetadata;

        
        IList<EntityConfigColumn> GetEntityConfigColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata;

        IList<EntityConfigColumn> GetEntityConfigColumns<T>(string key, string entityName = null)
            where T : DynamicMetadata;

        void SetEntityConfigColumns<T>(string key, IList<EntityConfigColumn> columns, string entityName = null)
            where T : DynamicMetadata;

        void SetEntityConfigColumns<T>(string key, IList<string> columns, string entityName = null)
            where T : DynamicMetadata;

        IList<EntitySearchColumn> GetEntitySearchColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata;

        IList<EntitySearchColumn> GetEntitySearchColumns<T>(string key, string entityName = null)
            where T : DynamicMetadata;

        void SetEntitySearchColumns<T>(string key, IList<EntitySearchColumn> columns, string entityName = null)
            where T : DynamicMetadata;

        IList<EntityConfigColumn> GetSearchConfigColumns<T>(IList<string> columnsName, string entityName = null)
            where T : DynamicMetadata;

        IList<EntityConfigColumn> GetSearchConfigColumns<T>(string key, string entityName = null)
            where T : DynamicMetadata;

        void SetSearchConfigColumns<T>(string key, IList<EntityConfigColumn> columns, string entityName = null)
            where T : DynamicMetadata;

        void SetSearchConfigColumns<T>(string key, IList<string> columns, string entityName = null)
            where T : DynamicMetadata;

        IList<EntityColumn> GetAllColumns<T>(bool isVisable, string entityName = null)
            where T : DynamicMetadata;

        IList<string> GetAllDatetimeColumn<T>(string entityName = null) where T : DynamicMetadata;

        IList<EntityColumn> GetAllSearchColumns<T>(string entityName = null)
            where T : DynamicMetadata;

        bool UpdateColumnsOrder<T>(string key, string orderInfo, string entityName = null) where T : DynamicMetadata;

        IList<DataSourceObject> GetDataSourceObjects(string column, string groupValue, string entityName = null);

        void CheckMetadataSettings(string entityName);
    }
}
