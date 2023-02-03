using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElectronicGradebook.Repositories
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public SurveyRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Survey>> SelectSurveysWithTheirAuthorsAsync()
        {
            return await _dbContext.Surveys
                .Include(s => s.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public SurveyPagedResponse SelectSurveyPagedReponse(BasePaginationParameters<ESurveySortableProperties> basePaginationParameters, Expression<Func<Survey, bool>> predicate)
        {
            return new SurveyPagedResponse(
                _dbContext.Surveys.Where(predicate).Include(s => s.User),
                basePaginationParameters.PageNumber,
                basePaginationParameters.PageSize,
                basePaginationParameters.SortBy,
                basePaginationParameters.Order
           );
        }

        public async Task<int> InsertSurveyAsync(Survey survey)
        {
            await _dbContext.Surveys.AddAsync(survey);
            await _dbContext.SaveChangesAsync();
            return survey.SurveyId;
        }

        public async Task<Survey?> SelectSurveyById(int surveyId)
        {
            return await _dbContext.Surveys
                .Where(s => s.SurveyId == surveyId)
                .Include(s => s.User)
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Survey?> SelectSurveyByIdJoiningQuestionsAndAnswers(int surveyId)
        {
            return await _dbContext.Surveys
                .Where(s => s.SurveyId == surveyId)
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Tuple<int, int>> SelectCountSurveyParticipantsAsync(int surveyId)
        { 
            var usersSurveys = _dbContext.UsersSurveys.Where(us => us.SurveyId == surveyId);
            var numberOfParticipantsWhoFinished = await usersSurveys.Where(us => us.CompletionDate != null).CountAsync();
            var numberOfParticipantsWhoUnFinished = await usersSurveys.AsNoTracking().CountAsync() - numberOfParticipantsWhoFinished;

            return new Tuple<int, int>(numberOfParticipantsWhoFinished, numberOfParticipantsWhoUnFinished);
        }
    }
}
