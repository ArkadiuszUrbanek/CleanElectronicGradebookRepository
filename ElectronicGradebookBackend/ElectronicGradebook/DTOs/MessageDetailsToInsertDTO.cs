namespace ElectronicGradebook.DTOs
{
    public class MessageDetailsToInsertDTO
    {
        public string Text { get; set; } = null!;
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
