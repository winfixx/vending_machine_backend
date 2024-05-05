namespace VendingMachine.Core.Interfaces.Mappers
{
    public interface IMapper<D, E>
    {
        D ToDomain(E entity);
        E ToEntity(D domainModel);
    }
}
