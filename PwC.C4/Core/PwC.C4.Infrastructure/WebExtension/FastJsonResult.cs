using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PwC.C4.Infrastructure.WebExtension
{
    ///<summary>
    /// 基于Json.net的快速序列化，且支持Jsonp
    ///</summary>
    public class FastJsonResult:JsonResult
    {
        /// <summary>
        /// 
        /// </summary>
        public FastJsonResult():base()
        {
            CallbackFunction = "callback";
            SpecificPropertiesOnly = false;
            IsRestApiFormat = false;
        }

        private readonly Type _jqueryGridModelType = typeof (DtGridModel<>);
        private readonly Type _restApiModelType = typeof (RestApiResultModel);
        ///<summary>
        /// 
        /// Jsonp回调函数名
        ///</summary>
        public string CallbackFunction { get; set; }
        ///<summary>
        /// 是否需要序列化null值
        ///</summary>
        public bool NullValueHandling { get; set; }
        /// <summary>
        /// 序列化字段
        /// </summary>
        public Dictionary<Type, IEnumerable<string>> SerializeProperties { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IContractResolver ContractResolver { get; set; }
        /// <summary>
        /// 自定义序列化属性设置
        /// </summary>
        public JsonSerializerSettings CustomeSerializeSetting { get; set; }
        /// <summary>
        /// 是否保留格式（空格or换行）
        /// </summary>
        public bool KeepIndent { get; set; }
        /// <summary>
        /// 是否ISO模式日期格式
        /// </summary>
        public bool IsoDateFormat { get; set; }
        /// <summary>
        /// 序列化or不序列化所提供SerializeProperties字段
        /// </summary>
        public bool SpecificPropertiesOnly{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRestApiFormat { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            var ajaxVersion = context.RequestContext.HttpContext.Request.Headers[ErrorDispatchAttribute.AjaxHeader];
            if (!string.IsNullOrEmpty(ajaxVersion))
            {
                IsRestApiFormat = true;
            }
         
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("donn't allow http method GET,Use JsonBehavior");
            }

            var response = context.HttpContext.Response;

            if (!context.IsChildAction)
            {
                response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data == null) return;
            var funcName = context.HttpContext.Request.QueryString[CallbackFunction];
            var enableJsonp = !string.IsNullOrEmpty(funcName);
            if(enableJsonp)
            {
                response.Write(funcName);
                response.Write('(');
            }

            var serializeSetting = CustomeSerializeSetting ?? new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling
                    ? Newtonsoft.Json.NullValueHandling.Include
                    : Newtonsoft.Json.NullValueHandling.Ignore,
                Formatting = KeepIndent ? Formatting.Indented : Formatting.None,
                DateFormatHandling = IsoDateFormat
                    ? DateFormatHandling.IsoDateFormat
                    : DateFormatHandling.MicrosoftDateFormat
            };
            if (SerializeProperties != null && SerializeProperties.Count > 0)
            {
                serializeSetting.ContractResolver = new DynamicContractResolver(SerializeProperties,
                    SpecificPropertiesOnly);
            }
            if (ContractResolver != null)
            {
                serializeSetting.ContractResolver = ContractResolver;
            }


            var dataType = Data.GetType();


            IsRestApiFormat = IsRestApiFormat && 
                              dataType != _restApiModelType && 
                              !(dataType.IsGenericType && dataType.GetGenericTypeDefinition() == _jqueryGridModelType);

            if (IsRestApiFormat)
            {
                response.Write("{\"code\":200,\"data\":");
            }

            var jsonData = JsonConvert.SerializeObject(Data, serializeSetting);

            response.Write(jsonData);

            if (IsRestApiFormat)
            {
                response.Write("}");
            }

            if (enableJsonp)
                response.Write(')');
        }
    }
}