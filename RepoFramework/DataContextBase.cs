using ErpCore.EntityFramework;
using ErpCore.EntityFramework.Attributes;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ErpCore.RepoFramework
{
    public class DataContextBase : DbContext, IDataContextBase
    {
        private string _userIdentity { get; set; }

        public DataContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        #region OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            DecimalPrecisionAttrOnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void DecimalPrecisionAttrOnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(x => x.GetCustomAttributes(false).OfType<DecimalPrecisionAttr>().Any()).Configure(c =>
            {
                var attr = (DecimalPrecisionAttr)c.ClrPropertyInfo.GetCustomAttributes(typeof(DecimalPrecisionAttr), true).FirstOrDefault();

                if (c.ClrPropertyInfo.PropertyType.IsGenericType && c.ClrPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    c.IsOptional();
                }

                c.HasPrecision(attr.Precision, attr.Scale);
            });

        }


        private void DecimalPrecisionAttrOnModelCreating2(DbModelBuilder modelBuilder)
        {
            foreach (Type classType in from t in Assembly.GetAssembly(typeof(DecimalPrecisionAttr)).GetTypes()
                                       where t.IsClass
                                       select t)
            {
                foreach (var propAttr in classType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<DecimalPrecisionAttr>() != null).Select(
                       p => new { prop = p, attr = p.GetCustomAttribute<DecimalPrecisionAttr>(true) }))
                {

                    var entityConfig = modelBuilder.GetType().GetMethod("Entity").MakeGenericMethod(classType).Invoke(modelBuilder, null);
                    ParameterExpression param = ParameterExpression.Parameter(classType, "c");
                    Expression property = Expression.Property(param, propAttr.prop.Name);
                    LambdaExpression lambdaExpression = Expression.Lambda(property, true,
                                                                             new ParameterExpression[]
                                                                                 {param});
                    DecimalPropertyConfiguration decimalConfig;
                    if (propAttr.prop.PropertyType.IsGenericType && propAttr.prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[7];
                        decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                    }
                    else
                    {
                        MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[6];
                        decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                    }

                    decimalConfig.HasPrecision(propAttr.attr.Precision, propAttr.attr.Scale);
                }
            }
        }

        //private void IngoreObjectState(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Types<EntityBase>().Configure(config => config.Ignore(x => x.ObjectState));
        //}

        #endregion

        public void SetUserIdentity(string userIdentity)
        {
            _userIdentity = userIdentity ?? "";
        }

        public override int SaveChanges()
        {
            UpdateAudiEntity();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            UpdateAudiEntity();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            //revers if cancel
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity.GetType().BaseType != typeof(EntityBase))
                {
                    continue;
                }

                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity.GetType().BaseType != typeof(EntityBase))
                {
                    continue;
                }

                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }


        private void UpdateAudiEntity()
        {
            var addOrUpdateEntities = ChangeTracker.Entries().Where(t => t.Entity is EntityBase && (t.State != EntityState.Unchanged || t.State != EntityState.Detached));

            DateTime now = DateTime.UtcNow;

            foreach (var entity in addOrUpdateEntities)
            {
                var entityBase = entity.Entity as EntityBase;

                if (entityBase != null)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            entityBase.CreatedAt = now;
                            entityBase.CreatedBy = _userIdentity;
                            break;

                        case EntityState.Modified:
                            entityBase.UpdatedAt = now;
                            entityBase.UpdatedBy = _userIdentity;
                            break;

                            //TODO handle soft delete
                    }
                }

            }
        }
    }
}
