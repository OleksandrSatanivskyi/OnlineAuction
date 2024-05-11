using FortuneWheel.Models;

namespace FortuneWheel.Services
{
    public interface IAccountService
    {
        Task<AccountModel> Get(Guid accountId);
        Task Update(AccountModel model);
    }
}
