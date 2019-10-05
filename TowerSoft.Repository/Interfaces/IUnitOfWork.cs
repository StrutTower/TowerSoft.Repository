using System;

namespace TowerSoft.Repository.Interfaces {
    public interface IUnitOfWork : IDisposable {
        IDbAdapter DbAdapter { get; set; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}