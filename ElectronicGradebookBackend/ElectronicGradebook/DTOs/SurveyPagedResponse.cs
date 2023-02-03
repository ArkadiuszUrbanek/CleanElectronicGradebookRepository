using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;

namespace ElectronicGradebook.DTOs
{
    public class SurveyPagedResponse : BasePagedResponse<SurveyDetailsToSelectDTO, Survey, ESurveySortableProperties> 
        
    {
        public SurveyPagedResponse(IQueryable<Survey> source, int pageNumber, int pageSize, ESurveySortableProperties orderBy, EOrder order) : base(source, pageNumber, pageSize, orderBy, order)
        {
        }

        protected override IQueryable<Survey> PerformSortingLogic(IQueryable<Survey> source, ESurveySortableProperties orderBy, EOrder order)
        {
            switch (order) {
                case EOrder.Ascending:
                    { 
                        switch (orderBy) {
                            case ESurveySortableProperties.Id: return source.OrderBy(s => s.SurveyId);
                            case ESurveySortableProperties.Name: return source.OrderBy(s => s.Name);
                            case ESurveySortableProperties.CreationDate: return source.OrderBy(s => s.CreationDate.Date).ThenBy(s => s.CreationDate.TimeOfDay);
                            case ESurveySortableProperties.ExpirationDate: return source.OrderBy(s => s.ExpirationDate.Date).ThenBy(s => s.ExpirationDate.TimeOfDay);
                            case ESurveySortableProperties.AuthorFullName: return source.OrderBy(s => s.User.FirstName).ThenBy(s => s.User.LastName);
                            default: return source;
                        }
                    }
                case EOrder.Descending:
                    {
                        switch (orderBy)
                        {
                            case ESurveySortableProperties.Id: return source.OrderByDescending(s => s.SurveyId);
                            case ESurveySortableProperties.Name: return source.OrderByDescending(s => s.Name);
                            case ESurveySortableProperties.CreationDate: return source.OrderByDescending(s => s.CreationDate.Date).ThenByDescending(s => s.CreationDate.TimeOfDay);
                            case ESurveySortableProperties.ExpirationDate: return source.OrderByDescending(s => s.ExpirationDate.Date).ThenByDescending(s => s.ExpirationDate.TimeOfDay);
                            case ESurveySortableProperties.AuthorFullName: return source.OrderByDescending(s => s.User.FirstName).ThenByDescending(s => s.User.LastName);
                            default: return source;
                        }
                    }
                default: return source;
            }
        }

        protected override IQueryable<SurveyDetailsToSelectDTO> PerformMapping(IQueryable<Survey> source, int? userId)
        {
            return source.Select(s => new SurveyDetailsToSelectDTO()
                {
                    Id = s.SurveyId,
                    Name = s.Name,
                    Description = s.Description,
                    CreationDate = s.CreationDate,
                    ExpirationDate = s.ExpirationDate,
                    Author = new UserDetailsToSelectDTO()
                    {
                        Id = s.UserId,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Role = s.User.Role,
                        IsActive = s.User.IsActive
                    }
                }
            );
        }
    }
}
