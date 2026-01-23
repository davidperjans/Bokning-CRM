namespace Application.Admin.DTOs
{
    public sealed class AdminDashboardStatsDto
    {
        public int TotalBookings { get; set; }
        public int UpcomingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int BookingsToday { get; set; }
    }
}
