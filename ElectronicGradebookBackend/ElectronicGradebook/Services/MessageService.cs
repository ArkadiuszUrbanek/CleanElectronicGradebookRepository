using ElectronicGradebook.Services.Interfaces;
using ElectronicGradebook.DTOs;
using ElectronicGradebook.Repositories.Interfaces;

namespace ElectronicGradebook.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public MessagePagedResponse GetMessagesBetweenUsers(MessagesToSelectPaginationParameters messagesToSelectPaginationParameters)
        {
            return _messageRepository.SelectMessagePagedReponse(messagesToSelectPaginationParameters);
        }

        public async Task InsertMessageAsync(MessageDetailsToInsertDTO messageDetailsToInsertDTO)
        {
            await _messageRepository.InsertMessageAsync(messageDetailsToInsertDTO.Text,
                                                        messageDetailsToInsertDTO.SenderId,
                                                        messageDetailsToInsertDTO.ReceiverId);
        }
    }
}
