using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Services.Interfaces
{
    public interface IMarkService
    {
        Task<IEnumerable<SubjectMarksDetailsToSelectDTO>> SelectPupilsMarksAsync(int pupilId, EUserRole userRole, int userId);
        Task<int> InsertMarkAsync(MarkDetailsToInsertDTO markDetailsToInsertDTO);
        Task UpdateMarkAsync(MarkDetailsToUpdateDTO markDetailsToUpdateDTO);
        Task DeleteMarkAsync(int id);
        Task<ICollection<MarksStatisticalDataToSelectDTO>> SelectPupilPartialMarksMonthlyStatisticalDataAsync(int pupilId, int subjectId);
    }
}
