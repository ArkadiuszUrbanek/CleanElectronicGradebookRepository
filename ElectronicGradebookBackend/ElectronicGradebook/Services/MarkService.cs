using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository _markRepository;
        private readonly IUserRepository _userRepository;
        public MarkService(IMarkRepository markRepository, IUserRepository userRepository)
        {
            _markRepository = markRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<SubjectMarksDetailsToSelectDTO>> SelectPupilsMarksAsync(int pupilId, EUserRole userRole, int userId)
        {
            Pupil pupil;
            if (userRole == EUserRole.Teacher) pupil = await _markRepository.SelectPupilsMarksAsync(pupilId, userId);
            else pupil = await _markRepository.SelectPupilsMarksAsync(pupilId);

            if (pupil.Class == null) return Enumerable.Empty<SubjectMarksDetailsToSelectDTO>();
            if (pupil.Class.ClassesSubjectsTeachers.Count == 0) return Enumerable.Empty<SubjectMarksDetailsToSelectDTO>();

            return pupil.Class.ClassesSubjectsTeachers.Select(cst =>
                new SubjectMarksDetailsToSelectDTO()
                {
                    Subject = new SubjectDetailsToSelectDTO()
                    {
                        Id = cst.Subject.SubjectId,
                        Name = cst.Subject.Name,
                    },
                    Marks = cst.Subject.Marks.Select(m =>
                        new MarkDetailsToSelectDTO()
                        {
                            Id = m.MarkId,
                            Value = m.Value,
                            Weight = m.Weight,
                            Issuer = new UserDetailsToSelectDTO()
                            {
                                Id = m.User.UserId,
                                FirstName = m.User.FirstName,
                                LastName = m.User.LastName,
                                Role = m.User.Role,
                                Gender = m.User.Gender,
                                IsActive = m.User.IsActive
                            },
                            Semester = m.Semester,
                            IssueDate = m.IssueDate,
                            Type = m.Type,
                            Category = m.Category
                        }
                    )
                }
            );
        }

        public async Task<int> InsertMarkAsync(MarkDetailsToInsertDTO markDetailsToInsertDTO)
        {
            return await _markRepository.InsertMarkAsync(markDetailsToInsertDTO);
        }

        public async Task UpdateMarkAsync(MarkDetailsToUpdateDTO markDetailsToUpdateDTO)
        {
            await _markRepository.UpdateMarkAsync(markDetailsToUpdateDTO);
        }

        public async Task DeleteMarkAsync(int id)
        {
            await _markRepository.DeleteMarkAsync(id);
        }

        public async Task<ICollection<MarksStatisticalDataToSelectDTO>> SelectPupilPartialMarksMonthlyStatisticalDataAsync(int pupilId, int subjectId)
        {
            int? classId = await _userRepository.SelectPupilClassId(pupilId);
            if (classId == null) return new List<MarksStatisticalDataToSelectDTO>();

            List<Pupil> pupils = await _markRepository.SelectClassPartialMarksAsync((int)classId, subjectId);

            SortedDictionary<int, SortedDictionary<int, List<Mark>>> marksDictionary = new SortedDictionary<int, SortedDictionary<int, List<Mark>>>();
            foreach (var pupil in pupils)
            {
                foreach (var mark in pupil.Marks)
                {
                    if (!marksDictionary.ContainsKey(mark.IssueDate.Year))
                    {
                        marksDictionary.Add(mark.IssueDate.Year, new SortedDictionary<int, List<Mark>> { [mark.IssueDate.Month] = new List<Mark>() { mark } });
                    }
                    else
                    {
                        if (!marksDictionary[mark.IssueDate.Year].ContainsKey(mark.IssueDate.Month))
                        {
                            marksDictionary[mark.IssueDate.Year].Add(mark.IssueDate.Month, new List<Mark>() { mark });
                        }
                        else
                        {
                            marksDictionary[mark.IssueDate.Year][mark.IssueDate.Month].Add(mark);
                        }
                    }
                }
            }

            Dictionary<int, string> monthNames = new Dictionary<int, string>()
            {
                [1]  = "styczeń",
                [2]  = "luty",
                [3]  = "marzec",
                [4]  = "kwiecień",
                [5]  = "maj",
                [6]  = "czerwiec",
                [7]  = "lipiec",
                [8]  = "sierpień",
                [9]  = "wrzesień",
                [10] = "październik",
                [11] = "listopad",
                [12] = "grudzień"
            };

            List<MarksStatisticalDataToSelectDTO> dtos = new List<MarksStatisticalDataToSelectDTO>();
            foreach (var year in marksDictionary)
            {
                foreach (var month in year.Value)
                {
                    decimal PupilWeightedAverageNumerator = 0;
                    int PupilWeightedAverageDenominator = 0;

                    decimal ClassWeightedAverageNumerator = 0;
                    int ClassWeightedAverageDenominator = 0;

                    foreach (var mark in month.Value)
                    {
                        ClassWeightedAverageNumerator += (decimal)(mark.Value! * mark.Weight!);
                        ClassWeightedAverageDenominator += (int)mark.Weight!;

                        if (mark.PupilId == pupilId)
                        {
                            PupilWeightedAverageNumerator += (decimal)(mark.Value! * mark.Weight!);
                            PupilWeightedAverageDenominator += (int)mark.Weight!;
                        }
                    }

                    dtos.Add(new MarksStatisticalDataToSelectDTO()
                    {
                        Label = $"{monthNames[month.Key]} {year.Key}",
                        PupilWeightedAverage = PupilWeightedAverageDenominator == 0 ? 0 : Math.Round(PupilWeightedAverageNumerator / PupilWeightedAverageDenominator, 2),
                        ClassWeightedAverage = Math.Round(ClassWeightedAverageNumerator / ClassWeightedAverageDenominator, 2)
                    });
                }
            }

            return dtos;
        }
    }
}