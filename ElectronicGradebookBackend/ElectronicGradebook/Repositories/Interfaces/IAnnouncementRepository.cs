using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using System.Linq.Expressions;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<List<Announcement>> SelectAnnouncementsWithTheirAuthorsAsync();
        AnnouncementPagedResponse SelectAnnouncementPagedReponse(BasePaginationParameters<EAnnouncementSortableProperties> basePaginationParameters, Expression<Func<Announcement, bool>> predicate);
        Task DeleteAnnouncementByIdAsync(int id);
        Task UpdateAnnouncementAsync(int id, string? title, string? contents, HashSet<EUserRole>? authorizedRoles);
        Task InsertAnnouncementAsync(Announcement announcement);
    }
}
