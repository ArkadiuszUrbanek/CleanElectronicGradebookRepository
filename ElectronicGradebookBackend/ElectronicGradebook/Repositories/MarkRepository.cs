using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicGradebook.Repositories
{
    public class MarkRepository : IMarkRepository
    {
        private readonly ElectronicGradebookDatabaseContext _dbContext;

        public MarkRepository(ElectronicGradebookDatabaseContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<Pupil> SelectPupilsMarksAsync(int pupilId)
        {
            return await _dbContext.Pupils
                 .Include(p => p.Class)
                 .ThenInclude(c => c.ClassesSubjectsTeachers)
                 .ThenInclude(cst => cst.Subject)
                 .ThenInclude(s => s.Marks.Where(m => m.PupilId == pupilId))
                 .ThenInclude(m => m.User)
                 .FirstAsync(p => p.UserId == pupilId);
        }

        public async Task<Pupil> SelectPupilsMarksAsync(int pupilId, int teacherId)
        {
            return await _dbContext.Pupils
                 .Include(p => p.Class)
                 .ThenInclude(c => c.ClassesSubjectsTeachers.Where(cst => cst.TeacherId == teacherId))
                 .ThenInclude(cst => cst.Subject)
                 .ThenInclude(s => s.Marks.Where(m => m.PupilId == pupilId))
                 .ThenInclude(m => m.User)
                 .FirstAsync(p => p.UserId == pupilId);
        }

        public async Task<int> InsertMarkAsync(MarkDetailsToInsertDTO markDetailsToInsertDTO)
        {
            Mark mark = new Mark()
            {
                Value = markDetailsToInsertDTO.Value,
                Weight = markDetailsToInsertDTO.Weight,
                SubjectId = markDetailsToInsertDTO.SubjectId,
                PupilId = markDetailsToInsertDTO.PupilId,
                UserId = markDetailsToInsertDTO.IssuerId,
                Semester = markDetailsToInsertDTO.Semester,
                IssueDate = DateTime.UtcNow,
                Type = markDetailsToInsertDTO.Type,
                Category = markDetailsToInsertDTO.Category
            };

            await _dbContext.Marks.AddAsync(mark);
            await _dbContext.SaveChangesAsync();
            return mark.MarkId;
        }

        public async Task UpdateMarkAsync(MarkDetailsToUpdateDTO markDetailsToUpdateDTO)
        {
            var mark = await _dbContext.Marks.SingleOrDefaultAsync(m => m.MarkId == markDetailsToUpdateDTO.Id);

            if (mark == null) return;

            mark.Value = markDetailsToUpdateDTO.Value ?? mark.Value;

            if (mark.Type == EMarkType.Partial)
            {
                mark.Weight = markDetailsToUpdateDTO.Weight ?? mark.Weight;
                mark.Category = markDetailsToUpdateDTO.Category ?? mark.Category;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMarkAsync(int id)
        {
            _dbContext.Marks.Remove(new Mark() { MarkId = id });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Pupil>> SelectClassPartialMarksAsync(int classId, int subjectId)
        {
            return await _dbContext.Pupils
                .Where(pupil => pupil.ClassId == classId)
                .Include(pupil => pupil.Marks.Where(mark => mark.SubjectId == subjectId && mark.Type == EMarkType.Partial))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
