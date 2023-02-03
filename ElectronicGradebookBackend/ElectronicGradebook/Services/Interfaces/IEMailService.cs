namespace ElectronicGradebook.Services.Interfaces
{
    public interface IEMailService
    {
        Task sendEMailAsync(string subject, string to, string body);
    }
}
