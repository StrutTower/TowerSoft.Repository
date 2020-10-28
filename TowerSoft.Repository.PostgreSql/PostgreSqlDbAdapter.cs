using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TowerSoft.Repository.PostgreSql {
    public class PostgreSqlDbAdapter : IDbAdapter {
        public PostgreSqlDbAdapter(string connectionString) {
            ConnectionString = connectionString;
            DbConnection = new NpgsqlConnection(ConnectionString);
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
            return new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        public string GetLastInsertIdStatement() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        public bool LastInsertIdInSeparateQuery => false;

        /// <summary>
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns></returns>
        public string GetParameterPlaceholder(string columnName) {
            return $"@{columnName}";
        }

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns></returns>
        public string GetParameterName(string columnName) {
            return $"@{columnName}";
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
            return $"{tableName}.{map.ColumnName} {map.PropertyName}";
        }
    }
}
