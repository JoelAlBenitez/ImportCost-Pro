using Application.Services.Result;

namespace Application.Services.BaseServices
{
    public interface IServicesBase <TDto, TKey>
    {
        Task<ServiceResult> CreateAsync(TDto dto);
        Task<ServiceResult> EditAsync(TDto dto);
        Task<TDto> GetKeyAsync(TKey key);
        Task<IReadOnlyCollection<TDto>> GetAllAsync();
        Task<ServiceResult> DeleteAsync(TKey key);
  
    }
}
