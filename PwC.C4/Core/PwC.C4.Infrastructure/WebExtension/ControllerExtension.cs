using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PwC.C4.Infrastructure.WebExtension
{
    ///<summary>
    /// Controller��չ
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
        /// ���ؿɽ��ܿ�����õ�Json����
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ص�����</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data)
        {
            return Jsonp(controller, data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// ���ؿɽ��ܿ�����õ�Json����
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ص�����</param>
        /// <param name="behavior">�Ƿ�֧��get����</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, JsonRequestBehavior behavior)
        {
            return new FastJsonResult() { Data = data, JsonRequestBehavior = behavior };
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ݶ���</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly)
        {
            return Jsonp(controller, data, serializeProperties, specificPropertiesOnly, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ݶ���</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
        /// <param name="behavior">�Ƿ�֧��get����</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonRequestBehavior behavior)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = behavior };
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ݶ���</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data,JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet,CustomeSerializeSetting = settings};
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ݶ���</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = JsonRequestBehavior.AllowGet,CustomeSerializeSetting = settings};
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ݶ���</param>
        /// <param name="contractResolver">�Զ������л����ƽӿ�</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data,IContractResolver contractResolver,  JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet, CustomeSerializeSetting = settings };
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="data">���ݶ���</param>
        /// <param name="contractResolver">�Զ������л����ƽӿ�</param>
        /// <returns></returns>
        public static JsonResult Jsonp(this Controller controller, object data, IContractResolver contractResolver)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// �Զ������л�-���DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">Դ����</param>
        /// <param name="draw">��������</param>
        /// <param name="recordsTotal">������</param>
        /// <param name="recordsFiltered">��ʾ����</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, IList<T> data, int draw, int recordsTotal, int recordsFiltered)
        {
            return Jsonp<T>(controller, data, draw, recordsTotal, recordsFiltered, null /* serializeProperties*/,
                            true, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// �Զ������л�-���DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">Դ����</param>
        /// <param name="draw">��������</param>
        /// <param name="recordsTotal">������</param>
        /// <param name="recordsFiltered">��ʾ����</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
        /// <param name="behavior">�Ƿ�֧��get����</param>
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
        /// �Զ������л�-���DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel����</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
        /// <param name="behavior">�Ƿ�֧��get����</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonRequestBehavior behavior)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = behavior };
        }
        /// <summary>
        /// �Զ������л�-���DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel����</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, Dictionary<Type, IEnumerable<string>> serializeProperties, bool specificPropertiesOnly, JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, SerializeProperties = serializeProperties, SpecificPropertiesOnly = specificPropertiesOnly, JsonRequestBehavior = JsonRequestBehavior.AllowGet,CustomeSerializeSetting = settings};
        }
        /// <summary>
        /// �Զ������л�-���DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel����</param>
        /// <param name="contractResolver">�Զ������л����ƽӿ�</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, IContractResolver contractResolver, JsonSerializerSettings settings)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet, CustomeSerializeSetting = settings };
        }
        /// <summary>
        /// �Զ������л�-���DtGridModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">DtGridModel����</param>
        /// <param name="contractResolver">�Զ������л����ƽӿ�</param>
        /// <returns></returns>
        public static JsonResult Jsonp<T>(this Controller controller, DtGridModel<T> data, IContractResolver contractResolver)
        {
            return new FastJsonResult() { Data = data, ContractResolver = contractResolver, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        /// <summary>
        /// �Զ������л�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">Դ����</param>
        /// <param name="draw">��������</param>
        /// <param name="recordsTotal">������</param>
        /// <param name="recordsFiltered">��ʾ����</param>
        /// <param name="contractResolver">�Զ������л����ƽӿ�</param>
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
        /// �Զ������л�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">Դ����</param>
        /// <param name="draw">��������</param>
        /// <param name="recordsTotal">������</param>
        /// <param name="recordsFiltered">��ʾ����</param>
        /// <param name="settings">�Զ������л����ò���</param>
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
        /// �Զ������л�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">Դ����</param>
        /// <param name="draw">��������</param>
        /// <param name="recordsTotal">������</param>
        /// <param name="recordsFiltered">��ʾ����</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <param name="serializeProperties">�ֶ�����</param>
        /// <param name="specificPropertiesOnly">�Ƿ�����л����ֵ��ֶ�</param>
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
        /// �Զ������л�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="data">Դ����</param>
        /// <param name="draw">��������</param>
        /// <param name="recordsTotal">������</param>
        /// <param name="recordsFiltered">��ʾ����</param>
        /// <param name="settings">�Զ������л����ò���</param>
        /// <param name="contractResolver">�Զ������л����ƽӿ�</param>
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