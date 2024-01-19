using FortuneWheel.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace FortuneWheel.Presentation.Controllers
{
    public class GoogleOAuthController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly GoogleOAuthService GoogleOAuthService;

        public GoogleOAuthController(IConfiguration configuration)
        {
            Configuration = configuration;
            GoogleOAuthService = new GoogleOAuthService(Configuration);
        }

        [HttpGet]
        public async Task<IActionResult> ContinueWithGoogle()
        {
            string scope = "openid profile email";
            string redirectUrl = Url.Action(nameof(HandleGoogleOAuthCode), "GoogleAuth", null, Request.Scheme);
            var codeVerifier = Guid.NewGuid().ToString("N");
            HttpContext.Session.SetString("сodeVerifier", codeVerifier);

            var codeChallenge = Services.CryptoService.ComputeSha256Hash(codeVerifier);

            var url = await GoogleOAuthService.GenerateOAuthRequestUrl(scope, redirectUrl, codeVerifier);

            return Redirect(url);
        }

        public async Task<IActionResult> HandleGoogleOAuthCode(string code)
        {
            string redirectUrl = Url.Action(nameof(HandleGoogleOAuthCode), "GoogleAuth", null, Request.Scheme);
            var сodeVerifier = HttpContext.Session.GetString("сodeVerifier");

            var url = await GoogleOAuthService.ExchangeCodeForToken(code, сodeVerifier, redirectUrl);

            return Redirect("https://localhost:7266");
        }
    }
}
