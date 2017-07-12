using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Nest;
using Nest.DSL.Visitor;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search.BaseFunction;

namespace PwC.C4.Metadata.Search.Converter
{
    public static class SearchItemConverter
    {
        static readonly LogWrapper Log = new LogWrapper();

        public static QueryContainer ToElasticSearchQuery(this IList<SearchItem> searchItems)
        {
            var query = BuildElasticSearchQuery(searchItems);
            return query.ToContainer();
        }

        private static BoolQuery BuildElasticSearchQuery(IList<SearchItem> searchItems)
        {
            var bq = new BoolQuery();
            var mustQuery = new List<IQueryContainer>();
            var mustNotQuery = new List<IQueryContainer>();
            var shouldQuery = new List<IQueryContainer>();
            foreach (var searchItem in searchItems)
            {
                if (searchItem.Method == "AND" && !searchItem.Operator.Contains("NOT",StringComparison.OrdinalIgnoreCase))
                {
                    mustQuery.AddRange(BaseElasticSearchQuery(searchItem));
                }
                else if (searchItem.Method == "AND" &&
                         searchItem.Operator.Contains("NOT", StringComparison.OrdinalIgnoreCase))
                {
                    mustNotQuery.AddRange(BaseElasticSearchQuery(searchItem));
                }
                else
                {
                    shouldQuery.AddRange(BaseElasticSearchQuery(searchItem));
                }
            }
            bq.Must = mustQuery;
            bq.MustNot = mustNotQuery;
            bq.Should = shouldQuery;
            return bq;
        }

        private static List<IQueryContainer> BaseElasticSearchQuery(SearchItem searchItem)
        {
            var queryList = new List<IQueryContainer>();
            if (!string.IsNullOrEmpty(searchItem.Operator) && !string.IsNullOrEmpty(searchItem.Name) &&
                !string.IsNullOrEmpty(searchItem.Value))
            {
                var query =
                    CreateElasticSearchQuery(searchItem.Name, searchItem.Value, searchItem.Operator)
                        .ToContainer();
                queryList.Add(query);
            }
            if (searchItem.SubSearchItems != null && searchItem.SubSearchItems.Any())
            {
                var subBq = BuildElasticSearchQuery(searchItem.SubSearchItems);
                queryList.Add(subBq.ToContainer());
            }
            return queryList;
        }

        private static PlainQuery CreateElasticSearchQuery(string name, string value, string oper)
        {
            if (oper.StartsWith("not", StringComparison.OrdinalIgnoreCase))
            {
                oper = oper.Substring(2, oper.Length - 3);
            }
            switch (oper.ToLower().Trim())
            {
                case "equal":
                case "intequal":
                    return new TermQuery
                    {
                        Field = name,
                        Value = value
                    };
                case "contains":
                case "like":
                    return new MatchQuery()
                    {
                        Field = name,
                        Query = value,
                    };
                case "in":
                case "intin":
                    var valueArrayInt = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!valueArrayInt.Any())
                        valueArrayInt =
                            value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    return new TermsQuery()
                    {
                        Field = name,
                        Terms = valueArrayInt
                    };
                case "range":
                    var valueRange = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!valueRange.Any() || valueRange.Count != 2)
                        valueRange = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!valueRange.Any() || valueRange.Count != 2)
                        return null;
                    var rq = new RangeQuery()
                    {
                        Field = name,
                        GreaterThanOrEqualTo = valueRange[0],
                        LowerThanOrEqualTo = valueRange[1]
                    };
                    return rq;

