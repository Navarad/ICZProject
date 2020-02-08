using ICZProject.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace ICZProject.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                AuthenticationMode = AuthenticationMode.Active,
                LoginPath = new PathString("/Home/Index"),
                ReturnUrlParameter = "continueUrl",
                ExpireTimeSpan = TimeSpan.FromDays(365),
                SlidingExpiration = true,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnApplyRedirect = context =>
                    {
                    }
                }
            });
        }
    }
}