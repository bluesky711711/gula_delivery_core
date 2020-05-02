using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ErpCore.RepoFramework
{
    public interface IDataContextBase
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void SyncObjectsStatePostCommit();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
    }
}