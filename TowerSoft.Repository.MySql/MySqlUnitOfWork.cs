using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace TowerSoft.Repository.MySql {
    public class MySqlUnitOfWork : IUnitOfWork, IDisposable {
        public string ConnectionString { get; }
        public IDbAdapter DbAdapter { get; }
        public IDbConnection DbConnection { get; }
        public IDbTransaction DbTransaction { get; private set; }

        public MySqlUnitOfWork(string connectionString) {
            ConnectionString = connectionString;
            DbConnection = new MySqlConnection(ConnectionString);
            DbAdapter = new MySqlDbAdapter();
        }

        public void BeginTransaction() {
            if (DbConnection.State != ConnectionState.Open)
                DbConnection.Open();
            DbTransaction = DbConnection.BeginTransaction();
        }

        public void CommitTransaction() {
            DbTransaction.Commit();
            DbTransaction.Dispose();
        }

        public void RollbackTransaction() {
            DbTransaction.Rollback();
            DbTransaction.Dispose();
        }

        public void Dispose() {
            if (DbTransaction != null)
            DbTransaction.Dispose();
            if (DbConnection != null)
            DbConnection.Dispose();
        }
    }
}
