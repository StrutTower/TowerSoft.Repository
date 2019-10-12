using TowerSoft.Repository;
using TowerSoft.Repository.Interfaces;
using TowerSoft.Repository.MySql;

namespace TestingLib.Repository {
    public class UnitOfWork : IRepositoryUnitOfWork {
        public UnitOfWork(string connectionString) {
            DbAdapter = new MySqlDbAdapter(connectionString);
        }

        public IDbAdapter DbAdapter { get; private set; }

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

        public static UnitOfWork CreateNew() {
            string connectionString = "server=towerserver;uid=unittests;password=$1letmein;database=unittests";
            return new UnitOfWork(connectionString);
        }
    }
}
