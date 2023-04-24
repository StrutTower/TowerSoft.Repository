using Dapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.Repository.Interfaces;

namespace TowerSoft.Repository {
    public partial class DbRepository<T> : IDbRepository {
        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        /// <returns></returns>
        public virtual long GetCount() {
            return GetCount(whereConditions: null);
        }

        /// <summary>
        /// Returns the number of rows that match the supplied WhereCondition
        /// </summary>
        /// <param name="whereCondition">WhereCondition to filter by</param>
        /// <returns></returns>
        protected virtual long GetCount(WhereCondition whereCondition) {
            return GetCount(new[] { whereCondition });
        }

        /// <summary>
        /// Return the number of rows that match the supplied WhereConditions
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereCondtions to filter by</param>
        /// <returns></returns>
        protected virtual long GetCount(IEnumerable<WhereCondition> whereConditions) {
            return GetCountAsync(whereConditions).Result;
        }

        /// <summary>
        /// Returns a total count of all rows in the table.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync() {
            return await GetCountAsync(whereConditions: null);
        }

        /// <summary>
        /// Returns the number of rows that match the supplied WhereCondition
        /// </summary>
        /// <param name="whereCondition">WhereCondition to filter by</param>
        /// <returns></returns>
        protected virtual async Task<long> GetCountAsync(WhereCondition whereCondition) {
            return await GetCountAsync(new[] { whereCondition });
        }

        /// <summary>
        /// Return the number of rows that match the supplied WhereConditions
        /// </summary>
        /// <param name="whereConditions">IEnumerable list of WhereCondtions to filter by</param>
        /// <returns></returns>
        protected virtual async Task<long> GetCountAsync(IEnumerable<WhereCondition> whereConditions) {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string query = $"SELECT COUNT(*) FROM {TableName} ";
            if (whereConditions != null && whereConditions.Any()) {
                List<string> whereStatements = new List<string>();
                int index = 1;
                foreach (WhereCondition whereCondition in whereConditions) {
                    if (whereCondition.IsNullEqualsOrNotEquals()) {
                        whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " NULL");
                    } else {
                        whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " " + DbAdapter.GetParameterPlaceholder(whereCondition.ColumnName, index));
                        parameters.Add(DbAdapter.GetParameterName(whereCondition.ColumnName, index), whereCondition.GetParameterValue());
                    }
                    index++;
                }
                query += "WHERE " + string.Join(" AND ", whereStatements);
            }

            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {query} /Parameters/ {string.Join(", ", parameters.Select(x => x.Key + ":" + x.Value))}");

            return await GetDbConnection().QuerySingleAsync<long>(query, parameters, DbAdapter.DbTransaction);
        }
    }
}
