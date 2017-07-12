using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PwC.C4.Membership.WebExtension
{
    /// <summary>
    /// Define which role can visit this page
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class C4SecurityAttribute : FilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {

        }

    }
}