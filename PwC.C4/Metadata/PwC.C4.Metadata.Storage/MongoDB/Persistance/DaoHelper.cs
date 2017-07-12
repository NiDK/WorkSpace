using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Wrappers;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Storage.MongoDB;

namespace PwC.C4.Metadata.Storage.MongoDb.Persistance
{

#if DEBUG
    public
#else
        internal
#endif

 static class DaoHelper
    {
        private static readonly Dictionary<string, DataTable> DataTableDic;
        static readonly LogWrapper Log= new LogWrapper();
        static DaoHelper()
        {
            if (DataTableDic == null)
                DataTableDic = new Dictionary<string, DataTable>();
        }

        public static SortByBuilder ToSort(this Dictionary<string, OrderMethod> orders)
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


        public static IMongoQuery ToQuery(this IList<SearchItem> searchs,string entityName)
        {
            return Builder(searchs, "AND", entityName);
        }

        private static IMongoQuery Builder(IEnumerable<SearchItem> searchs, string method, string entityName)
        {
            // var query = new List<IMongoQuery>();
            IMongoQuery query = null;
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
                        q = CreateQuery(searchItem.Name, enti.Type, searchItem.Value, searchItem.Operator);
                    }

                }
                if (q == null) continue;
                
                switch (searchItem.Method.ToLower())
                {
                    case "and":
                        query = query == null ? Query.And(q) : Query.And(new IMongoQuery[] {query, q});
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

        private static IMongoQuery CreateQuery(string name,string type, string value, string oper)
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
                            value.Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
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
                            value.Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
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
                    var valueArray = value.Split(new string[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    
                    if (!valueArray.Any())
                        valueArray = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var newArray = new BsonArray() ;
                    valueArray.ForEach(c =>
                    {
                        var obj = MongoTypeUtilities.BsonValueConverter(type, c); ;
                        newArray.Add(obj);
                    });
                    return Query.NotIn(name, newArray);
                case "range":
                    var valueRange = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (!valueRange.Any() || valueRange.Count!=2)
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

        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch 
            {
                return null;
            }
        }

    }
}
