using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq.Expressions;

namespace Camino.Infrastructure.EntityFrameworkCore
{
    public class BatchUpdate<TEntity> where TEntity : class
    {
        private DbSet<TEntity> _dbSet;
        private IAppDbContext _dbContext;

        public BatchUpdate(DbSet<TEntity> dbSet)
        {
            this._dbSet = dbSet;
            this._dbContext = dbSet.GetService<IAppDbContext>();
        }

        private BatchUpdate<TEntity> Set(LambdaExpression nameExpr, LambdaExpression valueExpr, Type propertType)
        {
            MemberExpression propExpression = nameExpr.Body as MemberExpression;
            string propertyName = propExpression.Member.Name;
            return this;
        }

        /// <summary>
        /// name is the expression of property's name, and value is the expression of the value
        /// </summary>
        /// <param name="name">something like: b=>b.Age</param>
        /// <param name="value">something like: b=>b.Age+1</param>
        /// <returns></returns>
        public BatchUpdate<TEntity> Set<TProperty>(Expression<Func<TEntity, TProperty>> name,
            Expression<Func<TEntity, TProperty>> value)
        {
            var propertyType = typeof(TProperty);
            return Set(name, value, propertyType);
        }


        public BatchUpdate<TEntity> Set<TProperty>(Expression<Func<TEntity, TProperty>> nameExpr,
            TProperty value)
        {
            var propertyType = typeof(TProperty);
            Expression valueExpr = Expression.Constant(value, propertyType);
            var pExpr = Expression.Parameter(typeof(TEntity));
            var valueLambdaExpr = Expression.Lambda<Func<TEntity, TProperty>>(valueExpr, pExpr);
            return Set(nameExpr, valueLambdaExpr, propertyType);
        }

        public BatchUpdate<TEntity> Set(string name, object value)
        {
            var propInfo = typeof(TEntity).GetProperty(name);
            Type propType = propInfo.PropertyType;//typeof of property

            var pExpr = Expression.Parameter(typeof(TEntity));
            Type tDelegate = typeof(Func<,>).MakeGenericType(typeof(TEntity), propType);

            var nameExpr = Expression.Lambda(tDelegate, Expression.MakeMemberAccess(pExpr, propInfo), pExpr);
            Expression valueExpr = Expression.Constant(value);
            if (value != null && value.GetType() != propType)
            {
                valueExpr = Expression.Convert(valueExpr, propType);
            }
            var valueLambdaExpr = Expression.Lambda(tDelegate, valueExpr, pExpr);
            return Set(nameExpr, valueLambdaExpr, propType);
        }
    }
}
