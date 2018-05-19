using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Presence.Data.Models;
using Presence.Infrastructure.Exceptions;
using Presence.Models.Authorize;
using Presence.Services;

namespace Presence.Api.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IUserService userService;

        public AuthorizationController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("api/User/Token")]
        public async Task<IActionResult> Token([FromBody]CredentialsModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var result = await this.userService.AuthenticateUserAsync(model);

                    return this.Json(result);
                }
                catch (UserAuthenticationException)
                {
                    return this.Unauthorized();
                }
            }

            return this.BadRequest(this.ModelState);
        }
    }
}