namespace Domain.Models
{
    public class WorkingHour
    {
        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        
        public DayOfWeek DayOfWeek { get; set; } // 0 = Söndag, 1 = Måndag...
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsClosed { get; set; }

        // Navigation property tillbaka till Staff
        public Staff Staff { get; set; } = null!;
    }
}