using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ImageResizer;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Web.ApiHelper.Models.ApiModel;
using PwC.C4.Web.ApiHelper.Service;
using PwC.C4.Web.ApiHelper.Service.People;

namespace PwC.C4.Web.ApiHelper.Controllers
{
    public class ValuesController : ApiController
    {

        [HttpPost]
        public object Fetch(FetchModel m,string method)
        {
            var d = new List<Dictionary<string, object>>();
            int totcalCount = 0;

            var result = "Empty";
            var msg = "No data";
            var data = PeopleService.StaffBankList(m, method, out totcalCount);
            if (data.Any())
            {
                data.ForEach(t =>
                {
                    var dic = t.ToDictionary(m.P, true);
                    d.Add(dic);
                });
            }
            if (data.Any())
            {
                result = "Success";
                msg = "Success";
            }
            return new { Result = result, Message = msg, Data = d, Count = totcalCount, CurrentPage = m.I };
        }

        [HttpPost]
        public object Search(FetchModel m, string method)
        {
            var d = new List<Dictionary<string, object>>();
            int totcalCount = 0;
            
            var result = "Empty";
            var msg = "No data";
            
            var data = PeopleService.StaffBankList(m, method, out totcalCount);
            if (data.Any())
            {
                data.ForEach(t =>
                {
                    var dic = t.ToDictionary(m.P, true);
                    d.Add(dic);
                });
            }
            if (data.Any())
            {
                result = "Success";
                msg = "Success";
            }
            return new {Result = result, Message = msg, Data = d, Count = totcalCount, CurrentPage = m.I};
        }

        [HttpPost]
        public object Pic(FetchModel m, string method)
        {
            var d = new List<Dictionary<string, object>>();

            var result = "Empty";
            var msg = "No data";
            m.P = new List<string>() {"StaffId","Pic","PicType"};
            var data = PeopleService.GetStaffPic(m, method);
            if (data.Any())
            {
                data.ForEach(t =>
                {
                    var dic = new Dictionary<string, object> {{"StaffId", t.StaffId}};
                    if (t.Pic != null)
                    {
                        var img = ImageResizeService.GetImageBase64(t.Pic, m.Ico);
                        var type = t.PicType;
                        var picbase64 = $"data:image/{type};base64,{img}";
                        dic.Add("StaffPhoto", picbase64);
                        dic.Add("HasPhoto", "True");
                    }
                    else
                    {
                        dic.Add("HasPhoto", "False");
                    }
                    d.Add(dic);
                });
                result = "Success";
                msg = "";
            }
            return new { Result = result, Message = msg, Data = d, Count = m.KeyArray.Count, CurrentPage = m.I };
        }
    }
}
