namespace ElectronicGradebook.DTOs
{
    public class SurveyDetailsToSelectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public UserDetailsToSelectDTO Author { get; set; } = null!;
    }
}
