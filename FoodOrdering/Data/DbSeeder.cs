using Microsoft.AspNetCore.Identity;
using FoodOrdering.Constants;

namespace FoodOrdering.Data
{
    public class DbSeeder
    {
        public static async Task SeederDefultData(IServiceProvider serviceProvider)
        {
            var userManger = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager=serviceProvider.GetService<RoleManager<IdentityRole>>();
            //await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            //await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //add admin account 
            var admin = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
                
            };
            var IsInDb=await userManger.FindByEmailAsync(admin.Email);
            if (IsInDb is null)
            {
               var res= await userManger.CreateAsync(admin, "Admin@123");
                if (res.Succeeded)
                {
                    await userManger.AddToRoleAsync(admin, Roles.Admin.ToString());
                }
            }
           
        }
    }
}
