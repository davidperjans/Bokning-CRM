namespace Application.Businesses.DTOs
{
    public record BusinessSettingsDto(
        Guid Id,
        string Name,
        string Description,
        string? ImageUrl,
        string Category
    );
}