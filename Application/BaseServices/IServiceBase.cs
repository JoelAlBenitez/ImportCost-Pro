namespace Application.Services.BaseServices
{
    public interface IServicesBase<TDto, TKey>
    {
        Task<bool> CreateAsync(TDto dto);
        Task<bool> EditAsync(TDto dto);
        Task<TDto> GetKeyAsync(TKey key);
        Task<IReadOnlyCollection<TDto>> GetAllAsync();
        Task<bool> DeleteAsync(TKey key);

    }
}