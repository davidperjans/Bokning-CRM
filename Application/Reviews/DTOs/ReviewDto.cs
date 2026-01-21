namespace Application.Reviews.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
