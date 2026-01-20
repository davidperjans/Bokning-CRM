namespace Application.Users.Dtos
{
    public record LoginResponseDto(
        string Token,
        DateTime Expires,
        UserDto User);
}