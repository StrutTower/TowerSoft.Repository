using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace TowerSoft.Repository {
    /// <summary>
    /// Interface for the DbAdapter classes that define the difference between different databases
    /// </summary>
    public interface IDbAdapter : IDisposable {
        #region Unit of Work
        /// <summary>
        /// Connection string used for the database connection
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Store if the DbConnection and DbTransaction has been disposed
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// ADO.NET DbConnection for the current database
        /// </summary>
        IDbConnection DbConnection { get; set; }

        /// <summary>
        /// ADO.NET DbTransaction for the current database
        /// </summary>
        IDbTransaction DbTransaction { get; }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit the changes during the transaction to the database
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the changes to the database that were made during the transaction
        /// </summary>
        void RollbackTransaction();
        #endregion

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        IDbConnection CreateNewDbConnection(string connectionString);

        /// <summary>
        /// Runs configuration settings on the DbConnection
        /// </summary>
        void ConfigureDbConnection();

        /// <summary>
        /// Returns a limit and offset statement for the current database type
        /// </summary>
        /// <param name="limit">How many rows to return</param>
        /// <param name="offset">How many rows to skip</param>
        /// <param name="query">Current query builder</param>
        /// <returns></returns>
        string GetLimitOffsetStatement(int? limit, int? offset, QueryBuilder query);

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        string GetLastInsertIdStatement();

        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        bool LastInsertIdInSeparateQuery { get; }

        /// <summary>
        /// Specifies if the database allows multiple entities to be inserted in a single statement.
        /// </summary>
        bool ListInsertSupported { get; }

        /// <summary>
        /// ILogger used for outputting debugging info
        /// </summary>
        ILogger DebugLogger { get; }

        /// <summary>
        /// ILogger can be supplied to enable outputting debug info from the ORM
        /// </summary>
        /// <param name="logger">ILogger</param>
        void AddLogger(ILogger logger);

        /// <summary>
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="parameterIndex">The index of the parameter in the query</param>
        /// <returns></returns>
        string GetParameterPlaceholder(string columnName, int parameterIndex);

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="parameterIndex">The index of the parameter in the query</param>
        /// <returns></returns>
        string GetParameterName(string columnName, int parameterIndex);

        /// <summary>
        /// Gets the SELECT statement for this table and column.
        /// Typically this is just TableName.ColumnName but some databases require casting the column to certain datatype
        /// </summary>
        /// <param name="type">Object type</param>
        /// <param name="tableName">Name of the database table</param>
        /// <param name="map">Map for the property</param>
        /// <returns></returns>
        string GetSelectColumnCast(Type type, string tableName, IMap map);
    }
}
