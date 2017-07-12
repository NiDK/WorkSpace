using System;
using System.Collections.Generic;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Storage
{
    public interface IEntityService
    {
        int SaveEntity<T>(T model, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata;

        int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, object> prop, out string currentDataId, string dataBeforSaveHandlingScript = null)
     where T : DynamicMetadata;

        int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, string> prop, out string currentDataId, string dataBeforSaveHandlingScript = null)
            where T : DynamicMetadata;

        int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, object> prop, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata;

        int SaveEntity<T>(string dataId, string modifyUserId, Dictionary<string, string> prop, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata;

        int SaveEntity<T>(string dataId, string modifyUserId, string json, string dataBeforSaveHandlingScript = null) where T : DynamicMetadata;

        T GetEntity<T>(string dataId, List<string> properties = null) where T : DynamicMetadata;

        T GetEntityTranslated<T>(string dataId) where T : DynamicMetadata;

        T GetEntityTranslated<T>(string dataId, List<string> fields) where T : DynamicMetadata;
        [Obsolete("Please to use GetEntitesWithSearch<T>")]
        List<T> GetEntites<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders,
        List<string> columns, int pageIndex, int pageSize, out long totalCount, out List<string> dependentColumns,
     string datahandlingScript = null) where T : DynamicMetadata;
        [Obsolete("Please to use GetEntitesTranslatedWithSearch<T>")]
        List<T> GetEntitesTranslated<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders,
            List<string> columns, int pageIndex, int pageSize,
            Dictionary<string, Dictionary<object, object>> threadThempVariable, IList<string> specialFileds,
            Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback,
            out long totalCount, string datahandlingScript = null) where T : DynamicMetadata;

        List<T> GetEntitesWithSearch<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders,
List<string> columns, int pageIndex, int pageSize, out long totalCount, out List<string> dependentColumns,
string datahandlingScript = null) where T : DynamicMetadata;

        List<T> GetEntitesTranslatedWithSearch<T>(IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders,
            List<string> columns, int pageIndex, int pageSize,
            Dictionary<string, Dictionary<object, object>> threadThempVariable, IList<string> specialFileds,
            Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback,
            out long totalCount, string datahandlingScript = null) where T : DynamicMetadata;

        List<Dictionary<string, object>> GetEntitesDicTranslatedWithSearch(IList<SearchItem> searchItems,
            Dictionary<string, OrderMethod> orders, List<string> columns, int pageIndex, int pageSize,
            Dictionary<string, Dictionary<object, object>> threadThempVariable, IList<string> specialFileds,
            Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback,
            out long totalCount,
            string datahandlingScript = null);

        Dictionary<int, int> BatchSaveEntiteis<T>(IEnumerable<T> models) where T : DynamicMetadata;

        List<Dictionary<string, object>> GetDataByGroup(List<string> groupBy, out long totalCount,
            IList<SearchItem> searchItems = null, Dictionary<string, OrderMethod> sort = null, int index = 0,
            int pageSize = -1);

        object SpecialFieldExplanation(string fieldName, DynamicMetadata fieldModel,
            Dictionary<string, Dictionary<object, object>> tDictionary);

        bool CheckDataExist<T>(string dataId);

        bool CheckDataExist<T>(string column, string dataId);
    }
}
