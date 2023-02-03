using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface ISurveyService
    {
        SurveyPagedResponse GetSurveysWithTheirAuthors(BasePaginationParameters<ESurveySortableProperties> basePaginationParameters, EUserRole role, int userId);
        Task CreateSurveyAsync(SurveyDetailsToInsertDTO surveyDetailsToInsertDTO);
        Task<SurveyExtendedDetailsToSelectDTO> SelectSurveyAsync(int surveyId);
        Task FillSurveyAsync(int userId, int surveyId, HashSet<int> selectedAnswersIds);
        Task<SurveyStatisticalDataToSelectDTO> SelectSurveyStatisticalDataAsync(int surveyId);
    }
}
