using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Model.Const;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Search;
using PwC.C4.Metadata.Storage;

namespace PwC.C4.Testing.Metadata
{
    [TestClass]
    public class SearchTesting
    {
        [TestMethod]
        public void GoSearch()
        {
            var si = new List<SearchItem>()
            {
                new SearchItem()
                {
                    Method = SearchItemMethod.And,
                    Name = "Menu1",
                    Value = "a261679e-1354-4eb3-8bb8-d45b86ddda96",
                    Operator = SearchItemOperator.Equal
                },
                new SearchItem()
                {
                    Method = SearchItemMethod.And,
                    Name = "Menu2",
                    Value = "",
                    Operator = SearchItemOperator.Equal
                },
                new SearchItem()
                {
                    Method = SearchItemMethod.And,
                    Name = "Menu3",
                    Value = "",
                    Operator = SearchItemOperator.Equal
                },
                new SearchItem()
                {
                    Method = SearchItemMethod.And,
                    Name = "Status",
                    Value = "Relese",
                    Operator = SearchItemOperator.Equal
                }
            };
            long totl = 0;
            var or = new Dictionary<string,OrderMethod>() { { "DocSubject", OrderMethod.Ascending} };
            var p = ProviderFactory.GetProvider<IEntityService>("dbconn.AdvRQPortal", "Form_frmWorkingStandDoc");
            var totalCount = 0L;
            var threadTempDic = new Dictionary<string, Dictionary<object, object>>();
            var data = p.GetEntitesTranslatedWithSearch<DynamicMetadata>(new List<SearchItem>(), or, new List<string>(), 0, 500,
                    threadTempDic, new string[] { }, null, out totalCount);
        }
    }
}
