using FortuneWheel.Data.DbContexts;
using FortuneWheel.Results.Auth;
using Microsoft.EntityFrameworkCore;
using FortuneWheel.Exceptions;
using FortuneWheel.Services;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using FortuneWheel.Models.Auth;
using FortuneWheel.Presentation.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using FortuneWheel.Domain.Auth;

namespace FortuneWheel.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IDbContext DbContext;
        private EmailService EmailService { get; set; }

        public AuthService(IDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            EmailService = new EmailService(configuration);
        }

        public async Task<IEnumerable<Claim>> Login(LoginModel model)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (account == null) throw new NotFoundException($"Account with email {model.Email} is not found.");

            var cipheredPassword = CryptoService.ComputeSha256Hash(model.Password);
            if (account.Password != cipheredPassword) throw new UnauthorizedAccessException("Invalid password.");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email)
            };

            return claims;
        }

        public async Task SignUp(SignUpModel model)
        {
            if (model.Password != model.ConfirmPassword) throw new ValidationException("The password and confirm password do not match.");

            var existingAccountWithSameEmail = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (existingAccountWithSameEmail != null) throw new DuplicateEmailException($"Email '{model.Email}' is already associated with another account.");

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Surname = model.Surname,
                Password = CryptoService.ComputeSha256Hash(model.Password)
            };

            var existingEmails = DbContext.UnconfirmedEmails.Where(e => e.Email == model.Email).ToList();
            if (existingEmails.Count > 0) DbContext.UnconfirmedEmails.RemoveRange(existingEmails);

            var unconfirmedEmail = new UnconfirmedEmail
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                AccountId = account.Id,
                Code = GenerateCode()
            };

            var messageBody = EmailContentBuilder.EmailConfirmationMessageBody(account.Name, account.Surname, unconfirmedEmail.Code);
            await EmailService.SendEmail(messageBody, unconfirmedEmail.Email, "Email confirmation");

            await DbContext.Accounts.AddAsync(account);
            await DbContext.UnconfirmedEmails.AddAsync(unconfirmedEmail);
            await DbContext.SaveChangesAsync();
        }

        private Random random = new Random();
        private string GenerateCode()
        {
            int codeValue = random.Next(10000, 99999);
            return codeValue.ToString();
        }

        public async Task ConfirmEmail(ConfirmEmailModel model)
        {
            var email = await DbContext.UnconfirmedEmails.FirstOrDefaultAsync(e => e.Email == model.Email);
            if (email == null) throw new NotFoundException($"There are no account registered with email {model.Email}.");
            if (email.Code != model.Code) throw new UnauthorizedAccessException("Invalid code.");

            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == email.AccountId);
            if (account == null) throw new NotFoundException($"There are no account registered with email {model.Email}.");
            if (!account.Email.IsNullOrEmpty()) throw new InvalidOperationException($"Account with id {account.Id} already has a verified email.");

            account.Email = model.Email;
            DbContext.UnconfirmedEmails.Remove(email);

            await DbContext.SaveChangesAsync();
        }
    }

}
