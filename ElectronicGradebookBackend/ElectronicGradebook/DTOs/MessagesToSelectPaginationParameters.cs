using ElectronicGradebook.DTOs.Enums;

namespace ElectronicGradebook.DTOs
{
    public class MessagesToSelectPaginationParameters : BasePaginationParameters<EMessageSortableProperties>
    {
        public int[] CoversationParticipantsIds { get; set; } = new int[2];
    }
}
