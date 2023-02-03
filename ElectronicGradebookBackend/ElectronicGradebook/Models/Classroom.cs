namespace ElectronicGradebook.Models
{
    public partial class Classroom
    {
        public Classroom()
        {
            Lessons = new HashSet<Lesson>();
        }

        public short ClassroomId { get; set; }
        public byte FloorNumber { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
