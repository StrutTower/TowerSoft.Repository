using Dapper;
using System.Threading.Tasks;
using TowerSoft.Repository.Builders;
using TowerSoft.Repository.Interfaces;

namespace TowerSoft.Repository {
    public partial class DbRepository<T> : IDbRepository {
        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        public virtual long GetCount() {
            return GetCountAsync(Query).Result;
        }

        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        public virtual async Task<long> GetCountAsync() {
            return await GetCountAsync(ConvertFluentToQueryBuilder(Query, isCountQuery: true));
        }

        /// <summary>
        /// Returned the total count of rows matching the built query
        /// </summary>
        /// <param name="builder">FluentQueryBuilder. Get this by using the Query property on DbRepository.</param>
        protected virtual long GetCount(FluentQueryBuilder<T> builder) {
            return GetCountAsync(builder).Result;
        }

        /// <summary>
        /// Returned the total count of rows matching the written query
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        protected virtual long GetCount(QueryBuilder builder) {
            return GetCountAsync(builder).Result;
        }

        /// <summary>
        /// Return the number of rows that match the supplied WhereConditions
        /// </summary>
        /// <param name="builder">FluentQueryBuilder. Get this by using the Query property on DbRepository.</param>
        protected virtual async Task<long> GetCountAsync(FluentQueryBuilder<T> builder) {
            return await GetCountAsync(ConvertFluentToQueryBuilder(builder, isCountQuery: true));
        }

        /// <summary>
        /// Return the number of rows that match the supplied QueryBuilder
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        protected virtual async Task<long> GetCountAsync(QueryBuilder builder) {
            WriteLog(GetType().Name, builder.SqlQuery, builder.Parameters);

            return await GetDbConnection().QuerySingleAsync<long>(builder.SqlQuery, builder.Parameters, DbAdapter.DbTransaction);
        }
    }
}
