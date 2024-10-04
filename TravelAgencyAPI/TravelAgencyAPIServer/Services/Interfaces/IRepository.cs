using TravelAgencyAPIServer.DTO;
using IModel = TravelAgencyAPIServer.Models.IModel;

namespace TravelAgencyAPIServer.Services.Interfaces;

public interface IRepository<T, in TDto> where T : class, IModel where TDto : class, IDto
{
    Task<T?> GetByIdWithIncludeAsync(int id);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<int> AddAsync(TDto entity);
    Task<bool> UpdateAsync(TDto entity);
    Task<bool> DeleteAsync(int id);
}