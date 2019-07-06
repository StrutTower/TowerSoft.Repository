using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace TowerSoft.Repository.SQLite {
    public class SQLiteUnitOfWork : IUnitOfWork {
        public string ConnectionString { get; }

        public IDbAdapter DbAdapter { get; }

        public IDbConnection DbConnection { get; }

        public IDbTransaction DbTransaction { get; private set; }

        public SQLiteUnitOfWork(string connectionString) {
            ConnectionString = connectionString;
            DbConnection = new SQLiteConnection(ConnectionString);
            DbAdapter = new SQLiteDbAdapter();
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
