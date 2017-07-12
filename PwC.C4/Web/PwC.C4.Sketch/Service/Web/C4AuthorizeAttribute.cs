using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PwC.C4.Sketch.Models.Enum;

namespace PwC.C4.Sketch.Service.Web
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
        public Roles[] VisitRole { get; set; }
        public List<string> RoleNames { get; set; }

        /// <summary>
        /// which role allow to visit
        /// </summary>
        /// <param name="role">RegistrationRole</param>
        public C4AuthorizeAttribute(params Roles[] role)
        {
            RoleNames = new List<string>();
            this.VisitRole = role;
            role.ForEach(c =>
            {
                RoleNames.Add(Models.Const.RoleNames.RoleEnDic[c]);
            });
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {

        }

    }
}