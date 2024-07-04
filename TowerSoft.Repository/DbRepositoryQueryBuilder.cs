using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TowerSoft.Repository.Builders;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.Maps;

namespace TowerSoft.Repository {
    public partial class DbRepository<T> : IDbRepository {
        /// <summary>
        /// Returns an instance of FluentQueryBuilder
        /// </summary>
        protected FluentQueryBuilder<T> Query {
            get {
                return new FluentQueryBuilder<T>(Mappings);
            }
        }
        /// <summary>
        /// Returns an instance of FluentQueryBuilder
        /// </summary>
        [Obsolete("Use " + nameof(Query) + " instead.")]
        protected FluentQueryBuilder<T> QueryBuilder {
            get {
                return new FluentQueryBuilder<T>(Mappings);
            }
        }

        /// <summary>
        /// Returns the basic SELECT and FROM parts of a SQL statement for the database table
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBasicSelectText() {
            List<string> columns = new List<string>();
            foreach (Map map in Mappings.AllMaps) {
                columns.Add(DbAdapter.GetSelectColumnCast(typeof(T), TableName, map));
            }

            string query = $"SELECT {string.Join(",", columns)} FROM {TableName} ";
            return query;
        }

        /// <summary>
        /// Returns the basic SELECT and FROM parts of a SQL statement for the database table including any extra columns that are supplied.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBasicSelectText(IEnumerable<string> customColumns) {
            List<string> columns = new List<string>();
            foreach (Map map in Mappings.AllMaps) {
                columns.Add(DbAdapter.GetSelectColumnCast(typeof(T), TableName, map));
            }

            columns.AddRange(customColumns);

            string query = $"SELECT {string.Join(",", columns)} FROM {TableName} ";
            return query;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected FluentQueryBuilder<T> WhereEqual<TProperty>(Expression<Func<T, TProperty>> propertyExpression, object value) {
            FluentQueryBuilder<T> builder = new FluentQueryBuilder<T>(Mappings);
            builder.AddWhereCondition(propertyExpression, Comparison.Equals, value);
            return builder;
        }

        /// <summary>
        /// Generate a query with the specified where statement.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression">Property to query</param>
        /// <param name="queryComparison">Comparison type</param>
        /// <param name="value">Value to compare to</param>
        /// <returns></returns>
        protected FluentQueryBuilder<T> Where<TProperty>(Expression<Func<T, TProperty>> propertyExpression, Comparison queryComparison, object value) {
            FluentQueryBuilder<T> builder = new FluentQueryBuilder<T>(Mappings);
            builder.AddWhereCondition(propertyExpression, queryComparison, value);
            return builder;
        }

        /// <summary>
        /// Returns a new instance of QueryBuilder for this repository.
        /// </summary>
        /// <returns></returns>
        protected QueryBuilder GetQueryBuilder() {
            return new QueryBuilder(TableName, Mappings.AllMaps, DbAdapter, typeof(T));
        }

        /// <summary>
        /// Returns a new instance of QueryBuilder for this repository including any custom columns supplied.
        /// </summary>
        /// <param name="customColumns">Custom select columns to include in the query.</param>
        /// <returns></returns>
        protected QueryBuilder GetQueryBuilder(IEnumerable<string> customColumns) {
            return new QueryBuilder(TableName, Mappings.AllMaps, DbAdapter, typeof(T), customColumns);
        }
    }
}
