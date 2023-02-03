namespace ElectronicGradebook.DTOs
{
    public class MessageDetailsToSelectDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
