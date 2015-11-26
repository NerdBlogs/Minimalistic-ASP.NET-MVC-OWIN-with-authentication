using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.Slack;

namespace UsingSlackLoginSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cookie authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Enable external sign in cookie
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Add Slack authentication support
            var options = new SlackAuthenticationOptions
            {
                ClientId = "[https://api.slack.com/applications - ClientId]",
                ClientSecret = "[https://api.slack.com/applications - Client Secret]",
                //TeamId = "" // optional
            };
            options.Scope.Add("identify");
            app.UseSlackAuthentication(options);
        }
    }
}
