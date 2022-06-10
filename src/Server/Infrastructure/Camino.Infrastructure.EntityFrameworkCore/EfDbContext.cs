﻿using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace Camino.Infrastructure.EntityFrameworkCore
{
    public abstract class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions options) : base(options) { }

        public string GenerateCreateScript()
        {
            return Database.GenerateCreateScript();
        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return Set<TEntity>().FromSqlRaw(CreateSqlWithParameters(sql, parameters), parameters);
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityEntry = Entry(entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        /// <summary>
        /// Modify the input SQL query by adding passed parameters
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>Modified raw SQL query</returns>
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }
    }
}
