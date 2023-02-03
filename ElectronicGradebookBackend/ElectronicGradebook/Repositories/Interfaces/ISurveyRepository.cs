using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using System.Linq.Expressions;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface ISurveyRepository
    {
        Task<List<Survey>> SelectSurveysWithTheirAuthorsAsync();
        SurveyPagedResponse SelectSurveyPagedReponse(BasePaginationParameters<ESurveySortableProperties> basePaginationParameters, Expression<Func<Survey, bool>> predicate);
        Task<int> InsertSurveyAsync(Survey survey);
        Task<Survey?> SelectSurveyById(int surveyId);
        Task<Survey?> SelectSurveyByIdJoiningQuestionsAndAnswers(int surveyId);
        Task<Tuple<int, int>> SelectCountSurveyParticipantsAsync(int surveyId);
    }
}
