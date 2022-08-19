using InterSystems.Data.IRISClient;
using System;
using System.Data;
using System.Reflection;

namespace TowerSoft.Repository.Iris {
    public class IrisDbAdapter : DbAdapter, IDbAdapter {
        /// <summary>
        /// Create a new DbAdapter for Intersystem's Iris Server
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public IrisDbAdapter(string connectionString) : base(connectionString) { }

        public override IDbConnection CreateNewDbConnection(string connectionString) {
            return new IRISConnection(connectionString);
        }

        public string GetLastInsertIdStatement() {
            return "SELECT LAST_IDENTITY()";
        }

        public bool LastInsertIdInSeparateQuery => true;

        public bool ListInsertSupported => false;

        public override string GetParameterPlaceholder(string columnName, int parameterIndex) {
            return "?";
        }

        public override string GetParameterName(string columnName, int parameterIndex) {
            return $"{columnName}{parameterIndex}";
        }

        public override string GetSelectColumnCast(Type type, string tableName, IMap map) {
            PropertyInfo pi = type.GetProperty(map.PropertyName);

            Type propertyType = pi.PropertyType;

            // Get underlying types for nullable objects
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                propertyType = pi.PropertyType.GetGenericArguments()[0];
            }

            if (!string.IsNullOrWhiteSpace(map.FunctionName)) {
                return $"{map.FunctionName}({tableName}.{map.ColumnName}) {map.PropertyName}";
            } else {
                string cast = string.Empty;

                if (Equals(propertyType, typeof(short))) {
                    cast = "SMALLINT";
                } else if (Equals(propertyType, typeof(int))
                      || Equals(propertyType.BaseType, typeof(Enum))) {
                    cast = "INT";
                } else if (Equals(propertyType, typeof(long))) {
                    cast = "BIGINT";
                } else if (Equals(propertyType, typeof(bool))) {
                    cast = "INT";
                } else if (Equals(propertyType, typeof(DateTime))) {
                    cast = "DATETIME";
                }

                if (string.IsNullOrEmpty(cast)) {
                    return $"{tableName}.{map.ColumnName} {map.PropertyName}";
                } else {
                    return $"CAST({tableName}.{map.ColumnName} AS {cast}) {map.PropertyName}";
                }
            }
        }

        /// <summary>
        /// Returns a limit and offset statement for the current database type. LIMIT and OFFSET are not supported in Iris
        /// </summary>
        /// <param name="limit">How many rows to return</param>
        /// <param name="offset">How many rows to skip</param>
        /// <param name="query">Current query builder</param>
        /// <returns></returns>
        public override string GetLimitOffsetStatement(int? limit, int? offset, QueryBuilder query) {
            throw new NotSupportedException("LIMIT and OFFSET statements are not supported in Intersystems Iris");
        }
    }
}
