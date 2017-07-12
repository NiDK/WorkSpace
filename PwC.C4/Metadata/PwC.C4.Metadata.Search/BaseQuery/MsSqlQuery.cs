using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Data;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search.Converter;

namespace PwC.C4.Metadata.Search.BaseQuery
{
    internal static class MsSqlQuery
    {
        internal static List<string> GetDataIds(string connName,string entity,string keyColumn, IList<SearchItem> searchItems, Dictionary<string, OrderMethod> orders, int from,
            int size, out long totalCount)
        {
            var where = "";
            if (searchItems != null && searchItems.Count > 0)
            {
                where = searchItems.ToSqlQuery();
            }
            var orderInfo = orders.ToSqlSort();
            var orderSql = string.IsNullOrEmpty(orderInfo) ? "ModifyDate Desc" : orderInfo;

            var db = Database.GetDatabase(connName);
            var rowcountParameter = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var dataIds = new List<string>();

            SafeProcedure.ExecuteAndGetInstanceList(db,
                "dbo.Metadata_GetDataSet",
                (IRecord record, Guid guid) => dataIds.Add(record.Get<string>(keyColumn))
                , new SqlParameter[]
                {

                            new SqlParameter("@TableName", entity),
                            new SqlParameter("@Columns", "[" + keyColumn + "]"),
                            new SqlParameter("@Where", where),
                            new SqlParameter("@OrderCol", ""),
                            new SqlParameter("@OrderType", ""),
                            new SqlParameter("@OtherOrder", orderSql),
                            new SqlParameter("@Start", from),
                            new SqlParameter("@Size", size),
                            rowcountParameter,
                }
                );
            totalCount = (int)rowcountParameter.Value;
            return dataIds;
        } 
    }
}
