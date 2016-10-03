﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Core;
using Umbraco.Core.IO;

namespace Umbraco.Web.Install
{
    /// <summary>
    /// Ensures authorization occurs for the installer if it has already completed. If install has not yet occured
    /// then the authorization is successful
    /// </summary>
    internal class InstallAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UmbracoContext _umbracoContext;
        
        /// <summary>
        /// THIS SHOULD BE ONLY USED FOR UNIT TESTS
        /// </summary>
        /// <param name="umbracoContext"></param>
        public InstallAuthorizeAttribute(UmbracoContext umbracoContext)
        {
            if (umbracoContext == null) throw new ArgumentNullException("umbracoContext");
            _umbracoContext = umbracoContext;
            _applicationContext = _umbracoContext.Application;
        }

        public InstallAuthorizeAttribute()
        {
        }

        /// <summary>
        /// Ensures that the user must be logged in or that the application is not configured just yet.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            try
            {
                //if its not configured then we can continue
                if (!GetApplicationContext().IsConfigured)
                {
                    return true;
                }
                var umbCtx = GetUmbracoContext();
                //otherwise we need to ensure that a user is logged in
                var isLoggedIn = GetUmbracoContext().Security.ValidateCurrentUser();
                if (isLoggedIn)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Override to redirect instead of throwing an exception
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(SystemDirectories.Umbraco.EnsureEndsWith('/'));           
        }

    }
}