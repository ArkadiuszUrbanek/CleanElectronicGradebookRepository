using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        MessagePagedResponse SelectMessagePagedReponse(MessagesToSelectPaginationParameters messagesToSelectPaginationParameters);
        Task InsertMessageAsync(string text, int senderId, int receiverId);
    }
}
