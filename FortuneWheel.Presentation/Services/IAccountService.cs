using OnlineAuc.Models;

namespace OnlineAuc.Services
{
    public interface IAccountService
    {
        Task<AccountModel> Get(Guid accountId);
        Task Update(AccountModel model);
    }
}
