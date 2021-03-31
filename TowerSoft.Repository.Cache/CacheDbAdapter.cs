using InterSystems.Data.CacheClient;
using System;
using System.Data;
using System.Reflection;

namespace TowerSoft.Repository.Cache {
    /// <summary>
    /// DbAdapter for Intersystem's Caché Server
    /// </summary>
    public class CacheDbAdapter : DbAdapter, IDbAdapter {
        /// <summary>
        /// Create a new DbAdapter for Intersystem's Caché Server
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public CacheDbAdapter(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public override IDbConnection CreateNewDbConnection(string connectionString) {
            var con = new CacheConnection(connectionString);
            con.Open();
            con.SetQueryRuntimeMode(InterSystems.Data.CacheTypes.QueryRuntimeMode.LOGICAL);
            return con;
        }

        /// <summary>
        /// Forces the DbConnection to use logical mode
        /// </summary>
        public override void ConfigureDbConnection() {
            ((CacheConnection)DbConnection).SetQueryRuntimeMode(InterSystems.Data.CacheTypes.QueryRuntimeMode.LOGICAL);
        }

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        public string GetLastInsertIdStatement() {
            return "SELECT LAST_IDENTITY()";
        }

        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        public bool LastInsertIdInSeparateQuery => true;

        /// <summary>
        /// Specifies if the database allows multiple entities to be inserted in a single statement.
        /// </summary>
        public bool ListInsertSupported => false;

        /// <summary>
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="parameterIndex">Index of the parameter for the query query</param>
        /// <returns></returns>
        public override string GetParameterPlaceholder(string columnName, int parameterIndex) {
            return "?";
        }

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="parameterIndex">Index of the parameter for the query query</param>
        /// <returns></returns>
        public override string GetParameterName(string columnName, int parameterIndex) {
            return $"?{columnName}{parameterIndex}";
        }

        /// <summary>
        /// Gets the SELECT statement for this table and column.
        /// Typically this is just TableName.ColumnName but some databases require casting the column to certain datatype
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="tableName">Name of the database table</param>
        /// <param name="map">Map for the property</param>
        /// <returns></returns>
        public override string GetSelectColumnCast(Type type, string tableName, IMap map) {
            PropertyInfo pi = type.GetProperty(map.PropertyName);

            Type propertyType = pi.PropertyType;

            // Get underlying types for nullable objects
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                propertyType = pi.PropertyType.GetGenericArguments()[0];
            }

            string cast = "";

            if (Equals(propertyType, typeof(short))) {
                cast = "SMALLINT";
            } else if (Equals(propertyType, typeof(int))
                  || Equals(propertyType, typeof(int))
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
}
