using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public SubjectRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<List<Subject>> SelectSubjectsAsync()
        {
            return await _dbContext.Subjects
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Teacher> SelectSubjectsTaughtByTeacherAsync(int teacherId, int classId)
        {
            return await _dbContext.Teachers
                .Where(teacher => teacher.UserId == teacherId)
                .Include(teacher => teacher.ClassesSubjectsTeachers.Where(classTeacherSubject => classTeacherSubject.ClassId == classId))
                .ThenInclude(classTeacherSubject => classTeacherSubject.Subject)
                .AsNoTracking()
                .FirstAsync();
        }
    }
}
