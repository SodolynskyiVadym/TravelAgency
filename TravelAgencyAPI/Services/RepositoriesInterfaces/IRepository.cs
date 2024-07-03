using TravelAgencyAPI.DTO;
using IModel = TravelAgencyAPI.Models.IModel;

namespace TravelAgencyAPI.Repositories.RepositoryInterfaces;

public interface IRepository<T, in TDto> where T : class, IModel where TDto : class, IDto
{
    Task<T?> GetByIdWithIncludeAsync(int id);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<int> AddAsync(TDto entity);
    Task<bool> UpdateAsync(TDto entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsUsedUniqueAttributes(TDto entity);
}