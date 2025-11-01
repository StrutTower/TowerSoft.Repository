using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerSoft.Repository.Builders;
using TowerSoft.Repository.Interfaces;

namespace TowerSoft.Repository {
    public partial class DbRepository<T> : IDbRepository {
        /// <summary>
        /// Returns the first row from the database
        /// </summary>
        /// <param name="builder">FluentQueryBuilder</param>
        /// <returns></returns>
        protected T GetSingleEntity(FluentQueryBuilder<T> builder) {
            return GetEntities(builder).FirstOrDefault();
        }

        /// <summary>
        /// Returns the first row from the database
        /// </summary>
        /// <param name="builder">FluentQueryBuilder</param>
        /// <returns></returns>
        protected async Task<T> GetSingleEntityAsync(FluentQueryBuilder<T> builder) {
            return (await GetEntitiesAsync(builder)).FirstOrDefault();
        }

        /// <summary>
        /// Returns the first row from the database
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        /// <returns></returns>
        protected T GetSingleEntity(QueryBuilder builder) {
            return GetEntities(builder).FirstOrDefault();
        }

        /// <summary>
        /// Returns the first row from the database
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        /// <returns></returns>
        protected async Task<T> GetSingleEntityAsync(QueryBuilder builder) {
            return (await GetEntitiesAsync(builder)).FirstOrDefault();
        }

        /// <summary>
        /// Return all rows matching the query in FluentQueryBuilder
        /// </summary>
        /// <param name="builder">FluentQueryBuilder</param>
        /// <returns></returns>
        protected List<T> GetEntities(FluentQueryBuilder<T> builder) {
            return GetEntitiesAsync(builder).Result;
        }

        /// <summary>
        /// Return all rows matching the query in FluentQueryBuilder
        /// </summary>
        /// <param name="builder">FluentQueryBuilder</param>
        /// <returns></returns>
        protected async Task<List<T>> GetEntitiesAsync(FluentQueryBuilder<T> builder) {

            return await GetEntitiesAsync(ConvertFluentToQueryBuilder(builder));
        }

        /// <summary>
        /// Returns all rows returns from the SQL query
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        /// <returns></returns>
        protected virtual List<T> GetEntities(QueryBuilder builder) {
            IEnumerable<T> enumerable = GetDbConnection().Query<T>(builder.SqlQuery, builder.Parameters, DbAdapter.DbTransaction);
            List<T> entities = enumerable.ToList();
            PostProcessEntities(entities);
            return entities;
        }


        /// <summary>
        /// Returns all rows returns from the SQL query
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        /// <returns></returns>
        protected virtual async Task<List<T>> GetEntitiesAsync(QueryBuilder builder) {
            WriteLog(GetType().Name, builder.SqlQuery, builder.Parameters);
            IEnumerable<T> enumerable = await GetDbConnection().QueryAsync<T>(builder.SqlQuery, builder.Parameters, DbAdapter.DbTransaction);
            List<T> entities = enumerable.ToList();
            PostProcessEntities(entities);
            return entities;
        }

        private QueryBuilder ConvertFluentToQueryBuilder(FluentQueryBuilder<T> builder, bool isCountQuery = false) {
            QueryBuilder query = GetQueryBuilder();

            if (isCountQuery) {
                query.SqlQuery = $"SELECT COUNT(*) FROM {TableName} ";
            }

            List<string> whereStatements = [];
            int index = 1;
            foreach (WhereCondition whereCondition in builder.WhereConditions) {
                if (whereCondition.IsNullEqualsOrNotEquals()) {
                    whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " + whereCondition.GetComparisonString() + " NULL");
                } else {
                    whereStatements.Add(TableName + "." + whereCondition.ColumnName + " " +
                        whereCondition.GetComparisonString() + " " + DbAdapter.GetParameterPlaceholder(whereCondition.ColumnName, index));
                    query.AddParameter(DbAdapter.GetParameterName(whereCondition.ColumnName, index), whereCondition.GetParameterValue());
                }
                index++;
            }

            if (whereStatements.Any())
                query.SqlQuery += $"WHERE {string.Join(" AND ", whereStatements)} ";

            if (builder.OrderStatements != null && builder.OrderStatements.Any()) {
                List<string> orderBy = [];
                foreach (OrderStatement orderStatement in builder.OrderStatements) {
                    orderBy.Add(TableName + "." + orderStatement.ColumnName + (orderStatement.IsAscending ? "" : " DESC"));
                }
                query.SqlQuery += $"ORDER BY {string.Join(",", orderBy)} ";
            }

            if (builder.Limit.HasValue || builder.Offset.HasValue) {
                query.SqlQuery += DbAdapter.GetLimitOffsetStatement(builder.Limit, builder.Offset, query);
            }
            return query;
        }
    }
}
