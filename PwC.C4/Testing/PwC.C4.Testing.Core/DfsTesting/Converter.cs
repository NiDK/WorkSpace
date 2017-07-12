using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Dfs.Converter;
using PwC.C4.Dfs.Converter.Model;
using PwC.C4.Dfs.Converter.Service;

namespace PwC.C4.Testing.Core.Dfs
{
    [TestClass]
    public class Converter
    {
        [TestMethod]
        public void TestMethod1()
        {
            var da = new ConvertionInfo()
            {
                ConvertDfsPath = "dfs://Image/InspireC4/79fef83eba424182b14cd19aa8a45360-m.jpg",
                ConvertFileName = "79fef83eba424182b14cd19aa8a45360-m.jpg",
                ConvertFinishTime = new DateTime(2016, 2, 22, 10, 0, 0),
                ConvertMode = "M",
                ConvertPara = "500x500",
                ConvertServer = "Server1",
                ConvertStatTime = new DateTime(2016, 2, 22, 9, 0, 0),
            };
            C4.Dfs.Converter.Persistance.BaseDao.UpdateConvertion("79fef83eba424182b14cd19aa8a45360", da
                , "");
        }
        [TestMethod]
        public void TestVideo()
        {
            var convertServer = new FileProcessProvider();
            convertServer.StartScheduler();
        }

        [TestMethod]
        public void TestImage()
        {
            var dfs = "dfs://Image/InspireC4/P60223-111904.jpg";
            //ImageResizeProvider.Resize(dfs);
            CommonService.ReConvertAllData();
        }

    }
}
