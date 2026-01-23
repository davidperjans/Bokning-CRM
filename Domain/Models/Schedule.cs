namespace Domain.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsDayOff { get; set; }
    }
}
