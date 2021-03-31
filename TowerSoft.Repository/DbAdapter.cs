using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TowerSoft.Repository {
    /// <summary>
    /// Abstract DbAdapter. Use the DbAdapter for your specific database
    /// </summary>
    public abstract class DbAdapter {
        /// <summary>
        /// Abstract DbAdapter. Use the DbAdapter for your specific database
        /// </summary>
        /// <param name="connectionString">Connection string to the database</param>
        public DbAdapter(string connectionString) {
            ConnectionString = connectionString;
            DbConnection = CreateNewDbConnection(ConnectionString);
        }

        /// <summary>
        /// Connection string used for the database connection
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Store if the DbConnection and DbTransaction has been disposed
        /// </summary>
        public bool IsDisposed { get; protected set; }

        /// <summary>
        /// ADO.NET DbConnection for the current database
        /// </summary>
        public IDbConnection DbConnection { get; set; }

        /// <summary>
        /// ADO.NET DbTransaction for the current database
        /// </summary>
        public IDbTransaction DbTransaction { get; protected set; }

        /// <summary>
        /// ILogger used for outputing debugging info
        /// </summary>
        public ILogger DebugLogger { get; private set; }

        /// <summary>
        /// ILogger can be supplied to enable outputing debug info from the ORM
        /// </summary>
        /// <param name="logger"></param>
        public void AddLogger(ILogger logger) {
            DebugLogger = logger;
        }

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

        /// <summary>
        /// Returns the ADO.NET IDbCommand for the database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public abstract IDbConnection CreateNewDbConnection(string connectionString);

        /// <summary>
        /// Runs configuration settings on the DbConnection
        /// </summary>
        public virtual void ConfigureDbConnection() { }

        /// <summary>
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="parameterIndex">Index of the parameter for the query query</param>
        /// <returns></returns>
        public virtual string GetParameterPlaceholder(string columnName, int parameterIndex) {
            return $"@{columnName}{parameterIndex}";
        }

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="parameterIndex">Index of the parameter for the query query</param>
        /// <returns></returns>
        public virtual string GetParameterName(string columnName, int parameterIndex) {
            return $"@{columnName}{parameterIndex}";
        }

        /// <summary>
        /// Gets the SELECT statement for this table and column.
        /// Typically this is just TableName.ColumnName but some databases require casting the column to certain datatype
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="tableName">Name of the database table</param>
        /// <param name="map">Map for the property</param>
        /// <returns></returns>
        public virtual string GetSelectColumnCast(Type type, string tableName, IMap map) {
            return $"{tableName}.{map.ColumnName} {map.PropertyName}";
        }
    }
}
