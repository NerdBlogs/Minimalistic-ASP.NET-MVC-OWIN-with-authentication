using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace UsingSlackLoginSite.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            return new ChallengeResult("Slack", Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = HttpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo.ExternalIdentity.IsAuthenticated)
            {
                SignIn();
                return Redirect(returnUrl);
            }

            return new HttpUnauthorizedResult();
        }

        private void SignIn()
        {
            var loginInfo = HttpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo.ExternalIdentity.IsAuthenticated)
            {
                var manager = HttpContext.GetOwinContext().Authentication;
                var claims = loginInfo.ExternalIdentity.Claims;
                manager.SignIn(new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(60)
                },
                new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            private string LoginProvider { get; }
            private string RedirectUri { get; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}