using FortuneWheel.Results.Auth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace FortuneWheel.Services.Auth
{
    public class GoogleOAuthService
    {
        private readonly IConfiguration Configuration;
        private readonly string ClientId;
        private readonly string ClientSecret;
        private const string OAuthServerEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string TokenEndpoint = "https://oauth2.googleapis.com/token";

        public GoogleOAuthService(IConfiguration configuration)
        {
            Configuration = configuration;
            ClientId = configuration["GoogleOAuth:ClientId"];
            ClientSecret = configuration["GoogleOAuth:ClientSecret"];
        }

        public async Task<string> ExchangeCodeForToken(string code, string сodeVerifier, string redirectUrl)
        {
            var authParams = new Dictionary<string, string>()
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "response_type", "code" },
                { "code", code },
                { "code_verifier", сodeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(TokenEndpoint, new FormUrlEncodedContent(authParams));
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResult = JsonConvert.DeserializeObject<TokenResult>(responseContent);
                if (!response.IsSuccessStatusCode) throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}. Content: {responseContent}");

                return tokenResult.AccessToken;
            }
        }

        public async Task<string> GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeChallenge)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "client_id", ClientId },
                { "redirect_uri", redirectUrl },
                { "response_type", "code" },
                { "scope", scope },
                { "code_challenge", codeChallenge },
                { "code_challenge_method", "plain" },
                { "access_type", "offline" }
            };

            var url = QueryHelpers.AddQueryString(OAuthServerEndpoint, queryParams);

            return url;
        }
    }


}
