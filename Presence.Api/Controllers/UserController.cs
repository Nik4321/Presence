using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presence.Data.Models;
using Presence.Api.Models.User;

namespace Presence.Api.Controllers
{
    [Route("api/User")]
    public class UserController : BaseController
    {
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return this.Ok();
            }

            this.AddErrors(result);
            return this.BadRequest(this.ModelState);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Code == "DuplicateUserName")
                {
                    continue;
                }

                if (error.Code.Contains("Email"))
                {
                    this.ModelState.AddModelError("Email", error.Description);
                }
                else if (error.Code.Contains("Password"))
                {
                    this.ModelState.AddModelError("Password", error.Description);
                }
                else
                {
                    this.ModelState.AddModelError(error.Code, error.Description);
                }
            }
        }
    }
}