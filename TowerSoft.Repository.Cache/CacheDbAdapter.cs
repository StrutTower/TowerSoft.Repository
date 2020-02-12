using InterSystems.Data.CacheClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace TowerSoft.Repository.Cache {
    /// <summary>
    /// DbAdapter for Intersystem's Cache Server
    /// </summary>
    public class CacheDbAdapter : IDbAdapter {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public CacheDbAdapter(string connectionString) {
            ConnectionString = connectionString;
            DbConnection = new CacheConnection(ConnectionString);
        }

        #region Unit of Work
        /// <summary>
        /// Connection string used for the database connection
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Store if the DbConnection and DbTransaction has been disposed
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// ADO.NET DbConnection for the current database
        /// </summary>
        public IDbConnection DbConnection { get; set; }

        /// <summary>
        /// ADO.NET DbTransaction for the current database
        /// </summary>
        public IDbTransaction DbTransaction { get; private set; }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        public void BeginTransaction() {
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
            DbTransaction = DbConnection.BeginTransaction();
        }

        /// <summary>
        /// Commit the changes during the transaction to the database
        /// </summary>
        public void CommitTransaction() {
            DbTransaction.Commit();
            DbTransaction.Dispose();
        }

        /// <summary>
        /// Rolls back the changes to the database that were made during the transaction
        /// </summary>
        public void RollbackTransaction() {
            DbTransaction.Rollback();
            DbTransaction.Dispose();
        }

        /// <summary>
        /// Disposes the DbConnection and DbTransaction
        /// </summary>
        public void Dispose() {
            if (DbTransaction != null)
                DbTransaction.Dispose();
            if (DbConnection != null)
                DbConnection.Dispose();
            IsDisposed = true;
        }
        #endregion

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public IDbConnection CreateNewDbConnection(string connectionString) {
            return new CacheConnection(connectionString);
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
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns></returns>
        public string GetParameterPlaceholder(string columnName) {
            return "?";
        }

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns></returns>
        public string GetParameterName(string columnName) {
            return $"?{columnName}";
        }

        /// <summary>
        /// Gets the SELECT statement for this table and column.
        /// Typically this is just TableName.ColumnName but some databases require casting the column to certain datatype
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="tableName">Name of the database table</param>
        /// <param name="map">Map for the property</param>
        /// <returns></returns>
        public string GetSelectColumnCast(Type type, string tableName, IMap map) {
            PropertyInfo pi = type.GetProperty(map.PropertyName);

            Type propertyType = pi.PropertyType;

            // Get underlying types for nullable objects
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                propertyType = pi.PropertyType.GetGenericArguments()[0];
            }

            string cast = "";

            if (Type.Equals(propertyType, typeof(short))) {
                cast = "SMALLINT";
            } else if (Type.Equals(propertyType, typeof(int))
                  || Type.Equals(propertyType, typeof(int))
                  || Type.Equals(propertyType.BaseType, typeof(Enum))) {
                cast = "INT";
            } else if (Type.Equals(propertyType, typeof(long))) {
                cast = "BIGINT";
            } else if (Type.Equals(propertyType, typeof(bool))) {
                cast = "INT";
            } else if (Type.Equals(propertyType, typeof(DateTime))) {
                cast = "DATETIME";
            }

            if (string.IsNullOrEmpty(cast)) {
                return tableName + "." + map.ColumnName;
            } else {
                return "CAST(" + tableName + "." + map.ColumnName + " AS " + cast + ") " + map.ColumnName;
            }
        }
    }
}
