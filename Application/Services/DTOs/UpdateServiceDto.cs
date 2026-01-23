namespace Application.Services.DTOs
{
    public record UpdateServiceDto(
        Guid Id,
        string Name,
        string Description,
        int DurationMinutes,
        decimal Price,
        bool IsActive,
        Guid BusinessId
    );
}