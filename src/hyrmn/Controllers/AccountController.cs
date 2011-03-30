using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using hyrmn.Infrastructure.Filters;

namespace hyrmn.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _adminEmail = ConfigurationManager.AppSettings["AdminEmail"];

        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult OpenIdLogin(string openidIdentifier)
        {
            if (!Identifier.IsValid(openidIdentifier))
            {
                ModelState.AddModelError("loginIdentifier",
                                         "The specified login identifier is invalid");

                return View("LogOn");
            }
            else
            {
                var openid = new OpenIdRelyingParty();
                IAuthenticationRequest request = openid.CreateRequest(
                    Identifier.Parse(openidIdentifier));

                // Require some additional data
                request.AddExtension(new ClaimsRequest
                {
                    Email = DemandLevel.Require,
                    FullName = DemandLevel.Request
                });

                return request.RedirectingResponse.AsActionResult();
            }
        }

        // handles the provider callback
        [OpenIdAuthenticationCallback]
        public ActionResult OpenIdLogin(IAuthenticationResponse response)
        {
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    var claimsResponse = response.GetExtension<ClaimsResponse>();
                    if (claimsResponse.Email == _adminEmail)
                    {
                        FormsAuthentication.SetAuthCookie(response.ClaimedIdentifier, false);
                        return RedirectToAction("Index", "Dashboard", new {area = "Admin"});
                    }
                    break;
                case AuthenticationStatus.Canceled:
                    ModelState.AddModelError("loginIdentifier",
                        "Login was cancelled at the provider");
                    break;
                case AuthenticationStatus.Failed:
                    ModelState.AddModelError("loginIdentifier",
                        "Login failed using the provided OpenID identifier");
                    break;
            }

            return new EmptyResult();
        }    
    }
}
