
namespace FoodOrdering.Services
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void DeleteAccount(string accountId)
        {
            var user = context.Users.FirstOrDefault(u=>u.Id==accountId);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Invalid User");
            }
        }

        public IEnumerable<IdentityUser> GetAllUser()
        {
            var users=context.Users.ToList();
            return users;
        }
    }
}
