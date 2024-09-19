using Ats.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ats.Data.EntityFramework
{
    public interface ISCDataContext
    {
        TEntity FindById<TEntity>(params object[] ids) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity, TPrimaryKey>(TEntity entity, params Expression<Func<TEntity, object>>[] properties) where TEntity : class, IEntity<TPrimaryKey>;
        void Delete<TEntity>(params object[] ids) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        IQueryable<TEntity> Get<TEntity>(string storedProcedureName, params object[] args) where TEntity : class;
        int Execute(string sqlCommand);
        int Execute(string sqlCommand, params object[] args);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        //DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        int SaveChanges(string userId);
        void Dispose();
    }
}