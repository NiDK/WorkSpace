using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PwC.C4.Infrastructure.Helper
{
    /// <summary>
    /// HttpContext处理类
    /// </summary>
    public class HttpContextHelper
    {
        private HttpContextHelper()
        {
        }

        /// <summary>
        /// 实例
        /// </summary>
        public static HttpContextHelper Instance { get; } = new HttpContextHelper();

        /// <summary>
        /// 获取当前HttpContext.Request中参数的值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">参数名称</param>
        /// <param name="objValue">如果值为空或不存在返回的默认值</param>
        /// <returns></returns>
        public T GetRequestParameterValue<T>(string key, T objValue)
        {
            return GetRequestParameterValue<T>(key, objValue, false);
        }

        /// <summary>
        /// 获取当前HttpContext.Request中参数的值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">参数名称</param>
        /// <param name="objValue">如果值为空或不存在返回的默认值</param>
        /// <param name="urlDecode">是否需要UrlDecode解码操作</param>
        /// <returns></returns>
        public T GetRequestParameterValue<T>(string key, T objValue, bool urlDecode)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request[key]))
            {
                return (T)Convert.ChangeType(urlDecode ?
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request[key]) :
                    HttpContext.Current.Request[key], typeof(T));
            }

            return objValue;
        }

        /// <summary>
        /// 将参数重新组合成Url
        /// </summary>
        /// <param name="uriString">url</param>
        /// <param name="requestParamsArray">参数集合的数组</param>
        /// <returns>补充了参数的url</returns>
        public string CombineUrlParams(string uriString, params NameValueCollection[] requestParamsArray)
        {
            return CombineUrlParams(uriString, Encoding.UTF8, requestParamsArray);
        }

        /// <summary>
        /// 将参数重新组合成Url
        /// </summary>
        /// <param name="uriString">url</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="requestParamsArray">参数集合的数组</param>
        /// <returns>补充了参数的url</returns>
        public string CombineUrlParams(string uriString, Encoding encoding, params NameValueCollection[] requestParamsArray)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                throw new ArgumentNullException(nameof(uriString));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            if (requestParamsArray == null)
            {
                throw new ArgumentNullException(nameof(requestParamsArray));
            }

            var requestParams = MergeParamsCollection(requestParamsArray);

            var strBuilder = new StringBuilder(1024);

            var startIndex = uriString.IndexOf('?');

            var leftPart = (startIndex >= 0) ? uriString.Substring(0, startIndex) : uriString;

            for (var i = 0; i < requestParams.Count; i++)
            {
                strBuilder.Append(i.Equals(0) ? "?" : "&");

                strBuilder.AppendFormat("{0}={1}",
                    HttpUtility.UrlEncode(requestParams.Keys[i], encoding),
                    HttpUtility.UrlEncode(requestParams[i], encoding));
            }

            return $"{leftPart}{strBuilder.ToString()}";
        }

        /// <summary>
        /// 得到URL锚点的信息。"#"后面的部分
        /// </summary>
        /// <param name="queryString">请求的字符串（http://localhost/home#littleTurtle）</param>
        /// <returns></returns>
        public string GetAnchorPointStringInUrl(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            int anchorPointStart = -1;

            for (int i = queryString.Length - 1; i >= 0; i--)
            {
                if (queryString[i].Equals('#'))
                {
                    anchorPointStart = i;
                }
                else
                {
                    if (queryString[i].Equals('&') || queryString[i].Equals('?'))
                    {
                        break;
                    }
                }
            }

            string result = string.Empty;

            if (anchorPointStart >= 0)
            {
                result = queryString.Substring(anchorPointStart);
            }

            return result;
        }

        #region 私有方法
        private NameValueCollection MergeParamsCollection(NameValueCollection[] requestParams)
        {
            var result = new NameValueCollection();

            for (int i = 0; i < requestParams.Length; i++)
                MergeTwoParamsCollection(result, requestParams[i]);

            return result;
        }

        private void MergeTwoParamsCollection(NameValueCollection target, NameValueCollection src)
        {
            foreach (string key in src.Keys)
            {
                if (target[key] == null)
                    target.Add(key, src[key]);
            }
        }
        #endregion
    }

    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Add, update, or remove parameters from a URL's query string.
        /// </summary>
        /// <param name="helper">UrlHelper instance</param>
        /// <param name="url">The URL to modify. If null, the current URL from the Request object is used.</param>
        /// <param name="updates">Query string parameters to add/overwrite.</param>
        /// <param name="removes">Query string parameters to remove entirely.</param>
        /// <param name="appends">Query string parameters to append additional values to (using delimiter)</param>
        /// <param name="subtracts">Query string parameters to subtract values from (using delimiter)</param>
        /// <param name="delimiter">Character to use to delimit multiple values for a query string parameter (defaults to `|`)</param>
        /// <returns>URL with modified query string</returns>
        public static string ModifyQueryString(this UrlHelper helper,
            string url,
            IDictionary<string, object> updates = null,
            IEnumerable<string> removes = null,
            IDictionary<string, object> appends = null,
            IDictionary<string, object> subtracts = null,
            char delimiter = '|')
        {
            var request = helper.RequestContext.HttpContext.Request;

            if (string.IsNullOrWhiteSpace(url))
            {
                url = request.RawUrl;
            }

            var urlParts = url.Split('?');
            url = urlParts[0];
            var query = urlParts.Length > 1
                ? HttpUtility.ParseQueryString(urlParts[1])
                : new NameValueCollection();

            if (updates != null)
            {
                updates.Keys.ToList().ForEach(key => query[key] = updates[key].ToString());
            }

            if (removes != null)
            {
                removes.ToList().ForEach(key => query.Remove(key));
            }

            if (appends != null)
            {
                foreach (var key in appends.Keys)
                {
                    var values = new List<string>();
                    if (query.AllKeys.Contains(key))
                    {
                        values.Add(query[key]);
                    }
                    if (typeof (IList).IsAssignableFrom(appends[key].GetType()))
                    {
                        foreach (var item in (appends[key] as IList))
                        {
                            values.Add(item.ToString());
                        }
                    }
                    else
                    {
                        values.Add(appends[key].ToString());
                    }
                    query[key] = string.Join(delimiter.ToString(), values);
                }
            }

            if (subtracts != null)
            {
                foreach (var key in subtracts.Keys)
                {
                    if (query.AllKeys.Contains(key))
                    {
                        var queryParts =
                            query[key].Split(new char[] {delimiter}, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (typeof (IList).IsAssignableFrom(subtracts[key].GetType()))
                        {
                            foreach (var item in (subtracts[key] as IList))
                            {
                                queryParts.Remove(item.ToString());
                            }
                        }
                        else
                        {
                            queryParts.Remove(subtracts[key].ToString());
                        }
                        query[key] = string.Join(delimiter.ToString(), queryParts);
                    }
                }
            }

            var queryString = string.Join("&",
                query.AllKeys.Where(key =>
                    !string.IsNullOrWhiteSpace(query[key])).Select(key =>
                        string.Join("&", query.GetValues(key).Select(val =>
                            string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(val))))));

            return query.HasKeys() ? url + "?" + queryString : url;
        }
    }
}