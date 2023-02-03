namespace ElectronicGradebook.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public byte Number { get; set; }
        public string Contents { get; set; } = null!;
        public short TimesSelected { get; set; }
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
