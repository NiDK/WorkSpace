using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net.Connection;
using MongoDB.Driver;
using Nest;
using Newtonsoft.Json.Linq;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search.Config;
using PwC.C4.Metadata.Search.Converter;

namespace PwC.C4.Metadata.Search.BaseQuery
{
    public static class ElasticSearchQuery
    {

        internal static List<string> GetDataIds(string appCode, string entity, string keyColumn,
            IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders, int from,
            int size, out long totalCount)
        {
            var esInstance = ElasticMappingConfig.Instance;
            var esNodes = esInstance.ElasticNodes(appCode);
            var esIndex = esInstance.ElasticIndexName(entity, appCode);
            var first = esNodes.FirstOrDefault();
            if (first != null)
            {
                var uri = new Uri(first);
                var settings = new ConnectionSettings(uri, esIndex);
                var searchClient = new ElasticClient(settings);
                var searchRequest = new SearchRequest()
                {
                    Fields = new List<PropertyPathMarker>() {new PropertyPathMarker() {Name = keyColumn}},
                    Query = searchItems.ToElasticSearchQuery(),
                    Sort = orders.ToElasticSearchSort(),
                    From = from,
                    Size = size
                };
                var data = searchClient.Search<string>(searchRequest);
                totalCount = data.Total;
                var result = new List<string>();
                foreach (var x in data.Hits)
                {
                    var messageValue = x.Fields.FieldValuesDictionary[keyColumn];
                    if (messageValue != null)
                    {
                        var array = messageValue as JArray;
                        result.Add(array?[0].ToString() ?? messageValue.ToString());
                    }
                }
                return result;
            }
            totalCount = 0;
            return new List<string>();
        }
    }
}
