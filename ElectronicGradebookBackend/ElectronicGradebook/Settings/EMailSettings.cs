namespace ElectronicGradebook.Settings
{
    public class EMailSettings
    {
        public string SenderAddress { get; set; } = null!;
        public string SenderDisplayName { get; set; } = null!;
        public string SenderPassword { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool EnableTLS { get; set; }
    }
}
