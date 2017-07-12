using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PwC.C4.Membership.WebExtension
{
    /// <summary>
    /// Define which role can visit this page
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class C4AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] VisitRole { get; set; }
        /// <summary>
        /// which role allow to visit
        /// </summary>
        /// <param name="role">RegistrationRole</param>
        public C4AuthorizeAttribute(params string[] role)
        {
            this.VisitRole = role;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {

        }

    }
}