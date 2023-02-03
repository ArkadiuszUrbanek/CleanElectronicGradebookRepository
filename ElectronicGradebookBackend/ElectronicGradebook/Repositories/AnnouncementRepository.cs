using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElectronicGradebook.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public AnnouncementRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Announcement>> SelectAnnouncementsWithTheirAuthorsAsync()
        {
            return await _dbContext.Announcements
                .Include(a => a.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public AnnouncementPagedResponse SelectAnnouncementPagedReponse(BasePaginationParameters<EAnnouncementSortableProperties> basePaginationParameters, Expression<Func<Announcement, bool>> predicate)
        {
            return new AnnouncementPagedResponse(
                _dbContext.Announcements.Where(predicate).Include(a => a.User).Include(a => a.AnnouncementRoles),
                basePaginationParameters.PageNumber,
                basePaginationParameters.PageSize,
                basePaginationParameters.SortBy,
                basePaginationParameters.Order
           );
        }

        public async Task DeleteAnnouncementByIdAsync(int id)
        {
            _dbContext.Announcements.Remove(new Announcement { AnnouncementId = id });
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAnnouncementAsync(int id, string? title, string? contents, HashSet<EUserRole>? authorizedRoles)
        {
            var foundAnnouncement = await _dbContext.Announcements
                .Where(a => a.AnnouncementId == id)
                .Include(a => a.AnnouncementRoles)
                .SingleOrDefaultAsync();

            if (foundAnnouncement != null)
            {
                foundAnnouncement.Title = title ?? foundAnnouncement.Title;
                foundAnnouncement.Contents = contents ?? foundAnnouncement.Contents;

                if (authorizedRoles != null)
                {
                    authorizedRoles.Add(EUserRole.Admin);
                    var currentlyAuthorizedRoles = foundAnnouncement.AnnouncementRoles.Select(ar => ar.Role).ToHashSet();

                    var rolesToDeauthorize = currentlyAuthorizedRoles.Except(authorizedRoles);
                    if (rolesToDeauthorize.Any()) _dbContext.AnnouncementRoles.RemoveRange(_dbContext.AnnouncementRoles.Where(ar => ar.AnnouncementId == foundAnnouncement.AnnouncementId && rolesToDeauthorize.Contains(ar.Role)));

                    var rolesToAuthorize = authorizedRoles.Except(currentlyAuthorizedRoles);
                    if (rolesToAuthorize.Any()) _dbContext.AnnouncementRoles.AddRange(rolesToAuthorize.Select(r => new AnnouncementRole()
                    {
                        AnnouncementId = foundAnnouncement.AnnouncementId,
                        Role = r
                    }));
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task InsertAnnouncementAsync(Announcement announcement)
        {
            await _dbContext.Announcements.AddAsync(announcement);
            await _dbContext.SaveChangesAsync();
        }
    }
}
