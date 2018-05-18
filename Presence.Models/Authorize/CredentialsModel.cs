using System.ComponentModel.DataAnnotations;

namespace Presence.Models.Authorize
{
    public class CredentialsModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
