using System;
using System.Data;

namespace TowerSoft.Repository {
    /// <summary>
    /// Unit of Work class
    /// </summary>
    public interface IUnitOfWork : IDisposable {
        /// <summary>
        /// Connection string used for the database connection
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// DBAdapter for the current database
        /// </summary>
        IDbAdapter DbAdapter { get; }

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
    }
}