                default:
                    return  new TermQuery
                    {
                        Field = name,
                        Value = value
                    }; ;

            }
        }

        public static IMongoQuery ToMongoQuery(this IList<SearchItem> searchItems, string entityName)
        {
            return Builder(searchItems, "AND", entityName);
        }

        private static IMongoQuery Builder(IEnumerable<SearchItem> searchs, string method, string entityName)
        {

            IMongoQuery query = null;
            if (searchs == null || !searchs.Any())
            {
                return new QueryDocument();
            }
            foreach (var searchItem in searchs)
            {
                IMongoQuery q = null;
                if (searchItem.SubSearchItems != null && searchItem.SubSearchItems.Any())
                {
                    q = Builder(searchItem.SubSearchItems, searchItem.Method, entityName);
                }
                else
                {

                    var enti = MetadataSettings.Instance.GetColumn(entityName, searchItem.Name);
                    if (enti == null)
                    {
                        Log.Error("Column:" + searchItem.Name + " not in entity:" + entityName);
                    }
                    else
                    {
                        q = CreateMongoQuery(searchItem.Name, enti.Type, searchItem.Value, searchItem.Operator);
                    }

                }
                if (q == null) continue;

                switch (searchItem.Method.ToLower())
                {
                    case "and":
                        query = query == null ? Query.And(q) : Query.And(new IMongoQuery[] { query, q });
                        break;
                    case "or":
                        query = query == null ? Query.Or(q) : Query.Or(new IMongoQuery[] { query, q });
                        break;
                }
            }
            switch (method.ToLower())
            {
                case "and":
                    return Query.And(query);
                case "or":
                    return Query.Or(query);
                default:
                    return Query.And(query);
            }

        }

        private static IMongoQuery CreateMongoQuery(string name, string type, string value, string oper)
        {

            switch (oper.ToLower().Trim())
            {
                case "equal":
                case "intequal":
                    var objectValue = MongoTypeUtilities.BsonValueConverter(type, value);
                    return Query.EQ(name, objectValue);
                case "like":
                    return Query.Matches(name, new BsonRegularExpression(value, "i"));
                case "intin":
                    var valueArrayInt = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!valueArrayInt.Any())
                        valueArrayInt =
                            value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var newArrayInt = new List<BsonValue>();
                    valueArrayInt.ForEach(c =>
                    {
                        var v0 = 0;
                        int.TryParse(c, out v0);
                        newArrayInt.Add(v0);
                    }
                        );
                    return Query.In(name, newArrayInt);
                case "in":
                    var valueArrayIn = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!valueArrayIn.Any())
                        valueArrayIn =
                            value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var newArrayIn = new BsonArray();
                    valueArrayIn.ForEach(c =>
                    {
                        var obj = MongoTypeUtilities.BsonValueConverter(type, c); ;
                        newArrayIn.Add(obj);
                    });
                    return Query.In(name, newArrayIn);
                case "contains":
                    return Query.Matches(name, new BsonRegularExpression(new Regex(value)));
                case "notin":
                    var valueArray = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (!valueArray.Any())
                        valueArray = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var newArray = new BsonArray();
                    valueArray.ForEach(c =>
                    {
                        var obj = MongoTypeUtilities.BsonValueConverter(type, c); ;
                        newArray.Add(obj);
                    });
                    return Query.NotIn(name, newArray);
                case "range":
                    var valueRange = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (!valueRange.Any() || valueRange.Count != 2)
                        valueRange = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (!valueRange.Any() || valueRange.Count != 2)
                        return null;

                    var fir = MongoTypeUtilities.BsonValueConverter(type, valueRange[0]);
                    var las = MongoTypeUtilities.BsonValueConverter(type, valueRange[1]);
                    return Query.And(Query.GTE(name, fir), Query.LTE(name, las));

                default:
                    return Query.EQ(name, value);

            }
        }


        public static string ToSqlQuery(this IList<SearchItem> searchItems)
        {
            var querySql = searchItems.Aggregate("",
                    (current, searchItem) =>
                        current + CreateSqlQuery(searchItem.Name, searchItem.Value, searchItem.Operator, searchItem.Method));
            return querySql;
        }

        private static string CreateSqlQuery(string name, string value, string oper, string method)
        {
            switch (method)
            {
                case "ALB":
                    return " And ( ";
                case "RLB":
                    return " Or ( ";
                case "RB":
                    return " ) ";
                default:
                    var sql = method;
                    var queryFormat = "";
                    switch (oper.ToLower().Trim())
                    {
                        case "equal":
                            queryFormat = " {0} = '{1}' ";
                            break;
                        case "intequal":
                            queryFormat = " {0} = {1} ";
                            break;
                        case "like":
                            queryFormat = " {0} like '%{1}%' ";
                            break;
                        case "in":
                            var valueArray =
                                value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (!valueArray.Any())
                                valueArray =
                                    value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var newArray = new List<string>();
                            valueArray.ForEach(c => newArray.Add("'" + c + "'"));
                            queryFormat = "{0} {1} in ({2}) ";
                            var res = string.Format(queryFormat, method, name, string.Join(",", newArray));
                            return res;
                        case "intin":
                            var valueArrayInt =
                                value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (!valueArrayInt.Any())
                                valueArrayInt =
                                    value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var newArrayInt = new List<string>();
                            valueArrayInt.ForEach(newArrayInt.Add);
                            queryFormat = "{0} {1} in ({2}) ";
                            var resInt = string.Format(queryFormat, method, name, string.Join(",", newArrayInt));
                            return resInt;
                        case "notin":
                            var notinvalueArray =
                                value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (!notinvalueArray.Any())
                                notinvalueArray =
                                    value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var newnotinvalueArray = new List<string>();
                            notinvalueArray.ForEach(c => newnotinvalueArray.Add("'" + c + "'"));
                            queryFormat = " {0} {1} not in ('{2}') ";
                            var resNotInt = string.Format(queryFormat, method, name,
                                string.Join(",", newnotinvalueArray));
                            return resNotInt;
                        case "contains":
                            var list = new List<string>();
                            var item =
                                value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            item.ForEach(c =>
                            {
                                list.Add(name + " like '%|C4|" + c + "'");
                                list.Add(name + " like '%|C4|" + c + "|C4|%'");
                                list.Add(name + " like '" + c + "|C4|%'");
                                list.Add(name + "='" + c + "'");
                            });
                            var str = string.Join(" Or ", list);
                            queryFormat = $" ( {str} ) ";
                            break;
                        default:
                            queryFormat = " {0} = '{1}' ";
                            break;
                    }
                    sql = sql + string.Format(queryFormat, name, value);
                    return sql;
            }


        }


    }
}
