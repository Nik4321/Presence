using Presence.Data.Models;
using Presence.Models.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presence.Services
{
    public interface IUserService
    {
        IQueryable<User> AllUsers();

        Task<TokenResponse> AuthenticateUserAsync(CredentialsModel model);
    }
}
