using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Metadata.Storage
{
    internal static class CommonService
    {
        static readonly LogWrapper Log = new LogWrapper();
        internal static void InsertMetadataLog(string appcode, string metadataobject, string dataId, PwC.C4.DataService.Model.Enum.MetadataLogType method,
            string json,
            string userId)
        {
            try
            {
                MetadataLogWrapper.Log(appcode, metadataobject, dataId, method, json, userId);
            }
            catch (Exception ee)
            {
                Log.Error("InsertMetadataLog error", ee);
            }
        }
    }
}
