namespace ElectronicGradebook.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public int UserSenderId { get; set; }
        public int UserReceiverId { get; set; }

        public virtual User UserReceiver { get; set; } = null!;
        public virtual User UserSender { get; set; } = null!;
    }
}
