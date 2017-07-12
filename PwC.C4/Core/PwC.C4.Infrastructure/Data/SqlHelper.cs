using System;
using System.Collections.Generic;
using System.Data;

namespace PwC.C4.Infrastructure.Data
{
    public static class SqlHelper
    {
        public static readonly string[] IllegalWords = {
            "'",
            "<",
            ">",
            ";", 
            "(", 
            ")", 
            "* ", 
            "% ", 
            "--",
            "and ",
            "or ",
            "select ",
            "update ", 
            "delete ", 
            "drop ", 
            "create ", 
            "union ", 
            "insert ", 
            "net ", 
            "truncate ", 
            "exec ", 
            "declare ", 
            "and ", 
            "count ", 
            "chr ", 
            "mid ", 
            "master ", 
            "char "
        };

        /// <summary>
        /// Check if the sql parameter value contains illegal words
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// true: The parameter value does NOT contain illegal words
        /// false: The parameter value does contain illegal words
        /// </returns>
        public static bool CheckSqlParameter(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (var word in IllegalWords)
                {
                    if (value.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                        return false;
                }
            }

            return true;
        }
        public static DataTable ToStrIdTable(this IList<string> ids)
        {
            if (ids == null) throw new ArgumentNullException("ids");

            var table = new DataTable("StrIdTable");
            table.Columns.Add(new DataColumn("ID", typeof(string)));
            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }

        private static DataTable ToGuidDataTable(IEnumerable<Guid> ids)
        {
            if (ids == null) throw new ArgumentNullException("ids");

            var table = new DataTable("GuidIdTable");
            table.Columns.Add(new DataColumn("ID", typeof(Guid)));
            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }

        public static DataTable ToGuidIdTable(this IList<Guid> ids)
        {
            return ToGuidDataTable(ids);
        }

        public static DataTable ToGuidIdTable(this IList<string> ids)
        {

            if (ids == null) throw new ArgumentNullException("ids");
            var glist = new List<Guid>();
            foreach (var id in ids)
            {
                Guid e;
                if (Guid.TryParse(id, out e))
                {
                    glist.Add(e);
                }
            }
            return ToGuidDataTable(glist);
        }
    }
}
