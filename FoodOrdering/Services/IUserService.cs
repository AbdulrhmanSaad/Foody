
namespace FoodOrdering.Services
{
    public interface IUserService
    {
        public IEnumerable<IdentityUser> GetAllUser();
        public void DeleteAccount(string accountId);

    }
}
