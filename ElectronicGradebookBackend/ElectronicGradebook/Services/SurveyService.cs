using ElectronicGradebook.DTOs;
using ElectronicGradebook.Services.Interfaces;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;

namespace ElectronicGradebook.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAnswerRepository _answerRepository;
        public SurveyService(ISurveyRepository surveyRepository, IUserRepository userRepository, IAnswerRepository answerRepository)
        {
            _surveyRepository = surveyRepository;
            _userRepository = userRepository;
            _answerRepository =  answerRepository;
        }

        public SurveyPagedResponse GetSurveysWithTheirAuthors(BasePaginationParameters<ESurveySortableProperties> basePaginationParameters, EUserRole role, int userId)
        {
            switch (role) 
            {
                case EUserRole.Pupil:
                case EUserRole.Parent: 
                    return _surveyRepository.SelectSurveyPagedReponse(basePaginationParameters, s => s.UsersSurveys
                        .Where(us => us.UserId == userId && us.SurveyId == s.SurveyId && us.CompletionDate == null).Count() == 1);

                case EUserRole.Teacher:
                    return _surveyRepository.SelectSurveyPagedReponse(basePaginationParameters, s => s.UserId == userId || s.UsersSurveys
                        .Where(us => us.UserId == userId && us.SurveyId == s.SurveyId && us.CompletionDate == null).Count() == 1);

                case EUserRole.Admin:
                    return _surveyRepository.SelectSurveyPagedReponse(basePaginationParameters, s => true);

                default: return null;
            }
        }

        public async Task CreateSurveyAsync(SurveyDetailsToInsertDTO surveyDetailsToInsertDTO) 
        {
            Survey survey = new Survey()
            {
                Name = surveyDetailsToInsertDTO.Name,
                Description = surveyDetailsToInsertDTO.Description,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = surveyDetailsToInsertDTO.ExpirationDate,
                UserId = surveyDetailsToInsertDTO.AuthorId
            };

            for (int questionIndex = 0; questionIndex < surveyDetailsToInsertDTO.Questions.Length; questionIndex++)
            {
                Question question = new Question()
                {
                    Number = (byte)(questionIndex + 1),
                    Contents = surveyDetailsToInsertDTO.Questions[questionIndex].Contents,
                    Type = surveyDetailsToInsertDTO.Questions[questionIndex].Type
                };

                for (int answerIndex = 0; answerIndex < surveyDetailsToInsertDTO.Questions[questionIndex].AnswersContents.Length; answerIndex++) {
                    question.Answers.Add(new Answer() 
                    { 
                        Number = (byte)(answerIndex + 1),
                        Contents = surveyDetailsToInsertDTO.Questions[questionIndex].AnswersContents[answerIndex],
                        TimesSelected = 0
                    });
                }

                survey.Questions.Add(question);
            }

            int surveyId = await _surveyRepository.InsertSurveyAsync(survey);
            await _userRepository.AssignUsersToSurvey(surveyDetailsToInsertDTO.TargetedRoles, surveyId, surveyDetailsToInsertDTO.AuthorId);

        }

        public async Task<SurveyExtendedDetailsToSelectDTO> SelectSurveyAsync(int surveyId) 
        {
            var survey = await _surveyRepository.SelectSurveyById(surveyId);

            var surveyExtendedDetailsToSelectDTO = new SurveyExtendedDetailsToSelectDTO()
            {
                Id = survey!.SurveyId,
                Name = survey.Name,
                Description = survey.Description,
                CreationDate = survey.CreationDate,
                ExpirationDate = survey.ExpirationDate,
                Author = new UserDetailsToSelectDTO() 
                {
                    Id = survey.User.UserId,
                    FirstName = survey.User.FirstName,
                    LastName = survey.User.LastName,
                    Role = survey.User.Role,
                    Gender = survey.User.Gender,
                    IsActive = survey.User.IsActive
                },
                Questions = survey.Questions.Select(q => new QuestionDetailsToSelectDTO() 
                {
                    Id = q.QuestionId,
                    Number = q.Number,
                    Contents = q.Contents,
                    Type = q.Type,
                    Answers = q.Answers.Select(a => new AnswerDetailsToSelectDTO()
                    {
                        Id = a.AnswerId,
                        Number = a.Number,
                        Contents = a.Contents
                    })
                })
            };

            return surveyExtendedDetailsToSelectDTO;
        }

        public async Task FillSurveyAsync(int userId, int surveyId, HashSet<int> selectedAnswersIds)
        {
            await _answerRepository.UpdateSelectionTimes(selectedAnswersIds);
            await _userRepository.UpdateSurveyCompletionDate(surveyId, userId);
        }

        public async Task<SurveyStatisticalDataToSelectDTO> SelectSurveyStatisticalDataAsync(int surveyId)
        {
            var survey = await _surveyRepository.SelectSurveyByIdJoiningQuestionsAndAnswers(surveyId);
            var surveyParticipantsStats = await _surveyRepository.SelectCountSurveyParticipantsAsync(surveyId);

            var surveyStatisticalDataToSelectDTO = new SurveyStatisticalDataToSelectDTO()
            {
                Name = survey.Name,
                Description = survey.Description,
                TimesFinished = surveyParticipantsStats.Item1,
                TimesUnfinished = surveyParticipantsStats.Item2,
                Questions = survey.Questions.Select(q => new QuestionStatisticalDataToSelectDTO()
                {
                    Number = q.Number,
                    Contents = q.Contents,
                    Type = q.Type,
                    Answers = q.Answers.Select(a => new AnswerStatisticalDataToSelectDTO()
                    {
                        Number = a.Number,
                        Contents = a.Contents,
                        TimesSelected = a.TimesSelected
                    })
                })
            };

            return surveyStatisticalDataToSelectDTO;
        }
    }
}
