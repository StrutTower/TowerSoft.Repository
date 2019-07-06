using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TowerSoft.Repository.MicrosoftSql {
    public class MicrosoftSqlUnitOfWork : IUnitOfWork {
        public string ConnectionString { get; }
        public IDbAdapter DbAdapter { get; }
        public IDbConnection DbConnection { get; }
        public IDbTransaction DbTransaction { get; private set; }

        public MicrosoftSqlUnitOfWork(string connectionString) {
            ConnectionString = connectionString;
            DbConnection = new SqlConnection(ConnectionString);
            DbAdapter = new MicrosoftSqlDbAdapter();
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
