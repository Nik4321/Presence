using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presence.Data;
using Presence.Data.Models;
using Presence.Infrastructure.Exceptions;
using Presence.Models.Authorize;
using Presence.Models.User;
using Presence.Infrastructure.Options;

namespace Presence.Services
{
    public class UserService : IUserService
    {
        private readonly PresenceDbContext db;
        private readonly UserManager<User> userManager;
        private readonly JwtSettings jwtSettings;

        public UserService(
            PresenceDbContext db,
            UserManager<User> userManager,
            IOptions<JwtSettings> jwtSettings)
        {
            this.db = db;
            this.userManager = userManager;
            this.jwtSettings = jwtSettings.Value;
        }

        public IQueryable<User> AllUsers()
        {
            return this.db.Users;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<TokenResponse> AuthenticateUserAsync(CredentialsModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);

            var isPasswordCorrect = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !isPasswordCorrect)
            {
                throw new UserAuthenticationException("Unauthorized");
            }

            var token = await this.GenerateUserToken(user);

            var result = new TokenResponse
            {
                UserEmail = user.Email,
                Type = "bearer",
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return result;
        }

        private async Task<JwtSecurityToken> GenerateUserToken(User user)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                this.jwtSettings.Authority,
                this.jwtSettings.Audience,
                claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return token;
        }
    }
}
