using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ITeachingHourRepository _teachinghourRepository;
        public AttendanceService(IAttendanceRepository attendanceRepository, ITeachingHourRepository teachingHourRepository)
        {
            _attendanceRepository = attendanceRepository;
            _teachinghourRepository = teachingHourRepository;
        }

        public async Task<WeeklyAttendanceDetailsToSelectDTO> SelectWeeklyAttendacesAsync(EUserRole userRole, int userId, DateOnly clientDate, int? classId)
        {
            int diff = (7 + (clientDate.DayOfWeek - DayOfWeek.Monday)) % 7;

            List<WorkdayDetailsToSelectDTO> workdaysDetails = new List<WorkdayDetailsToSelectDTO>();

            ELessonWorkday[] workdaysEnums = Enum.GetValues<ELessonWorkday>();
            for (int i = 0; i < workdaysEnums.Length; i++)
            {
                workdaysDetails.Add(new WorkdayDetailsToSelectDTO()
                {
                    Workday = workdaysEnums[i],
                    Date = clientDate.AddDays(-1 * diff + i)
                });
            }

            IEnumerable<PupilWeeklyAttendanceDetailsToSelectDTO> pupilsWeeklyAttendances = Enumerable.Empty<PupilWeeklyAttendanceDetailsToSelectDTO>();
            switch (userRole)
            {
                case EUserRole.Admin:
                case EUserRole.Teacher:
                {
                    Class classAttendances = await _attendanceRepository.SelectPupilsAttendancesAsync(clientDate.AddDays(-1 * diff), clientDate.AddDays(-1 * diff + 4), (int)classId!);
                    pupilsWeeklyAttendances = performAttendanceMapping(classAttendances.Pupils);
                    break;
                }

                case EUserRole.Parent:
                {
                    User parentChildrenAttendances = await _attendanceRepository.SelectChildrenAttendancesAsync(clientDate.AddDays(-1 * diff), clientDate.AddDays(-1 * diff + 4), userId);
                    pupilsWeeklyAttendances = performAttendanceMapping(parentChildrenAttendances.Pupils);
                    break;
                }

                case EUserRole.Pupil:
                {
                    Pupil pupilAttendances = await _attendanceRepository.SelectPupilAttendancesAsync(clientDate.AddDays(-1 * diff), clientDate.AddDays(-1 * diff + 4), userId);
                    pupilsWeeklyAttendances = performAttendanceMapping(new List<Pupil>() { pupilAttendances });
                    break;
                }

            }

            return new WeeklyAttendanceDetailsToSelectDTO()
            {
                Workdays = workdaysDetails,
                TeachingHoursIds = await _teachinghourRepository.SelectTeachingHoursIdsAsync(),
                PupilsWeeklyAttendances = pupilsWeeklyAttendances.OrderBy(p => p.Pupil.FirstName).ThenBy(p => p.Pupil.LastName)
            };
        }

        private IEnumerable<PupilWeeklyAttendanceDetailsToSelectDTO> performAttendanceMapping(ICollection<Pupil> pupils)
        {
            return pupils.Select(p =>
            {
                Dictionary<DayOfWeek, Dictionary<int, AttendanceDetailsToSelectDTO>> dailyAttendances = new Dictionary<DayOfWeek, Dictionary<int, AttendanceDetailsToSelectDTO>>();

                foreach (var attendance in p.Attendances)
                {
                    AttendanceDetailsToSelectDTO attendanceDetails = new AttendanceDetailsToSelectDTO()
                    {
                        Id = attendance.AttendanceId,
                        IssueDate = attendance.IssueDate,
                        Subject = new SubjectDetailsToSelectDTO()
                        {
                            Id = attendance.SubjectId,
                            Name = attendance.Subject.Name
                        },
                        Issuer = new UserShrinkedDetailsToSelectDTO()
                        {
                            Id = attendance.UserId,
                            FirstName = attendance.User.FirstName,
                            LastName = attendance.User.LastName
                        },
                        Type = attendance.Type
                    };

                    if (!dailyAttendances.ContainsKey(attendance.Date.DayOfWeek))
                    {
                        dailyAttendances.Add(attendance.Date.DayOfWeek, new Dictionary<int, AttendanceDetailsToSelectDTO>()
                        {
                            [attendance.TeachingHourId] = attendanceDetails
                        });
                    }
                    else
                    {
                        dailyAttendances[attendance.Date.DayOfWeek].Add(attendance.TeachingHourId, attendanceDetails);
                    }
                }

                return new PupilWeeklyAttendanceDetailsToSelectDTO()
                {
                    Pupil = new UserShrinkedDetailsToSelectDTO()
                    {
                        Id = p.UserId,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                    },
                    DailyAttendances = dailyAttendances
                };
            });
        }

        public async Task<int> InsertAttendaceAsync(AttendanceDetailsToInsertDTO attendanceDetailsToInsertDTO)
        {
            return await _attendanceRepository.InsertAttendanceAsync(attendanceDetailsToInsertDTO);
        }

        public async Task UpdateAttendaceAsync(AttendanceDetailsToUpdateDTO attendanceDetailsToUpdateDTO)
        {
            await _attendanceRepository.UpdateAttendanceAsync(attendanceDetailsToUpdateDTO);
        }

        public async Task DeleteAttendaceAsync(int attendanceId)
        {
            await _attendanceRepository.DeleteAttendanceAsync(attendanceId);
        }

        public async Task<ICollection<AttendanceStatisticalDataToSelectDTO>> SelectPupilAttendanceMonthlyStatisticalDataAsync(int pupilId)
        {
            var pupilAttendances = await _attendanceRepository.SelectPupilAttendancesAsync(pupilId);

            SortedDictionary<int, SortedDictionary<int, List<Attendance>>> attendancesDictionary = new SortedDictionary<int, SortedDictionary<int, List<Attendance>>>();
            foreach (var attendance in pupilAttendances)
            {
                if (!attendancesDictionary.ContainsKey(attendance.Date.Year))
                {
                    attendancesDictionary.Add(attendance.Date.Year, new SortedDictionary<int, List<Attendance>> { [attendance.Date.Month] = new List<Attendance>() { attendance } });
                }
                else
                {
                    if (!attendancesDictionary[attendance.Date.Year].ContainsKey(attendance.Date.Month))
                    {
                        attendancesDictionary[attendance.Date.Year].Add(attendance.Date.Month, new List<Attendance>() { attendance });
                    }
                    else
                    {
                        attendancesDictionary[attendance.Date.Year][attendance.Date.Month].Add(attendance);
                    }
                }
            }

            Dictionary<int, string> monthNames = new Dictionary<int, string>()
            {
                [1] = "styczeń",
                [2] = "luty",
                [3] = "marzec",
                [4] = "kwiecień",
                [5] = "maj",
                [6] = "czerwiec",
                [7] = "lipiec",
                [8] = "sierpień",
                [9] = "wrzesień",
                [10] = "październik",
                [11] = "listopad",
                [12] = "grudzień"
            };

            List<AttendanceStatisticalDataToSelectDTO> dtos = new List<AttendanceStatisticalDataToSelectDTO>();
            foreach (var year in attendancesDictionary)
            {
                foreach (var month in year.Value)
                {
                    Dictionary<EAttendanceType, int> statisticalData = new Dictionary<EAttendanceType, int>();
                    foreach (var attendanceEnum in Enum.GetValues<EAttendanceType>())
                    {
                        statisticalData.Add(attendanceEnum, 0);
                    }

                    foreach (var attendance in month.Value)
                    {
                        statisticalData[attendance.Type] += 1;
                    }

                    dtos.Add(new AttendanceStatisticalDataToSelectDTO()
                    {
                        Label = $"{monthNames[month.Key]} {year.Key}",
                        Data = statisticalData
                    });
                }
            }

            return dtos;
        }
    }
}

