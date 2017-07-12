using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace PwC.C4.Infrastructure.Helper
{
    public class DataTableHelper
    {
        public static DataTable AppendMapColumns(DataTable datatable, string fieldid, string fieldparentid, string defaultvalue)
        {
            datatable.Columns.Add("index", typeof(int));
            datatable.Columns.Add("mapid", typeof(int));
            int index = 0;
            AppendMapColumns(datatable.DefaultView, fieldid, fieldparentid, defaultvalue, ref index);
            return datatable;
        }
        public static void AppendMapColumns(DataView dv, string fieldid, string fieldparentid, string defaultvalue, ref int index)
        {
            dv.RowFilter = string.Format("{0}='{1}'", fieldid, defaultvalue);
            int mapid = 0;
            if (dv.Count > 0)
            {
                mapid = (int)dv[0]["index"];
            }

            dv.RowFilter = string.Format("{0}='{1}'", fieldparentid, defaultvalue);
            foreach (DataRowView drv in dv)
            {
                string parentid = drv[fieldid].ToString();

                drv.Row["index"] = ++index;
                drv.Row["mapid"] = mapid;
                AppendMapColumns(dv, fieldid, fieldparentid, parentid, ref index);
            }

        }
        public static DataTable GetDataTable<T>(List<T> list) where T : class
        {
            DataTable dt = new DataTable();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo p in properties)
            {
                dt.Columns.Add(p.Name);
            }
            foreach (T t in list)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo p in properties)
                {
                    dr[p.Name] = p.GetValue(t, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
