using System;
using System.Web.Mvc;

namespace PwC.C4.Infrastructure.WebExtension
{
    /// <summary>
    /// 错误分发页面，可以自定义类型，其中视图的ViewData中，Model是异常类实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ErrorDispatchAttribute : FilterAttribute, System.Web.Mvc.IExceptionFilter
    {
        /// <summary>
        /// 错误状态代码
        /// </summary>
        public const int ErrorStatusCode = 312;
        /// <summary>
        /// Ajax请求的头
        /// </summary>
        public const string AjaxHeader = "x-PwC-ajax";
        /// <summary>
        /// Ajax异常消息头
        /// </summary>
        public const string AjaxError = "x-PwC-ajax-error";
        /// <summary>
        /// 异常分发
        /// </summary>
        public ErrorDispatchAttribute()
        {
        }
        public ErrorDispatchAttribute(Type type)
            :this()
        {
            ExceptionType = type;
        }

        public string View { get; set; }
        public Type ExceptionType { get; set; }
        #region IExceptionFilter 成员

        public void OnException(ExceptionContext filterContext)
        {
            if (this.ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                filterContext.ExceptionHandled = true;
                string ajaxHeader = filterContext.RequestContext.HttpContext.Request.Headers[AjaxHeader];
                if (!string.IsNullOrEmpty(ajaxHeader))
                {
                    filterContext.HttpContext.Response.AppendHeader(AjaxError, System.Web.HttpUtility.UrlEncode(filterContext.Exception.Message));
                }
                if (!string.IsNullOrEmpty(View))
                {
                    filterContext.HttpContext.Response.StatusCode = ErrorStatusCode;
                    ViewResult view = new ViewResult() { ViewName = this.View };
                    view.ViewData.Model = filterContext.Exception;
                    filterContext.Result = view;
                }
                else
                {
                    filterContext.Result = new ContentResult()
                    {
                         Content=filterContext.Exception.Message,
                         ContentType="text/html"
                    };
                }
            }
            else
            {
                //Log
            }
        }

        #endregion
    }
}
