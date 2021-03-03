using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace eCert
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Index"),
                SlidingExpiration = true
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "595802833781-nm52ahup2i8ksp5nogqdr2av5tgi3br2.apps.googleusercontent.com",
                ClientSecret = "IFmTPHO1AkToP0MXqWIKaUK6",
                CallbackPath = new PathString("/signin-google")
            });
        }
    }
}
