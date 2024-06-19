using OnlineAuc.Models.Auth;
using OnlineAuc.Presentation.Models.Auth;
using Google.Apis.Auth;
using System.Security.Claims;

namespace OnlineAuc.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<IEnumerable<Claim>> Login(LoginModel model);
        Task SignUp(SignUpModel model);
        Task ConfirmEmail(ConfirmEmailModel model);
        Task<IEnumerable<Claim>> ContinueWithGoogle(GoogleJsonWebSignature.Payload payload);
    }
}
