namespace ElectronicGradebook.Models
{
    public partial class Pupil
    {
        public Pupil()
        {
            Attendances = new HashSet<Attendance>();
            Marks = new HashSet<Mark>();
            Parents = new HashSet<User>();
        }

        public int UserId { get; set; }
        public int? ClassId { get; set; }
        public string? SecondName { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual Class? Class { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }

        public virtual ICollection<User> Parents { get; set; }
    }
}
