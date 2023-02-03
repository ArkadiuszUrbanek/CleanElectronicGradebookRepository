using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Repositories.Interfaces;
using ElectronicGradebook.Services.Interfaces;

namespace ElectronicGradebook.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;
        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public AnnouncementPagedResponse SelectAnnouncements(BasePaginationParameters<EAnnouncementSortableProperties> basePaginationParameters, EUserRole userRole, int userId)
        {
            switch (userRole) {
                case EUserRole.Pupil:
                case EUserRole.Parent:
                    return _announcementRepository.SelectAnnouncementPagedReponse(basePaginationParameters, a => a.AnnouncementRoles.Contains(
                        new AnnouncementRole()
                        {
                            AnnouncementId = a.AnnouncementId,
                            Role = userRole
                        }));

                case EUserRole.Teacher:
                    return _announcementRepository.SelectAnnouncementPagedReponse(basePaginationParameters, a => a.UserId == userId || a.AnnouncementRoles.Contains(
                        new AnnouncementRole()
                        {
                            AnnouncementId = a.AnnouncementId,
                            Role = EUserRole.Teacher
                        }));

                case EUserRole.Admin:
                    return _announcementRepository.SelectAnnouncementPagedReponse(basePaginationParameters, s => true);

                default: return null;
            }
        }

        public async Task DeleteAnnouncementByIdAsync(int id)
        {
            await _announcementRepository.DeleteAnnouncementByIdAsync(id);
        }

        public async Task UpdateAnnouncementAsync(AnnouncementDetailsToUpdateDTO announcementDetailsToUpdateDTO)
        {
            await _announcementRepository.UpdateAnnouncementAsync(announcementDetailsToUpdateDTO.Id,
                                                                  announcementDetailsToUpdateDTO.Title,
                                                                  announcementDetailsToUpdateDTO.Contents,
                                                                  announcementDetailsToUpdateDTO.AuthorizedRoles);
        }

        public async Task InsertAnnouncementAsync(AnnouncementDetailsToInsertDTO announcementDetailsToInsertDTO)
        {
            Announcement announcement = new Announcement()
            {
                UserId = announcementDetailsToInsertDTO.AuthorId,
                Title = announcementDetailsToInsertDTO.Title,
                Contents = announcementDetailsToInsertDTO.Contents,
                CreationDate = DateTime.UtcNow
            };

            announcementDetailsToInsertDTO.AuthorizedRoles.Add(EUserRole.Admin);

            foreach (EUserRole authorizedRole in announcementDetailsToInsertDTO.AuthorizedRoles)
            {
                announcement.AnnouncementRoles.Add(new AnnouncementRole() { Role = authorizedRole });
            }

            await _announcementRepository.InsertAnnouncementAsync(announcement);
        }
    }
}
