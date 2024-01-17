using FortuneWheel.Data.DbContexts;
using FortuneWheel.Presentation.Models.Auth;
using FortuneWheel.Results.Auth;
using Microsoft.EntityFrameworkCore;
using FortuneWheel.Exceptions;
using FortuneWheel.Services;
using FortuneWheel.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Diagnostics;
using System;

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
                Password = model.Password
            };

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
        }

        private Random random = new Random();
        private string GenerateCode()
        {
            int codeValue = random.Next(10000, 99999);
            return codeValue.ToString();
        }
    }

}
