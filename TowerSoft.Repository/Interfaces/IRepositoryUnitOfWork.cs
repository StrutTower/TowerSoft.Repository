using System;

namespace TowerSoft.Repository.Interfaces {
    /// <summary>
    /// Interface used to help create a UnitOfWork class in your project
    /// </summary>
    public interface IRepositoryUnitOfWork : IDisposable {
        /// <summary>
        /// Stores the DbAdapter. The DbAdapter is normally initialized in the UnitOfWork's constructor
        /// </summary>
        IDbAdapter DbAdapter { get; }

        /// <summary>
        /// Wrapper method for IDbAdapter.BeginTransaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Wrapper method for IDbAdapter.CommitTransaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Wrapper method for IDbAdapter.RollbackTransaction
        /// </summary>
        void RollbackTransaction();
    }
}