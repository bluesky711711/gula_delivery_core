using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ErpCore.RepoFramework
{
    public interface IUnitOfWorkBase
    {
        DataContextBase DbContext { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbContextTransaction BeginTransaction();
        void Dispose(bool disposing);
    }

    public class UnitOfWorkBase : IUnitOfWorkBase, IDisposable
    {
        public DataContextBase DbContext { get; }
        private bool _disposed;

        public UnitOfWorkBase(DataContextBase dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            DbContext = dbContext;
        }

        public virtual int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public DbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                DbContext.Dispose();
            }

            _disposed = true;
        }
    }
}
