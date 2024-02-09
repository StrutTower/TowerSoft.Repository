using System;

namespace TowerSoft.Repository {
    /// <summary>
    /// Base class for standard UnitOfWork classes.
    /// </summary>
    public abstract class UnitOfWorkBase : IDisposable {
        /// <summary>
        /// The DbAdapter for the current database connection.
        /// </summary>
        public IDbAdapter DbAdapter { get; protected set; }

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        public virtual void BeginTransaction() => DbAdapter.BeginTransaction();

        /// <summary>
        /// Commits the current database transaction.
        /// </summary>
        public virtual void CommitTransaction() => DbAdapter.CommitTransaction();

        /// <summary>
        /// Rolls back the current database transaction.
        /// </summary>
        public virtual void RollbackTransaction() => DbAdapter.RollbackTransaction();

        /// <summary>
        /// Disposes of the database connection.
        /// </summary>
        public virtual void Dispose() => DbAdapter.Dispose();
    }
}
