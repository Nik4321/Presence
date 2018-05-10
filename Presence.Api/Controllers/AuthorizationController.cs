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
using Presence.Api.Models.Authorize;

namespace Presence.Api.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public AuthorizationController(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("api/User/Token")]
        public async Task<IActionResult> Token(Credentials model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);

            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !isPasswordCorrect)
            {
                return this.Unauthorized();
            }

            var token = this.GenerateToken(user).GetAwaiter().GetResult();
            
            var result = new
            {
                userEmail = user.Email,
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };

            return this.Json(result);
        }

        private async Task<JwtSecurityToken> GenerateToken(User user)
        {
            var roles = await this.userManager.GetRolesAsync(user);

            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token");
            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("JwtSettings")["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                this.configuration.GetSection("JwtSettings")["Authority"],
                this.configuration.GetSection("JwtSettings")["Audience"],
                claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return token;
        }
    }
}