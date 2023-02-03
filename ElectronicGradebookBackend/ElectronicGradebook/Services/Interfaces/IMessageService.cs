using ElectronicGradebook.DTOs;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IMessageService
    {
        MessagePagedResponse GetMessagesBetweenUsers(MessagesToSelectPaginationParameters messagesToSelectPaginationParameters);
        Task InsertMessageAsync(MessageDetailsToInsertDTO messageDetailsToInsertDTO);
    }
}
