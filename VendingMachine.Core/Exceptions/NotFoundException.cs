namespace VendingMachine.Core.Exceptions
{
    public class NotFoundException(string exceptionMessage) : Exception(exceptionMessage);
}
