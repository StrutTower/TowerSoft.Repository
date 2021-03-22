using System;
using System.Collections.Generic;
using System.Text;
using TowerSoft.Repository;

namespace TowerSoft.RepositoryTests.Interfaces {
    public interface IUnitOfWork {
        IDbAdapter DbAdapter { get; }

        void BeginTransaction();
        void CommitTransaction();
        void Dispose();
        TRepo GetRepo<TRepo>();
        void RollbackTransaction();
    }
}
