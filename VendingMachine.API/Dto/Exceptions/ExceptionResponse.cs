namespace VendingMachine.Core.Models.Exceptions
{
    public record ExceptionResponse(
        int Status,
        string ErrorMessage = "Необработанная ошибка");
}
