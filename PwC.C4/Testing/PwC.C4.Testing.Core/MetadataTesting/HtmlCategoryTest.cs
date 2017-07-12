using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PwC.C4.DataService.Model;
using PwC.C4.Metadata.Service;

namespace PwC.C4.Testing.Core.MetadataTesting
{
    [TestClass]
    public class HtmlCategoryTest
    {
        [TestMethod]
        public void CreateOrUpdateCategory()
        {
            var item = new HtmlCategory
            {
                Id = Guid.NewGuid(),
                AppCode = "Sketch",
                Code = "0331",
                CreateBy = "Chenhui Yu",
                CreateTime = DateTime.Now,
                Description = "NoDesc",
                Func = "openForm();",
                DisplayName = "Authorisation for Services system(AFS)",
                IsCollapse = true,
                IsDeleted = false,
                ModifyBy = "Chenhui Yu",
                ModifyTime = DateTime.Now,
                Status = 0,
                Type = 0,
                ParentId = new Guid("c849aabb-cc31-4630-8e88-50f258b487a4"),
                Url = "",
                Parameters = "type=0331",
                Icon = "",
                Group = "Default",
                Order = 2
            };

            var i = HtmlCategoryService.Instance().HtmlCategory_Update(item);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void GetCategoryByCode()
        {
            var item = HtmlCategoryService.Instance().GetHtmlCategory_ByCode("Default", "031");

        }

        [TestMethod]
        public void GetCategoryById()
        {
            var item = HtmlCategoryService.Instance()
                .GetHtmlCategory_ById("Default", new Guid("ffb7ee20-de52-4893-8d47-49c706999b0c"));

        }

        [TestMethod]
        public void GetCategoryByListByP()
        {
            var item = HtmlCategoryService.Instance()
                .GetHtmlCategory_ListByParentId("Default", new Guid("67b9e9da-4e0c-4b85-b0ed-88f79e6864c4"));

        }

        [TestMethod]
        public void GetHtmlCategory_ListByAppCode()
        {
            var item = HtmlCategoryService.Instance().GetHtmlCategory_ListByAppCode("Default");

        }

        [TestMethod]
        public void GetHtmlCategory_ListByGroup()
        {
            var item = HtmlCategoryService.Instance().HtmlCategory_GetByGroup("Default", "TestStaffId");
        }

        [TestMethod]
        public void UpdateStats()
        {
            HtmlCategoryService.Instance()
                .UpdateCategoryCollaspseStatus("TestStaffId",
                    new List<Guid>() {new Guid("eaa4853a-b206-4a3d-8462-729b261cc87e")});
        }
    }
}
