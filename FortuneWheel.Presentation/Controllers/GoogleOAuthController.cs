using FortuneWheel.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace FortuneWheel.Presentation.Controllers
{
    public class GoogleOAuthController : Controller
    {
        private readonly IConfiguration Configuration;
        private GoogleOAuthService GoogleOAuthService { get; set; }

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

            var codeChallenge = ComputeSha256Hash(codeVerifier);

            var url = await GoogleOAuthService.GenerateOAuthRequestUrl(scope, redirectUrl, codeVerifier);

            return Redirect(url);
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(rawData);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public async Task<IActionResult> HandleGoogleOAuthCode(string code)
        {
            string redirectUrl = "https://localhost:7266/Auth/HandleGoogleOAuthCode";
            var сodeVerifier = HttpContext.Session.GetString("сodeVerifier");

            var url = await GoogleOAuthService.ExchangeCodeForToken(code, сodeVerifier, redirectUrl);

            return Redirect("https://localhost:7266");
        }
    }
}
