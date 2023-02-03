using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public UserRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<int?> SelectPupilClassId(int pupilId)
        {
            return (await _dbContext.Pupils.SingleAsync(pupil => pupil.UserId == pupilId)).ClassId;
        }

        public async Task<User?> UpdateUserAccountActivityStatusAsync(int userId, bool isActive)
        {
            User? user = await _dbContext.Users.Where(user => user.UserId == userId).SingleOrDefaultAsync();
            if (user != null) user.IsActive = isActive;
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task UpdateTeacherContactDetailsAsync(int teacherId, string? contactNumber, string? contactEmail)
        {
            var foundTeacher = await _dbContext.Teachers
                .Where(teacher => teacher.UserId == teacherId)
                .SingleOrDefaultAsync();

            if (foundTeacher != null)
            {
                foundTeacher.ContactNumber = contactNumber;
                foundTeacher.ContactEmail = contactEmail;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<User>> SelectAllTeachersAsync(int classId)
        {
            return await _dbContext.Users
                .IgnoreQueryFilters()
                .Where(user => user.Role == EUserRole.Teacher)
                .Include(user => user.Teacher)
                    .ThenInclude(teacher => teacher!.ClassesSubjectsTeachers.Where(classSubjectTeacher => classSubjectTeacher.ClassId == classId))
                        .ThenInclude(classSubjectTeacher => classSubjectTeacher.Subject)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<User>> SelectAllChildrenAsync(int userId)
        {
            return await _dbContext.Users
                .Where(user => user.UserId == userId)
                .SelectMany(user => user.Pupils)
                .Select(pupil => pupil.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<User>> SelectAllPupilsAsync(int classId)
        {
            return await _dbContext.Users
                .Where(u => u.Role == EUserRole.Pupil)
                .Include(u => u.Pupil)
                .Where(u => u.Pupil!.ClassId == classId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<User>> SelectAllUsersAsync(EUserRole userRole)
        { 
            return await _dbContext.Users
                .Where(u => u.Role == userRole)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateSurveyCompletionDate(int surveyId, int userId)
        {
            (await _dbContext.UsersSurveys.SingleAsync(x => x.SurveyId == surveyId && x.UserId == userId)).CompletionDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task AssignUsersToSurvey(EUserRole[] roles, int surveyId, int authorId)
        {
            List<User> users = await _dbContext.Users.Where(u => roles.Contains(u.Role) && u.UserId != authorId).ToListAsync();
            foreach (var user in users) user.UsersSurveys.Add(new UsersSurvey() 
            {
                SurveyId = surveyId,
                CompletionDate = null
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> SelectUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public UserPagedResponse SelectUserPagedReponse(UsersToSelectPaginationParameters usersToSelectPaginationParameters)
        {
            bool searchByAnyName = usersToSelectPaginationParameters.SearchPhrase == null;
            return new UserPagedResponse(
                _dbContext.Users.Where(u => searchByAnyName || (u.FirstName + ' ' + u.LastName).ToLower().StartsWith(usersToSelectPaginationParameters.SearchPhrase!.ToLower())),
                usersToSelectPaginationParameters.PageNumber,
                usersToSelectPaginationParameters.PageSize,
                usersToSelectPaginationParameters.SortBy,
                usersToSelectPaginationParameters.Order
           );
        }

        public async Task<int> InsertUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.UserId;
        }

        public async Task<int> InsertPupilAsync(User user, Pupil pupil)
        {
            pupil.UserId = await InsertUserAsync(user);
            await _dbContext.Pupils.AddAsync(pupil);
            await _dbContext.SaveChangesAsync();
            return pupil.UserId;
        }

        public async Task<int> InsertTeacherAsync(User user, Teacher teacher)
        {
            teacher.UserId = await InsertUserAsync(user);
            await _dbContext.Teachers.AddAsync(teacher);
            await _dbContext.SaveChangesAsync();
            return teacher.UserId;
        }
    }
}
