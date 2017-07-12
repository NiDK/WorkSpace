using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PwC.C4.Infrastructure.WebExtension
{
    ///<summary>
    /// Controller扩展
    ///</summary>
    public static class ControllerExtension
    {
        public static JsonpViewResult JsonpView(this Controller controller)
        {
            return controller.JsonpView(null /* viewName */, null /* masterName */, null /* model */);
        }

        public static JsonpViewResult JsonpView(this Controller controller, object model)
        {
            return controller.JsonpView(null /* viewName */, null /* masterName */, model);
        }

        public static JsonpViewResult JsonpView(this Controller controller, string viewName)
        {
            return controller.JsonpView(viewName, null /* masterName */, null /* model */);
        }

        public static JsonpViewResult JsonpView(this Controller controller, string viewName, string masterName)
        {
            return controller.JsonpView(viewName, masterName, null /* model */);
        }

        public static JsonpViewResult JsonpView(this Controller controller, string viewName, object model)
        {
            return controller.JsonpView(viewName, null /* masterName */, model);
        }

        public static JsonpViewResult JsonpView(this Controller controller, string viewName, string masterName, object model)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }

            return new JsonpViewResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "The method name 'View' is a convenient shorthand for 'CreateJsonpViewResult'.")]
        public static JsonpViewResult JsonpView(this Controller controller, IView view)
        {
            return controller.JsonpView(view, null /* model */);
        }

        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", MessageId = "0#",
            Justification = "The method name 'View' is a convenient shorthand for 'CreateViewResult'.")]
        public static JsonpViewResult JsonpView(this Controller controller, IView view, object model)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }

            return new JsonpViewResult
            {
                View = view,
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };
        }
        /// <summary>
        /// 返回可接受跨域调用的Json数据
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data)
        {
            return Jsonp(controller, data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回可接受跨域调用的Json数据
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">返回的数据</param>
        /// <param name="behavior">是否支持get请求</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, JsonRequestBehavior behavior)
        {
            return new FastJsonResult() { Data = data, JsonRequestBehavior = behavior };
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">数据对象</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly)
        {
            return Jsonp(controller, data, serializeProperties, specificPropertiesOnly, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">数据对象</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <param name="behavior">是否支持get请求</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonRequestBehavior behavior)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = behavior };
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">数据对象</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data,JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet,CustomeSerializeSetting = settings};
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">数据对象</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = JsonRequestBehavior.AllowGet,CustomeSerializeSetting = settings};
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">数据对象</param>
        /// <param name="contractResolver">自定义序列化定制接口</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data,IContractResolver contractResolver,  JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet, CustomeSerializeSetting = settings };
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">数据对象</param>
        /// <param name="contractResolver">自定义序列化定制接口</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, IContractResolver contractResolver)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// 自定义序列化-针对DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">源数据</param>
        /// <param name="draw">表格随机数</param>
        /// <param name="recordsTotal">总数量</param>
        /// <param name="recordsFiltered">显示数量</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered)
        {
            return Jsonp<T>(controller, data, draw, recordsTotal, recordsFiltered, null /* serializeProperties*/,
                            true, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 自定义序列化-针对DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">源数据</param>
        /// <param name="draw">表格随机数</param>
        /// <param name="recordsTotal">总数量</param>
        /// <param name="recordsFiltered">显示数量</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <param name="behavior">是否支持get请求</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonRequestBehavior behavior)
        {
            return new FastJsonResult()
            {
                Data = new DtGridModel<T>()
                           {
                               Data = data,
                               Draw = draw,
                               RecordsTotal = recordsTotal,
                               RecordsFiltered = recordsFiltered

                           },
                SerializeProperties = serializeProperties,
                SpecificPropertiesOnly = specificPropertiesOnly,
                JsonRequestBehavior = behavior
            };
        }
        /// <summary>
        /// 自定义序列化-针对DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel数据</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <param name="behavior">是否支持get请求</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonRequestBehavior behavior)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = behavior };
        }
        /// <summary>
        /// 自定义序列化-针对DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel数据</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = JsonRequestBehavior.AllowGet,CustomeSerializeSetting = settings};
        }
        /// <summary>
        /// 自定义序列化-针对DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel数据</param>
        /// <param name="contractResolver">自定义序列化定制接口</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, IContractResolver contractResolver, JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet, CustomeSerializeSetting = settings };
        }
        /// <summary>
        /// 自定义序列化-针对DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel数据</param>
        /// <param name="contractResolver">自定义序列化定制接口</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, IContractResolver contractResolver)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">源数据</param>
        /// <param name="draw">表格随机数</param>
        /// <param name="recordsTotal">总数量</param>
        /// <param name="recordsFiltered">显示数量</param>
        /// <param name="contractResolver">自定义序列化定制接口</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered, IContractResolver contractResolver)
        {
            return new FastJsonResult()
            {
                Data = new DtGridModel<T>()
                {
                    Data = data,
                    Draw = draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered

                },
                ContractResolver = contractResolver
            };
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">源数据</param>
        /// <param name="draw">表格随机数</param>
        /// <param name="recordsTotal">总数量</param>
        /// <param name="recordsFiltered">显示数量</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered, JsonSerializerSettings settings)
        {


            return new FastJsonResult()
            {
                Data = new DtGridModel<T>()
                {
                    Data = data,
                    Draw = draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered

                },
                CustomeSerializeSetting = settings
            };
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">源数据</param>
        /// <param name="draw">表格随机数</param>
        /// <param name="recordsTotal">总数量</param>
        /// <param name="recordsFiltered">显示数量</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <param name="serializeProperties">字段属性</param>
        /// <param name="specificPropertiesOnly">是否仅序列化该字典字段</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered, JsonSerializerSettings settings, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly)
        {


            return new FastJsonResult()
            {
                Data = new DtGridModel<T>()
                {
                    Data = data,
                    Draw = draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered

                },
                CustomeSerializeSetting = settings,
                SerializeProperties = serializeProperties,
                SpecificPropertiesOnly = specificPropertiesOnly
            };
        }
        /// <summary>
        /// 自定义序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">源数据</param>
        /// <param name="draw">表格随机数</param>
        /// <param name="recordsTotal">总数量</param>
        /// <param name="recordsFiltered">显示数量</param>
        /// <param name="settings">自定义序列化设置参数</param>
        /// <param name="contractResolver">自定义序列化定制接口</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered, JsonSerializerSettings settings, IContractResolver contractResolver)
        {


            return new FastJsonResult()
            {
                Data = new DtGridModel<T>()
                {
                    Data = data,
                    Draw = draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered

                },
                CustomeSerializeSetting = settings,
                ContractResolver = contractResolver
            };
        }
    }
}