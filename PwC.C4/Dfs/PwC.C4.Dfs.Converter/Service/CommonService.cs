using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Converter.Model;
using PwC.C4.Dfs.Converter.Persistance;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Converter.Service
{
    public static class CommonService
    {
        public static void UpdateConvertInfoToDb(DfsPath dfs, DateTime startTime, string mode, object para,string result)
        {
            var newFileDfs = new DfsPath(dfs.Keyspace, dfs.AppCode, dfs.FileId + "-" + mode, dfs.FileExtension);
            BaseDao.UpdateConvertion(dfs.FileId, new ConvertionInfo()
            {
                ConvertDfsPath = newFileDfs.ToString(),
                ConvertFileName = dfs.FileId + "-" + mode + '.' + dfs.FileExtension,
                ConvertFinishTime = DateTime.Now,
                ConvertMode = mode,
                ConvertPara = JsonHelper.Serialize(para),
                ConvertServer = System.Net.Dns.GetHostName(),
                ConvertStatTime = startTime
            }, result);
        }

        public static void ReConvertAllData()
        {
            BaseDao.ReConvertAllData();
        }
    }
}
