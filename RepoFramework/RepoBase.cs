using ErpCore.EntityFramework.Attributes;
using System;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using static ErpCore.RepoFramework.RepoConstants;

namespace ErpCore.RepoFramework
{

    public interface IRepoBase<T> where T : class
    {
        void Update(T newEntity, params Expression<Func<T, object>>[] updateProps);
        int Update_ByInfoGroup(T newEntity, InfoGroupFilterTypes updateMethod, string infoGroupFilterStr, bool updateNonInfoGroup = true);
    }

    public class RepoBase<T> : IRepoBase<T> where T : class
    {
        //TODO db context warp
        //TODO async 
        private readonly DataContextBase dbContext;
        private readonly DbSet<T> dbSet;

        public RepoBase(DataContextBase _dbContext)
        {
            dbContext = _dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
            dbSet = dbContext.Set<T>();
        }

        public void Update(T newEntity, params Expression<Func<T, object>>[] updateProps)
        {
            dbSet.Attach(newEntity);

            foreach (var p in updateProps)
            {
                dbContext.Entry(newEntity).Property(p).IsModified = true;
            }
        }

        public int Update_ByInfoGroup(T newEntity, InfoGroupFilterTypes updateMethod, string infoGroupFilterStr, bool updateNonInfoGroup = false)
        {
            var infoGroupFilter = infoGroupFilterStr.Trim().ToUpper().Split(',');
            dbSet.Attach(newEntity);
            var conEntity = dbContext.Entry(newEntity);
            bool isInclude = updateMethod == InfoGroupFilterTypes.Inlcude;
            int updatedPropCount = 0;

            foreach (var prop in newEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(t =>
                 !t.GetGetMethod().IsVirtual
                 && t.GetCustomAttribute<KeyAttribute>(false) == null
                )
                )
            {
                var infoTypeAttr = prop.GetCustomAttribute<InfoGroupAttr>(false);

                if (infoTypeAttr != null && infoTypeAttr.InfoGroups.Any(t => infoGroupFilter.Contains(t.Trim().ToUpper())) == isInclude)
                {
                    conEntity.Property(prop.Name).IsModified = true;
                    updatedPropCount++;
                }
                else if (infoTypeAttr == null && updateNonInfoGroup)
                {
                    conEntity.Property(prop.Name).IsModified = true;
                    updatedPropCount++;
                }
            }

            return updatedPropCount;
        }

    }
}
