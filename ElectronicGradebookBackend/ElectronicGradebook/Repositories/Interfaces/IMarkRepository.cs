using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;

namespace ElectronicGradebook.Repositories.Interfaces
{
    public interface IMarkRepository
    {
        Task<Pupil> SelectPupilsMarksAsync(int pupilId);
        Task<Pupil> SelectPupilsMarksAsync(int pupilId, int teacherId);
        Task<int> InsertMarkAsync(MarkDetailsToInsertDTO markDetailsToInsertDTO);
        Task UpdateMarkAsync(MarkDetailsToUpdateDTO markDetailsToUpdateDTO);
        Task DeleteMarkAsync(int id);
        Task<List<Pupil>> SelectClassPartialMarksAsync(int classId, int subjectId);
    }
}
