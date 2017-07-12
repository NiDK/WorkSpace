using System;
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.Infrastructure.Logger
{
    public static class MetadataLogWrapper
    {

        private static readonly C4LogServiceClient C4Client = null;

        static MetadataLogWrapper()
        {
            if (C4Client == null)
            {
                C4Client = new C4LogServiceClient();
            }
        }


        public static void Log(string appcode, string metadataobject, object dataId, MetadataLogType method, string json,
            string userId)
        {
            C4Client.Log_FortMetadata_Insert(appcode,metadataobject,dataId,method,json,userId);
        }
    }
}
