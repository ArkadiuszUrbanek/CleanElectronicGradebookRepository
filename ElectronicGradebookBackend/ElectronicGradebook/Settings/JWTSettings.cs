namespace ElectronicGradebook.Settings
{
    public class JWTSettings
    {
        public string SecurityKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public double ExpirationTimeInMinutes { get; set; }
    }
}
