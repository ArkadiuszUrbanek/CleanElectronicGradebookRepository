using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int?> SelectPupilClassId(int pupilId);
        Task<User?> UpdateUserAccountActivityStatusAsync(int userId, bool isActive);
        Task UpdateTeacherContactDetailsAsync(int teacherId, string? contactNumber, string? contactEmail);
        Task<List<User>> SelectAllTeachersAsync(int classId);
        Task<List<User>> SelectAllChildrenAsync(int userId);
        Task<List<User>> SelectAllPupilsAsync(int classId);
        Task<List<User>> SelectAllUsersAsync(EUserRole userRole);
        Task UpdateSurveyCompletionDate(int surveyId, int userId);
        Task AssignUsersToSurvey(EUserRole[] roles, int surveyId, int authorId);
        Task<User?> SelectUserByEmailAsync(string email);
        UserPagedResponse SelectUserPagedReponse(UsersToSelectPaginationParameters usersToSelectPaginationParameters);
        Task<int> InsertUserAsync(User user);
        Task<int> InsertPupilAsync(User user, Pupil pupil);
        Task<int> InsertTeacherAsync(User user, Teacher teacher);
    }
}
