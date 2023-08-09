using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using BCMCH.OTM.External;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.API.Security
{
    public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IOTMDataClient _oTMDataClient;

        public BearerAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOTMDataClient oTMDataClient)
            : base(options, logger, encoder, clock)
        {
            _oTMDataClient = oTMDataClient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            Authentication auth = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var _token = authHeader.Parameter;
                if (authHeader != null)
                {
                    // auth = await _oTMDataClient.AuthenticateUser(_token);
                    // above calls the auth api and do the function 
                    // uncomment above to call auth api 


                    // comment below when we apply the auth api START
                    auth = new Authentication();
                    auth.Authenticated=true;
                    auth.Id=0;
                    auth.UserName="";
                    auth.key="";
                    // comment below when we apply the auth api END
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (auth == null)
                return AuthenticateResult.Fail("Invalid Token");

            if(!auth.Authenticated)
                return AuthenticateResult.Fail("Invalid Token");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "test"),
                new Claim(ClaimTypes.Name, "testAuth"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
