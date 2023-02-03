using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ElectronicGradebook.Models
{
    public partial class ElectronicGradebookDatabaseContext : DbContext
    {
        public ElectronicGradebookDatabaseContext()
        {
        }

        public ElectronicGradebookDatabaseContext(DbContextOptions<ElectronicGradebookDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Announcement> Announcements { get; set; } = null!;
        public virtual DbSet<AnnouncementRole> AnnouncementRoles { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Attendance> Attendances { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<ClassesSubjectsTeacher> ClassesSubjectsTeachers { get; set; } = null!;
        public virtual DbSet<Classroom> Classrooms { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<LessonsException> LessonsExceptions { get; set; } = null!;
        public virtual DbSet<Mark> Marks { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostReaction> PostReactions { get; set; } = null!;
        public virtual DbSet<PostRole> PostRoles { get; set; } = null!;
        public virtual DbSet<Pupil> Pupils { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Survey> Surveys { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<TeachingHour> TeachingHours { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UsersSurvey> UsersSurveys { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.AnnouncementId).HasColumnName("AnnouncementID");

                entity.Property(e => e.Contents)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Announcements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Announcements_Users");
            });

            modelBuilder.Entity<AnnouncementRole>(entity =>
            {
                entity.HasKey(e => new { e.AnnouncementId, e.Role });

                entity.Property(e => e.AnnouncementId).HasColumnName("AnnouncementID");

                entity.HasOne(d => d.Announcement)
                    .WithMany(p => p.AnnouncementRoles)
                    .HasForeignKey(d => d.AnnouncementId)
                    .HasConstraintName("FK_AnnouncementRoles_Announcements");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.Contents)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answers_Questions");
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.IssueDate).HasColumnType("datetime");

                entity.Property(e => e.PupilId).HasColumnName("PupilID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.TeachingHourId).HasColumnName("TeachingHourID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.PupilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendances_Pupils");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendances_Subjects");

                entity.HasOne(d => d.TeachingHour)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.TeachingHourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendances_TeachingHours");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Attendances)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attendances_Users");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.Name)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClassesSubjectsTeacher>(entity =>
            {
                entity.HasKey(e => new { e.ClassId, e.SubjectId, e.TeacherId });

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassesSubjectsTeachers)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassesSubjectsTeachers_Classes");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.ClassesSubjectsTeachers)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassesSubjectsTeachers_Subjects");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.ClassesSubjectsTeachers)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassesSubjectsTeachers_Teachers");
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.Property(e => e.ClassroomId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClassroomID");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.LessonId).HasColumnName("LessonID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.Property(e => e.TeachingHourId).HasColumnName("TeachingHourID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_Classes");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_Classrooms");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_Subjects");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_Teachers");

                entity.HasOne(d => d.TeachingHour)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.TeachingHourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lessons_TeachingHours");
            });

            modelBuilder.Entity<LessonsException>(entity =>
            {
                entity.HasKey(e => e.LessonExceptionId);

                entity.Property(e => e.LessonExceptionId).HasColumnName("LessonExceptionID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.LessonId).HasColumnName("LessonID");

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.LessonsExceptions)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK_LessonsExceptions_Lessons");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.LessonsExceptions)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_LessonsExceptions_Teachers");
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.Property(e => e.MarkId).HasColumnName("MarkID");

                entity.Property(e => e.IssueDate).HasColumnType("datetime");

                entity.Property(e => e.PupilId).HasColumnName("PupilID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Value).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.PupilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marks_Pupils");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marks_Subjects");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marks_Users");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.Text)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserReceiverId).HasColumnName("UserReceiverID");

                entity.Property(e => e.UserSenderId).HasColumnName("UserSenderID");

                entity.HasOne(d => d.UserReceiver)
                    .WithMany(p => p.MessageUserReceivers)
                    .HasForeignKey(d => d.UserReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Messages_UsersReceivers");

                entity.HasOne(d => d.UserSender)
                    .WithMany(p => p.MessageUserSenders)
                    .HasForeignKey(d => d.UserSenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Messages_UsersSenders");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.Contents)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<PostReaction>(entity =>
            {
                entity.Property(e => e.PostReactionId).HasColumnName("PostReactionID");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostReactions)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostReactions_Posts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostReactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostReactions_Users");
            });

            modelBuilder.Entity<PostRole>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.Role });

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostRoles)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostRoles_Posts");
            });

            modelBuilder.Entity<Pupil>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Pupils)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_Pupils_Classes");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Pupil)
                    .HasForeignKey<Pupil>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pupils_Users");

                entity.HasMany(d => d.Parents)
                    .WithMany(p => p.Pupils)
                    .UsingEntity<Dictionary<string, object>>(
                        "PupilsParent",
                        l => l.HasOne<User>().WithMany().HasForeignKey("ParentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PupilsParents_Parents"),
                        r => r.HasOne<Pupil>().WithMany().HasForeignKey("PupilId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_PupilsParents_Pupils"),
                        j =>
                        {
                            j.HasKey("PupilId", "ParentId");

                            j.ToTable("PupilsParents");

                            j.IndexerProperty<int>("PupilId").HasColumnName("PupilID");

                            j.IndexerProperty<int>("ParentId").HasColumnName("ParentID");
                        });
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Contents)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_Surveys");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Surveys_Users");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Teacher)
                    .HasForeignKey<Teacher>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Teachers_Users");
            });

            modelBuilder.Entity<TeachingHour>(entity =>
            {
                entity.Property(e => e.TeachingHourId).HasColumnName("TeachingHourID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMail");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).HasMaxLength(64);

                entity.Property(e => e.PasswordSalt).HasMaxLength(128);
            });

            modelBuilder.Entity<UsersSurvey>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SurveyId });

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.CompletionDate).HasColumnType("datetime");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.UsersSurveys)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersSurveys_Surveys");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersSurveys)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersSurveys_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
