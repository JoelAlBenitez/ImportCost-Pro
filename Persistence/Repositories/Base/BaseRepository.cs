
namespace Persistence.Repositories.Base
{
    public interface BaseRepository<TEntity, Tkey>
    {
       Task<bool> CreateAsync(TEntity entity);
       Task<bool> EditAsync (TEntity entity);
       Task<bool> DeleteAsync (TEntity entity);
       Task<TEntity> GetEntityById(Tkey key);
       Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    }
}
