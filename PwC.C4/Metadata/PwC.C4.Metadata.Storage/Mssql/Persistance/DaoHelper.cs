using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;

namespace PwC.C4.Metadata.Storage.Mssql.Persistance
{
    internal static class DaoHelper
    {
        private static readonly Dictionary<string, DataTable> DataTableDic;

        static DaoHelper()
        {
            if (DataTableDic == null)
                DataTableDic = new Dictionary<string, DataTable>();
        }

        internal static string ToSql(this IList<SearchItem> searchs)
        {
            var querySql = searchs.Aggregate("",
                    (current, searchItem) =>
                        current + CreateQuery(searchItem.Name, searchItem.Value, searchItem.Operator, searchItem.Method));
            return querySql;
        }

        internal static string ToSql(this Dictionary<string, OrderMethod> orders)
        {
            var sql = orders.Select(orderMethod => string.Format(" {0} {1} ", orderMethod.Key, orderMethod.Value == OrderMethod.Ascending ? "asc" : "desc")).ToList();
            return string.Join(",", sql);
        }

        private static string CreateQuery(string name, string value, string oper, string method)
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
                                value.Split(new string[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (!valueArray.Any())
                                valueArray =
                                    value.Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var newArray = new List<string>();
                            valueArray.ForEach(c => newArray.Add("'" + c + "'"));
                            queryFormat = "{0} {1} in ({2}) ";
                            var res = string.Format(queryFormat, method, name, string.Join(",", newArray));
                            return res;
                        case "intin":
                            var valueArrayInt =
                                value.Split(new string[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (!valueArrayInt.Any())
                                valueArrayInt =
                                    value.Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var newArrayInt = new List<string>();
                            valueArrayInt.ForEach(newArrayInt.Add);
                            queryFormat = "{0} {1} in ({2}) ";
                            var resInt = string.Format(queryFormat, method, name, string.Join(",", newArrayInt));
                            return resInt;
                        case "notin":
                            var notinvalueArray =
                                value.Split(new string[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (!notinvalueArray.Any())
                                notinvalueArray =
                                    value.Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            var newnotinvalueArray = new List<string>();
                            notinvalueArray.ForEach(c=>newnotinvalueArray.Add("'" + c + "'"));
                            queryFormat = " {0} {1} not in ('{2}') ";
                            var resNotInt = string.Format(queryFormat, method, name,
                                string.Join(",", newnotinvalueArray));
                            return resNotInt;
                        case "contains":
                            var list = new List<string>();
                            var item =
                                value.Split(new string[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            item.ForEach(c =>
                            {
                                list.Add(name + " like '%|C4|" + c + "'");
                                list.Add(name + " like '%|C4|" + c + "|C4|%'");
                                list.Add(name + " like '" + c + "|C4|%'");
                                list.Add(name + "='" + c + "'");
                            });
                            var str = string.Join(" Or ", list);
                            queryFormat = string.Format(" ( {0} ) ", str);
                            break;
                        default:
                            queryFormat = " {0} = '{1}' ";
                            break;
                    }
                    sql = sql + string.Format(queryFormat, name, value);
                    return sql;
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

        //public static DataTable ToTable<T>(this List<T> data)
        //{
        //    var entityName = "";
        //    var info = typeof (T);
        //    try
        //    {
                
        //        var classAttribute =
        //            (MetaObjectAttribute) Attribute.GetCustomAttribute(info, typeof (MetaObjectAttribute));
        //        entityName = classAttribute.Name;
        //    }
        //    catch
        //    {
        //        throw new NoMetadataEntityException(info.FullName);
        //    }
        //    var dataTable = new DataTable();
        //    if (DataTableDic.ContainsKey(entityName))
        //        dataTable = DataTableDic[entityName];
        //    else
        //    {
        //        var entity = MetadataSettings.Instance.GetEntity(entityName);
        //        entity.Columns.ForEach(c => dataTable.Columns.Add(c.Name, TypeHelper.GetType(c.Type)));
        //    }
        //    foreach (var id in data)
        //    {
        //        id.
        //        dataTable.Rows.Add()
        //    }
        //    return dataTable;
        //}
    }
}
