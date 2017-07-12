using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Service;
using PwC.C4.Model;

namespace PwC.C4.UnitTest.MetadataTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MetadataEntityConfigTest()
        {
            try
            {
                var data = MetadataSettings.Instance.GetEntity("Entity_Goods");
            }
            catch (Exception ee)
            {
                var str = ee.ToString();
                throw;
            }

        }

        [TestMethod]
        public void MetadataSaveTest()
        {
            try
            {
                var model = new GoodsInfo()
                {
                    GoodsId = new Guid("08E93267-CF3D-4638-A824-58A48ED04370"),

                    IsDeleted = true
                };
                var data = EntityService.Instance().SaveEntity<GoodsInfo>(model);
            }
            catch (Exception ee)
            {
                var str = ee.ToString();
                throw;
            }

        }


        [TestMethod]
        public void MetadataGetEntityTest()
        {
            try
            {

                var data = EntityService.Instance().GetEntity<GoodsInfo>(new Guid("34341480-C07E-4E95-B8B7-5397BDCE174D"),new List<string>() );
            }
            catch (Exception ee)
            {
                var str = ee.ToString();
                throw;
            }

        }

        [TestMethod]
        public void MetadataInterfaceDataSourceTest()
        {
            try
            {
                var data =
                    DataSourceService.Instance()
                        .GetInterfaceDataSource("PwC.C4.ServiceImp,PwC.C4.ServiceImp.Service.CustomGoodsTypeDataSource");
            }
            catch (Exception ee)
            {
                var str = ee.ToString();
                throw;
            }

        }
    }
}
