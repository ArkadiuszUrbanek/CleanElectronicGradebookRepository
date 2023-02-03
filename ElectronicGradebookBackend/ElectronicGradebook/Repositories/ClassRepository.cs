using ElectronicGradebook.Models;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class ClassRepository: IClassRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public ClassRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }
        public async Task<List<Class>> SelectClassesAsync() 
        { 
            return await _dbContext.Classes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Class>> SelectClassesTaughtByTeacherAsync(int teacherId)
        {
            List<int> classesIds = await _dbContext.ClassesSubjectsTeachers
                .Where(cst => cst.TeacherId == teacherId)
                .Select(cst => new { ClassId = cst.ClassId })
                .GroupBy(c => c.ClassId)
                .Select(c => c.First().ClassId)
                .ToListAsync();

            return await _dbContext.Classes
                .Where(c => classesIds.Contains(c.ClassId))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
