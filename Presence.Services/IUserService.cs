using Microsoft.AspNetCore.Identity;
using Presence.Data.Models;
using Presence.Models.Authorize;
using Presence.Models.User;
using System.Linq;
using System.Threading.Tasks;

namespace Presence.Services
{
    public interface IUserService
    {
        IQueryable<User> AllUsers();

        Task<IdentityResult> RegisterUserAsync(RegisterModel model);

        Task<TokenResponse> AuthenticateUserAsync(CredentialsModel model);
    }
}
