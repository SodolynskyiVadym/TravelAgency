namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IPaymentRepository
{
    public Task DeleteUnpaid();
}