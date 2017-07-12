using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.Testing.Core.InfrastructureTesting
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void TestGetInst()
        {
           //var data = PwC.C4.DataService.Persistance.StaffMasterDao.GetStaffInfo("CN112076");
        }

        public class SAndPMapping
        {
            /// <summary>
            /// 
            /// </summary>
            public string PartnerName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string SecretaryId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PartnerId { get; set; }

        }

        [TestMethod]
        public void GetSAndPMappings()
        {

            Database db = Database.GetDatabase(ConfigConstValues.AppConnStrName);
            List<SAndPMapping> list = SafeProcedure.ExecuteAndGetInstanceList<SAndPMapping>(db,
                "dbo.Secretary_Partner_Mapping_GetPartners",
                MapperSAndPMapping,
                new SqlParameter[]
                {
                    new SqlParameter("@secretaryId", "CN410831")
                }
                );
        }

        public void MapperSAndPMapping(IRecord record, SAndPMapping entity)
        {
            entity.PartnerId = record.Get<string>("PartnerId");
            entity.PartnerName = record.Get<string>("PartnerName");
            entity.SecretaryId = record.Get<string>("SecretaryId");
        }
    }
}
