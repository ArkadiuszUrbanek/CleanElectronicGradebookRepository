using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;

namespace ElectronicGradebook.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public MessageRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public MessagePagedResponse SelectMessagePagedReponse(MessagesToSelectPaginationParameters messagesToSelectPaginationParameters)
        {
            return new MessagePagedResponse(
                _dbContext.Messages
                .Where(m => (m.UserSenderId == messagesToSelectPaginationParameters.CoversationParticipantsIds[0] && m.UserReceiverId == messagesToSelectPaginationParameters.CoversationParticipantsIds[1]) ||
                      (m.UserSenderId == messagesToSelectPaginationParameters.CoversationParticipantsIds[1] && m.UserReceiverId == messagesToSelectPaginationParameters.CoversationParticipantsIds[0])),
                messagesToSelectPaginationParameters.PageNumber,
                messagesToSelectPaginationParameters.PageSize,
                messagesToSelectPaginationParameters.SortBy,
                messagesToSelectPaginationParameters.Order
           );
        }

        public async Task InsertMessageAsync(string text, int senderId, int receiverId)
        {
            await _dbContext.Messages.AddAsync(new Message()
            {
                Text = text,
                Timestamp = DateTime.UtcNow,
                UserSenderId = senderId,
                UserReceiverId = receiverId
            }
           );

           await _dbContext.SaveChangesAsync();
        }
    }
}
