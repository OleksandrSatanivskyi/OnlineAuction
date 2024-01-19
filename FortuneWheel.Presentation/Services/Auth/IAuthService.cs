using FortuneWheel.Models.Auth;
using FortuneWheel.Presentation.Models.Auth;
using FortuneWheel.Results.Auth;
using System.Security.Claims;

namespace FortuneWheel.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<IEnumerable<Claim>> Login(LoginModel model);
        Task SignUp(SignUpModel model);
        Task ConfirmEmail(ConfirmEmailModel model);
    }
}
