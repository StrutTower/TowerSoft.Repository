using System;

namespace TowerSoft.Repository.Interfaces {
    public interface IRepositoryUnitOfWork : IDisposable {
        IDbAdapter DbAdapter { get; set; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}