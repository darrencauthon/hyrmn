using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace hyrmn.Infrastructure.Filters
{
    public class OpenIdAuthenticationCallbackAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            var openid = new OpenIdRelyingParty();
            var response = openid.GetResponse();

            // We do have an openId response, it's the provider calling back
            if (response != null)
            {
                // look for IAuthenticationResponse parameter and pass the response object to the action as we can't call GetResponse() twice later in the action method
                var parameterName = methodInfo.GetParameters().Where(pi => pi.ParameterType.Equals(typeof(IAuthenticationResponse))).Select(pi => pi.Name).SingleOrDefault();
                if (!String.IsNullOrEmpty(parameterName))
                {
                    controllerContext.RouteData.Values.Add(parameterName, response);
                }
                return true;
            }
            return false;
        }
    }

}