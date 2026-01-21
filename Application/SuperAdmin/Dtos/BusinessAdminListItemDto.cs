namespace Application.SuperAdmin.Dtos
{
    public record BusinessAdminListItemDto(
        Guid Id,
        string Name,
        string Status, // Viktigt för SuperAdmin!
        DateTime CreatedAt,
        string OwnerEmail
    );
}