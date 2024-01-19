using FortuneWheel.Models.Auth;
using FortuneWheel.Presentation.Models.Auth;
using Google.Apis.Auth;
using System.Security.Claims;

namespace FortuneWheel.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<IEnumerable<Claim>> Login(LoginModel model);
        Task SignUp(SignUpModel model);
        Task ConfirmEmail(ConfirmEmailModel model);
        Task<IEnumerable<Claim>> ContinueWithGoogle(GoogleJsonWebSignature.Payload payload);
    }
}
