using ElectronicGradebook.Models.Enums;

namespace ElectronicGradebook.Models
{
    public partial class User
    {
        public User()
        {
            Announcements = new HashSet<Announcement>();
            Attendances = new HashSet<Attendance>();
            Marks = new HashSet<Mark>();
            MessageUserReceivers = new HashSet<Message>();
            MessageUserSenders = new HashSet<Message>();
            PostReactions = new HashSet<PostReaction>();
            Posts = new HashSet<Post>();
            Surveys = new HashSet<Survey>();
            UsersSurveys = new HashSet<UsersSurvey>();
            Pupils = new HashSet<Pupil>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EUserRole Role { get; set; }
        public EUserGender Gender { get; set; }
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        public virtual Pupil? Pupil { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
        public virtual ICollection<Message> MessageUserReceivers { get; set; }
        public virtual ICollection<Message> MessageUserSenders { get; set; }
        public virtual ICollection<PostReaction> PostReactions { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
        public virtual ICollection<UsersSurvey> UsersSurveys { get; set; }

        public virtual ICollection<Pupil> Pupils { get; set; }
    }
}
