using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PwC.C4.Infrastructure.WebExtension
{
    public class JsonBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //从请求中获取提交的参数数据 
            var json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName] as string;
            //提交参数是对象 
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                var jsonBody = JObject.Parse(json);
                var js = new JsonSerializer();
                var obj = js.Deserialize(jsonBody.CreateReader(), typeof(T));
                return obj;
            }
            //提交参数是数组 
            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                IList<T> list = new List<T>();
                var jsonRsp = JArray.Parse(json);
                if (jsonRsp == null) return list;
                foreach (var t in jsonRsp)
                {
                    var js = new JsonSerializer();
                    try
                    {
                        var obj = js.Deserialize(t.CreateReader(), typeof(T));
                        list.Add((T)obj);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return list;
            }
            return null;
        }
    }
}