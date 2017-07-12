using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using Nest;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Search.Converter
{
    public static class SortConverter
    {
        public static IList<KeyValuePair<PropertyPathMarker, ISort>> ToElasticSearchSort(
            this Dictionary<string, OrderMethod> orders)
        {
            var or = new List<KeyValuePair<PropertyPathMarker, ISort>>();
            foreach (var keyValuePair in orders)
            {
                var orderMethod = keyValuePair.Value == OrderMethod.Ascending
                    ? SortOrder.Ascending
                    : SortOrder.Descending;
                or.Add(new KeyValuePair<PropertyPathMarker, ISort>(keyValuePair.Key,
                    new Sort { Order = orderMethod, Missing = "_first" }));
            }
            return or;
        }

        public static SortByBuilder ToMongoSort(this Dictionary<string, OrderMethod> orders)
        {
            var builder = new SortByBuilder();
            foreach (var orderMethod in orders)
            {
                if (orderMethod.Value == OrderMethod.Ascending)
                {
                    builder.Ascending(orderMethod.Key);
                }
                else
                {
                    builder.Descending(orderMethod.Key);
                }
            }
            return builder;
        }


        public static string ToSqlSort(this Dictionary<string, OrderMethod> orders)
        {
            var sql = orders.Select(orderMethod =>
                $" {orderMethod.Key} {(orderMethod.Value == OrderMethod.Ascending ? "asc" : "desc")} ").ToList();
            return string.Join(",", sql);
        }
    }
}