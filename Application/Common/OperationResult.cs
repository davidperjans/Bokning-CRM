namespace Application.Common
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string>? ValidationErrors { get; set; }
        
        protected OperationResult(bool isSuccess, T? value = default, string? errorMessage = null, List<string>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors;
        }
        
        public static OperationResult<T> Success(T value) => new(true, value, null);
        public static OperationResult<T> Failure(string error) => new(false, default, error);
        public static OperationResult<T> ValidationFailure(List<string> errors) => new(false, default, "Validation failed", errors);
    }
}