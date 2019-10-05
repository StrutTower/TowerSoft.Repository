using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.MySql;

namespace TestingLib.Repository {
    public class UnitOfWork : IUnitOfWork {
        public UnitOfWork(string connectionString) {
            DbAdapter = new MySqlDbAdapter(connectionString);
        }

        public IDbAdapter DbAdapter { get; set; }

        public void BeginTransaction() {
            DbAdapter.BeginTransaction();
        }

        public void CommitTransaction() {
            DbAdapter.CommitTransaction();
        }

        public void RollbackTransaction() {
            DbAdapter.RollbackTransaction();
        }

        public void Dispose() {
            DbAdapter.Dispose();
        }
    }
}
