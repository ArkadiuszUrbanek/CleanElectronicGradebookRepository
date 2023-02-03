using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;

namespace ElectronicGradebook.DTOs
{
    public class MessagePagedResponse : BasePagedResponse<MessageDetailsToSelectDTO, Message, EMessageSortableProperties> 
        
    {
        public MessagePagedResponse(IQueryable<Message> source, int pageNumber, int pageSize, EMessageSortableProperties orderBy, EOrder order) : base(source, pageNumber, pageSize, orderBy, order)
        {
        }

        protected override IQueryable<Message> PerformSortingLogic(IQueryable<Message> source, EMessageSortableProperties orderBy, EOrder order)
        {
            switch (order) {
                case EOrder.Ascending:
                    { 
                        switch (orderBy) {
                            case EMessageSortableProperties.Id: return source.OrderBy(m => m.MessageId);
                            case EMessageSortableProperties.Timestamp: return source.OrderBy(m => m.Timestamp.Date).ThenBy(m => m.Timestamp.TimeOfDay);
                            default: return source;

                        }
                    }
                case EOrder.Descending:
                    {
                        switch (orderBy)
                        {
                            case EMessageSortableProperties.Id: return source.OrderByDescending(m => m.MessageId);
                            case EMessageSortableProperties.Timestamp: return source.OrderByDescending(m => m.Timestamp.Date).ThenByDescending(m => m.Timestamp.TimeOfDay);
                            default: return source;
                        }
                    }
                default: return source;
            }
        }

        protected override IQueryable<MessageDetailsToSelectDTO> PerformMapping(IQueryable<Message> source, int? userId)
        {
            return source.Select(m => new MessageDetailsToSelectDTO()
                {
                    Id = m.MessageId,
                    Text = m.Text,
                    Timestamp = m.Timestamp,
                    SenderId = m.UserSenderId,
                    ReceiverId = m.UserReceiverId
                }
            );
        }
    }
}
