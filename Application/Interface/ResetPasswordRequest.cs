namespace Application.Interface
{
    public record ResetPasswordRequest
    (
        string Token,
        string NewPassword
    );
}