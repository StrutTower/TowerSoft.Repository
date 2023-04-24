using Dapper;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.Repository.Builders;
using TowerSoft.Repository.Interfaces;

namespace TowerSoft.Repository {
    public partial class DbRepository<T> : IDbRepository {
        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        /// <returns></returns>
        public virtual long GetCount() {
            return GetCountAsync(Query).Result;
        }

        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync() {
            return await GetCountAsync(ConvertFluentToQueryBuilder(Query, isCountQuery: true));
        }

        /// <summary>
        /// Returned the total count of rows matching the built query
        /// </summary>
        /// <param name="builder">FluentQueryBuilder</param>
        /// <returns></returns>
        protected virtual long GetCount(FluentQueryBuilder<T> builder) {
            return GetCountAsync(builder).Result;
        }

        /// <summary>
        /// Returned the total count of rows matching the written query
        /// </summary>
        /// <param name="builder">=QueryBuilder</param>
        /// <returns></returns>
        protected virtual long GetCount(QueryBuilder builder) {
            return GetCountAsync(builder).Result;
        }

        /// <summary>
        /// Return the number of rows that match the supplied WhereConditions
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereCondtions to filter by</param>
        /// <returns></returns>
        protected virtual async Task<long> GetCountAsync(FluentQueryBuilder<T> builder) {
            return await GetCountAsync(ConvertFluentToQueryBuilder(builder, isCountQuery: true));
        }

        protected virtual async Task<long> GetCountAsync(QueryBuilder builder) {
            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {builder.SqlQuery} /Parameters/ {string.Join(", ", builder.Parameters.Select(x => x.Key + ":" + x.Value))}");

            return await GetDbConnection().QuerySingleAsync<long>(builder.SqlQuery, builder.Parameters, DbAdapter.DbTransaction);
        }
    }
}
