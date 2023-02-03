using ElectronicGradebook.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElectronicGradebook.Models
{
    public partial class ElectronicGradebookDatabaseContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostReaction>().Property(p => p.Type).HasConversion(new EnumToNumberConverter<EPostReaction, byte>());
            modelBuilder.Entity<Question>().Property(p => p.Type).HasConversion(new EnumToNumberConverter<EQuestionType, byte>());
            modelBuilder.Entity<Attendance>().Property(p => p.Type).HasConversion(new EnumToNumberConverter<EAttendanceType, byte>());
            modelBuilder.Entity<Lesson>().Property(p => p.Workday).HasConversion(new EnumToNumberConverter<ELessonWorkday, byte>());
            modelBuilder.Entity<LessonsException>().Property(p => p.Status).HasConversion(new EnumToNumberConverter<ELessonsExceptionStatus, byte>());
            modelBuilder.Entity<AnnouncementRole>().Property(p => p.Role).HasConversion(new EnumToNumberConverter<EUserRole, byte>());

            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.Role).HasConversion(new EnumToNumberConverter<EUserRole, byte>());
                e.Property(p => p.Gender).HasConversion(new EnumToNumberConverter<EUserGender, byte>());
            });

            modelBuilder.Entity<Mark>(e =>
            {
                e.Property(p => p.Semester).HasConversion(new EnumToNumberConverter<EMarkSemester, byte>());
                e.Property(p => p.Type).HasConversion(new EnumToNumberConverter<EMarkType, byte>());
                e.Property(p => p.Category).HasConversion(new EnumToNumberConverter<EMarkCategory, byte>());
            });
        }
    }
}
