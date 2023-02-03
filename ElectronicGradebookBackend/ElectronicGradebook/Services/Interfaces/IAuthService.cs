using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LogInAsync(UserCredentialsDTO userCredentialsDTO);
    }
}
