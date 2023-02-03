using ElectronicGradebook.DTOs.Enums;

namespace ElectronicGradebook.DTOs
{
    public class UsersToSelectPaginationParameters : BasePaginationParameters<EUserSortableProperties>
    {
        public string? SearchPhrase { get; set; }
    }
}
