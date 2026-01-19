namespace Application.Users.Dtos
{
    public record UserDto(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string Phone,
        DateTime? DateOfBirth,
        string Role,
        DateTime CreatedAt
    );
}