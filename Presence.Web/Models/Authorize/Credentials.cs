using System.ComponentModel.DataAnnotations;

namespace Presence.Web.Models.Authorize
{
    public class Credentials
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
