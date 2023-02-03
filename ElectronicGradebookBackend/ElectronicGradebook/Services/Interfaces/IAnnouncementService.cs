using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IAnnouncementService
    {
        AnnouncementPagedResponse SelectAnnouncements(BasePaginationParameters<EAnnouncementSortableProperties> basePaginationParameters, EUserRole userRole, int userId);
        Task DeleteAnnouncementByIdAsync(int id);
        Task UpdateAnnouncementAsync(AnnouncementDetailsToUpdateDTO announcementDetailsToUpdateDTO);
        Task InsertAnnouncementAsync(AnnouncementDetailsToInsertDTO announcementDetailsToInsertDTO);
    }
}
