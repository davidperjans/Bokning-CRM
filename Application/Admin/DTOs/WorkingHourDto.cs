namespace Application.Admin.DTOs
{
    public record WorkingHourDto(
        DayOfWeek DayOfWeek,
        string? StartTime, // Format "HH:mm"
        string? EndTime,
        bool IsClosed
    );
}