using justAsk.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace justAsk.Controllers
{
    public class UserHelper : UserHelperProvider
    {
        private readonly UserManager<ApplicationUser> userManager;
        public UserHelper(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ApplicationUser> GetApplicationUser(ClaimsPrincipal user)
        {
            return await this.userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }

    public interface UserHelperProvider
    {
        Task<ApplicationUser> GetApplicationUser(ClaimsPrincipal user);
    }
}