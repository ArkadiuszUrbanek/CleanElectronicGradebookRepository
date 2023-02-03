namespace ElectronicGradebook.DTOs
{
    public class UserCredentialsDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Device { get; set; } = null!;
        public string OS { get; set; } = null!;
        public string WebBrowser { get; set; } = null!;
    }
}
