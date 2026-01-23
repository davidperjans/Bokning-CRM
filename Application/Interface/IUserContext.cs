namespace Application.Interface
{
    public interface IUserContext
    {
        Guid? UserId { get; }

        Guid? BusinessId { get; }
        string? Role { get; }
    }
}