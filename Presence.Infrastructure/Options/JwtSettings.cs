namespace Presence.Infrastructure.Options
{
    public class JwtSettings
    {
        public string Authority { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }
    }
}
