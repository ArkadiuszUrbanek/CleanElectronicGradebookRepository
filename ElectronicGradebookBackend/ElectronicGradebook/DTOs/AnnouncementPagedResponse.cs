using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;

namespace ElectronicGradebook.DTOs
{
    public class AnnouncementPagedResponse : BasePagedResponse<AnnouncementDetailsToSelectDTO, Announcement, EAnnouncementSortableProperties> 
        
    {
        public AnnouncementPagedResponse(IQueryable<Announcement> source, int pageNumber, int pageSize, EAnnouncementSortableProperties orderBy, EOrder order) : base(source, pageNumber, pageSize, orderBy, order)
        {
        }

        protected override IQueryable<Announcement> PerformSortingLogic(IQueryable<Announcement> source, EAnnouncementSortableProperties orderBy, EOrder order)
        {
            switch (order) {
                case EOrder.Ascending:
                    { 
                        switch (orderBy) {
                            case EAnnouncementSortableProperties.Id: return source.OrderBy(a => a.AnnouncementId);
                            case EAnnouncementSortableProperties.Title: return source.OrderBy(a => a.Title);
                            case EAnnouncementSortableProperties.CreationDate: return source.OrderBy(a => a.CreationDate.Date).ThenBy(a => a.CreationDate.TimeOfDay);
                            default: return source;

                        }
                    }
                case EOrder.Descending:
                    {
                        switch (orderBy)
                        {
                            case EAnnouncementSortableProperties.Id: return source.OrderByDescending(a => a.AnnouncementId);
                            case EAnnouncementSortableProperties.Title: return source.OrderByDescending(a => a.Title);
                            case EAnnouncementSortableProperties.CreationDate: return source.OrderByDescending(a => a.CreationDate.Date).ThenByDescending(a => a.CreationDate.TimeOfDay);
                            default: return source;
                        }
                    }
                default: return source;
            }
        }

        protected override IQueryable<AnnouncementDetailsToSelectDTO> PerformMapping(IQueryable<Announcement> source, int? userId)
        {
            return source.Select(a => new AnnouncementDetailsToSelectDTO()
                {
                    Id = a.AnnouncementId,
                    Title = a.Title,
                    Contents = a.Contents,
                    CreationDate = a.CreationDate,
                    Author = new UserDetailsToSelectDTO()
                    {
                        Id = a.UserId,
                        FirstName = a.User.FirstName,
                        LastName = a.User.LastName,
                        Role = a.User.Role,
                        IsActive = a.User.IsActive
                    },
                    AuthorizedRoles = a.AnnouncementRoles.Select(a => a.Role).ToHashSet()
                }
            );
        }
    }
}
