namespace Application.Users.Dtos
{
    public record LoginResponseDto(
        string Token,
        string RefreshToken,
        DateTime Expires,
        UserDto User);
}