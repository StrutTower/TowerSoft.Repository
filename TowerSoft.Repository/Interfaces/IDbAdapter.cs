using System;
using System.Data;

namespace TowerSoft.Repository {
    public interface IDbAdapter : IDisposable {
        #region Unit of Work
        /// <summary>
        /// Connection string used for the database connection
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// ADO.NET DbConnection for the current database
        /// </summary>
        IDbConnection DbConnection { get; }

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

        bool IsDisposed { get; }
        #endregion

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IDbConnection CreateNewDbConnection(string connectionString);

        /// <summary>
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string GetParameterPlaceholder(string columnName);

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns></returns>
        string GetParameterName(string columnName);

        /// <summary>
        /// Gets the SELECT statement for this table and column.
        /// Typically this is just TableName.ColumnName but some databases require casting the column to s certain datatype
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableName"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        string GetSelectColumnCast(Type type, string tableName, IMap map);

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        string GetLastInsertIdStatement();

        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        bool LastInsertIdInSeparateQuery { get; }
    }
}
