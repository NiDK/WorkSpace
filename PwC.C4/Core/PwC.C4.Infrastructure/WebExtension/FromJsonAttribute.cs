using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PwC.C4.Infrastructure.WebExtension
{
    /// <summary>
    /// FromJsonAttribute 页面传递Json对象MVC序列化失败修正
    /// </summary>
    public class FromJsonAttribute : CustomModelBinderAttribute
    {
        /// <summary>
        /// JavaScriptSerializer
        /// </summary>
        private readonly static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        /// <summary>
        /// GetBinder
        /// </summary>
        /// <returns>Binder</returns>
        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }

        /// <summary>
        /// JsonModelBinder
        /// </summary>
        private class JsonModelBinder : IModelBinder
        {
            /// <summary>
            /// BindModel
            /// </summary>
            /// <returns>object</returns>
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var stringified = controllerContext.HttpContext.Request[bindingContext.ModelName];
                return string.IsNullOrEmpty(stringified) ? null : Serializer.Deserialize(stringified, bindingContext.ModelType);
            }
        }
    }
}
