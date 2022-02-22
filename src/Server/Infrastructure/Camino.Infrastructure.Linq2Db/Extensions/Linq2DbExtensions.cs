using Camino.Core.Contracts.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB.Linq;

namespace Camino.Infrastructure.Linq2Db.Extensions
{
    public static class Linq2DbExtensions
    {
        /// <summary>
        /// Adds update field expression to query.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <typeparam name="TV">Updated field type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="extract">Updated field selector expression.</param>
        /// <param name="update">Updated field setter expression. Uses updated record as parameter.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        public static IEntryUpdate<T> SetEntry<T, TV>(this IQueryable<T> source, Expression<Func<T, TV>> extract,
            Expression<Func<T, TV>> update)
        {
            var updated = source.Set(extract, update);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <typeparam name="TV">Updated field type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="extract">Updated field selector expression.</param>
        /// <param name="update">Updated field setter expression. Uses updated record as parameter.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        public static IEntryUpdate<T> SetEntry<T, TV>(this IEntryUpdate<T> source, Expression<Func<T, TV>> extract,
            Expression<Func<T, TV>> update)
        {
            var updatable = GetUpdatable(source);
            var updated = updatable.Set(extract, update);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <typeparam name="TV">Updated field type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="extract">Updated field selector expression.</param>
        /// <param name="update">Updated field setter expression.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        public static IEntryUpdate<T> SetEntry<T, TV>(this IQueryable<T> source, Expression<Func<T, TV>> extract,
            Expression<Func<TV>> update)
        {
            var updated = source.Set(extract, update);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <typeparam name="TV">Updated field type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="extract">Updated field selector expression.</param>
        /// <param name="update">Updated field setter expression.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        public static IEntryUpdate<T> SetEntry<T, TV>(this IEntryUpdate<T> source, Expression<Func<T, TV>> extract,
            Expression<Func<TV>> update)
        {
            var updatable = GetUpdatable(source);
            var updated = updatable.Set(extract, update);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query. It can be any expression with string interpolation.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="setExpression">Custom update expression.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        /// <example>
        /// The following example shows how to append string value to appropriate field.
        /// <code>
        ///		db.Users.Where(u => u.UserId == id)
        ///			.Set(u => $"{u.Name}" += {str}")
        ///			.Update();
        /// </code>
        /// </example>
        public static IEntryUpdate<T> SetEntry<T>(this IQueryable<T> source, Expression<Func<T, string>> setExpression)
        {
            var updated = source.Set(setExpression);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query. It can be any expression with string interpolation.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="setExpression">Custom update expression.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        /// <example>
        /// The following example shows how to append string value to appropriate field.
        /// <code>
        ///		db.Users.Where(u => u.UserId == id)
        ///			.AsUpdatable()
        ///			.Set(u => $"{u.Name}" += {str}")
        ///			.Update();
        /// </code>
        /// </example>
        public static IEntryUpdate<T> SetEntry<T>(this IEntryUpdate<T> source, Expression<Func<T, string>> setExpression)
        {
            var updatable = GetUpdatable(source);
            var updated = updatable.Set(setExpression);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <typeparam name="TV">Updated field type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="extract">Updated field selector expression.</param>
        /// <param name="value">Value, assigned to updated field.</param>
        /// <returns><see cref="IUpdatable{T}"/> query.</returns>
        public static IEntryUpdate<T> SetEntry<T, TV>(this IQueryable<T> source, Expression<Func<T, TV>> extract, TV value)
        {
            var updated = source.Set(extract, value);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Adds update field expression to query.
        /// </summary>
        /// <typeparam name="T">Updated record type.</typeparam>
        /// <typeparam name="TV">Updated field type.</typeparam>
        /// <param name="source">Source query with records to update.</param>
        /// <param name="extract">Updated field selector expression.</param>
        /// <param name="value">Value, assigned to updated field.</param>
        /// <returns><see cref="IEntryUpdate{T}"/> query.</returns>
        public static IEntryUpdate<T> SetEntry<T, TV>(this IEntryUpdate<T> source, Expression<Func<T, TV>> extract,
            TV value)
        {
            var updatable = GetUpdatable(source);
            var updated = updatable.Set(extract, value);
            return new EntryUpdate<T>(updated);
        }

        /// <summary>
        /// Executes update operation asynchronously for already configured update query.
        /// </summary>
        /// <typeparam name="T">Updated table record type.</typeparam>
        /// <param name="source">Update query.</param>
        /// <param name="token">Optional asynchronous operation cancellation token.</param>
        /// <returns>Number of updated records.</returns>
        public static async Task<int> UpdateAsync<T>(this IEntryUpdate<T> source, CancellationToken token = default)
        {
            var updatable = GetUpdatable(source);
            return await updatable.UpdateAsync(token);
        }

        private static IUpdatable<T> GetUpdatable<T>(this IEntryUpdate<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var updatable = ((EntryUpdate<T>)source).Updatable;
            if(updatable == null)
            {
                throw new NullReferenceException(nameof(updatable));
            }

            return updatable;
        }
    }
}
