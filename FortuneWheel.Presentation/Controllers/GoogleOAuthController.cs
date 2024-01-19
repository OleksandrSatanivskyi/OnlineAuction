using FortuneWheel.Application.Services.Auth;
using FortuneWheel.Services.Auth;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace FortuneWheel.Presentation.Controllers
{
    public class GoogleOAuthController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly GoogleOAuthService GoogleOAuthService;
        private readonly IAuthService AuthService;
        public GoogleOAuthController(IConfiguration configuration, IAuthService authService)
        {
            Configuration = configuration;
            GoogleOAuthService = new GoogleOAuthService(Configuration);
            AuthService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> ContinueWithGoogle()
        {
            string scope = "openid profile email";
            string redirectUrl = Url.Action(nameof(HandleGoogleOAuthCode), "GoogleOAuth", null, Request.Scheme);
            var codeVerifier = Guid.NewGuid().ToString("N");
            HttpContext.Session.SetString("сodeVerifier", codeVerifier);

            var codeChallenge = Services.CryptoService.ComputeSha256Hash(codeVerifier);

            var url = await GoogleOAuthService.GenerateOAuthRequestUrl(scope, redirectUrl, codeVerifier);

            return Redirect(url);
        }

        public async Task<IActionResult> HandleGoogleOAuthCode(string code)
        {
            string redirectUrl = Url.Action(nameof(HandleGoogleOAuthCode), "GoogleOAuth", null, Request.Scheme);
            var сodeVerifier = HttpContext.Session.GetString("сodeVerifier");

            var tokenResult = await GoogleOAuthService.ExchangeCodeForToken(code, сodeVerifier, redirectUrl);
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(tokenResult.IdToken);

            var claims = await AuthService.ContinueWithGoogle(payload);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return RedirectToAction("LoginRedirect", "Auth");
        }
    }
}
