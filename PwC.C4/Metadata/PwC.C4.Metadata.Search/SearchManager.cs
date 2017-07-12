using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search.BaseQuery;
using PwC.C4.Metadata.Search.Converter;
using PwC.C4.Metadata.Search.Exceptions;

namespace PwC.C4.Metadata.Search
{
    public class SearchManager
    {
        private readonly PwC.C4.Infrastructure.Logger.LogWrapper _log;

        private readonly string _searchProvider;

        private readonly string _baseStorage;

        private readonly string _entityName;

        private readonly string _appCode;

        private readonly string _connName;


        public SearchManager(string conn, string entity, string appCode = null,string searchProvider = null)
        {
            var apps = AppSettings.Instance;
            _appCode = string.IsNullOrEmpty(appCode)
              ? apps.GetAppCode()
              : appCode;
            _log = new LogWrapper();
            if (string.IsNullOrEmpty(entity))
            {
                _log.Error("SearchManager init error,Entity is null");
                throw new ArgumentException("SearchManager init error,Entity is null,AppCode:" + _appCode);
            }
            _entityName = entity;
            _searchProvider = string.IsNullOrEmpty(searchProvider)
                ? apps.GetSearchProvider().ToLower()
                : searchProvider.ToLower();
          
            _connName = conn;
            
            if (_searchProvider == "base")
            {
                _baseStorage = apps.GetStorage().ToLower();
            }
        }


        public List<string> GetDataIds(string keyColumn, IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders, int from,
            int size, out long totalCount)
        {
            try
            {
                if (_searchProvider == "elasticsearch")
                {
                    return ElasticSearchQuery.GetDataIds(_appCode, _entityName, keyColumn, searchItems, orders, from, size,
                        out totalCount);
                }
                else
                {
                    switch (_baseStorage)
                    {
                        case "mssql":
                            return MsSqlQuery.GetDataIds(_connName, _entityName, keyColumn, searchItems, orders, from, size,
                                out totalCount);
                        case "mongodb":
                            return MongoDbQuery.GetDataIds(_connName, _entityName, keyColumn, searchItems, orders, from, size,
                                out totalCount);
                        default:
                            _log.Error("Base search provider Storage should be “MsSql” or “MongoDb”,AppCode:" + _appCode);
                            throw new ArgumentException("Base search provider Storage should be “MsSql” or “MongoDb”,AppCode:" + _appCode);
                    }
                }
            }
              catch (Exception ex)
            {
                var msg =
                    $"GetDataIds error!,appCode：{_appCode}，entityName:{_entityName},keyColumn:{keyColumn},searchitem:{JsonHelper.Serialize(searchItems)},order:{JsonHelper.Serialize(orders)},from:{@from},size:{size}";
                _log.Error(msg, ex);
                throw new MetadataSearchException(msg, ex);
            }
            
        }

    }
}
