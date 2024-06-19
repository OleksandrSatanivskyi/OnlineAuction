using OnlineAuc.Models;
using Microsoft.EntityFrameworkCore;
using OnlineAuc.Data.DbContexts;
using OnlineAuc.Exceptions;

namespace OnlineAuc.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbContext DbContext;
        public readonly string ImagesDirectory;

        public AccountService(IDbContext dbContext)
        {
            DbContext = dbContext;
            ImagesDirectory  = Environment.CurrentDirectory + "/wwwroot/images/";
        }

        public async Task<AccountModel> Get(Guid accountId)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
            {
                throw new NotFoundException("Account was not found");
            }

            return new AccountModel
            {
                Id = account.Id,
                Name = account.Name,
                Surname = account.Surname,
                Email = account.Email,
                AvatarName = account.AvatarName,
                Language = account.Language,
                IsDarkTheme = account.IsDarkTheme,
            };
        }

        public async Task Update(AccountModel model)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == model.Id);

            if (account == null)
            {
                throw new NotFoundException("Account was not found");
            }

            account.Name = model.Name;
            account.Surname = model.Surname;
            account.IsDarkTheme = model.IsDarkTheme;
            account.Language = model.Language;

            if(model.Photo != null)
            {
                if (!string.IsNullOrEmpty(account.AvatarName))
                {
                    string imagePath = Path.Combine(ImagesDirectory, account.AvatarName);

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                account.AvatarName = await SaveImage(model.Photo);
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                if (!Directory.Exists(ImagesDirectory))
                {
                    Directory.CreateDirectory(ImagesDirectory);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(ImagesDirectory, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return fileName;
            }

            throw new ArgumentException("Uncorrect image.");
        }
    }
}
