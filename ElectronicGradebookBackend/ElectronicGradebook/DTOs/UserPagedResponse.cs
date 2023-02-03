using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;

namespace ElectronicGradebook.DTOs
{
    public class UserPagedResponse : BasePagedResponse<UserDetailsToSelectDTO, User, EUserSortableProperties> 
        
    {
        public UserPagedResponse(IQueryable<User> source, int pageNumber, int pageSize, EUserSortableProperties orderBy, EOrder order) : base(source, pageNumber, pageSize, orderBy, order)
        {
        }

        protected override IQueryable<User> PerformSortingLogic(IQueryable<User> source, EUserSortableProperties orderBy, EOrder order)
        {
            switch (order) {
                case EOrder.Ascending:
                    {
                        switch (orderBy) {
                            case EUserSortableProperties.Id: return source.OrderBy(u => u.UserId);
                            case EUserSortableProperties.FirstName: return source.OrderBy(u => u.FirstName);
                            case EUserSortableProperties.LastName: return source.OrderBy(u => u.LastName);
                            case EUserSortableProperties.FullName: return source.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
                            case EUserSortableProperties.Role: return source.OrderBy(u => u.Role);
                            case EUserSortableProperties.Gender: return source.OrderBy(u => u.Gender);
                            default: return source;

                        }
                    }
                case EOrder.Descending:
                    {
                        switch (orderBy)
                        {
                            case EUserSortableProperties.Id: return source.OrderByDescending(u => u.UserId);
                            case EUserSortableProperties.FirstName: return source.OrderByDescending(u => u.FirstName);
                            case EUserSortableProperties.LastName: return source.OrderByDescending(u => u.LastName);
                            case EUserSortableProperties.FullName: return source.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName);
                            case EUserSortableProperties.Role: return source.OrderByDescending(u => u.Role);
                            case EUserSortableProperties.Gender: return source.OrderByDescending(u => u.Gender);
                            default: return source;
                        }
                    }
                default: return source;
            }
        }

        protected override IQueryable<UserDetailsToSelectDTO> PerformMapping(IQueryable<User> source, int? userId)
        {
            return source.Select(u => new UserDetailsToSelectDTO()
                {
                    Id = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role,
                    Gender = u.Gender,
                    IsActive = u.IsActive
                }
            );
        }
    }
}
