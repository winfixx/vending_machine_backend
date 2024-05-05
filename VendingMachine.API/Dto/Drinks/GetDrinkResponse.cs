namespace VendingMachine.API.Dto.Drinks
{
    public record GetDrinkResponse(
        Guid Id,
        decimal Price,
        string Title,
        string Image,
        int Count);
}
