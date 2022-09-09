using Dapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="builder">QueryBuilder</param>
        /// <returns></returns>
        protected T GetSingleEntity(QueryBuilder builder) {
            return GetEntities(builder).FirstOrDefault();
        }

        /// <summary>
        /// Return all rows matching the query in FluentQueryBuilder
        /// </summary>
        /// <param name="builder">FluentQueryBuilder</param>
        /// <returns></returns>
        protected List<T> GetEntities(FluentQueryBuilder<T> builder) {
            QueryBuilder query = GetQueryBuilder();
            List<string> whereStatements = new List<string>();
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
                List<string> orderBy = new List<string>();
                foreach (OrderStatement orderStatement in builder.OrderStatements) {
                    orderBy.Add(TableName + "." + orderStatement.ColumnName + (orderStatement.IsAscending ? "" : " DESC"));
                }
                query.SqlQuery += $"ORDER BY {string.Join(",", orderBy)} ";
            }

            if (builder.Limit.HasValue || builder.Offset.HasValue) {
                query.SqlQuery += DbAdapter.GetLimitOffsetStatement(builder.Limit, builder.Offset, query);
            }
            return GetEntities(query);
        }

        /// <summary>
        /// Returns all rows returns from the SQL query
        /// </summary>
        /// <param name="builder">QueryBuilder</param>
        /// <returns></returns>
        protected virtual List<T> GetEntities(QueryBuilder builder) {
            if (DbAdapter.DebugLogger != null)
                DbAdapter.DebugLogger.LogInformation($"{GetType().Name} /Query/ {builder.SqlQuery} /Parameters/ {string.Join(", ", builder.Parameters.Select(x => x.Key + ":" + x.Value))}");
            List<T> entities = GetDbConnection().Query<T>(builder.SqlQuery, builder.Parameters, DbAdapter.DbTransaction).ToList();
            PostProcessEntities(entities);
            return entities;
        }
    }
}
