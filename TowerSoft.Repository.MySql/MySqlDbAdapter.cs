using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TowerSoft.Repository.MySql {
    public class MySqlDbAdapter : IDbAdapter {
        public MySqlDbAdapter(string connectionString) {
            ConnectionString = connectionString;
        }

        #region Unit of Work
        public string ConnectionString { get; }

        public bool IsDisposed { get; private set; }

        public IDbConnection DbConnection { get; }

        public IDbTransaction DbTransaction { get; private set; }

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
            IsDisposed = true;
        }
        #endregion

        public IDbConnection CreateNewDbConnection(string connectionString) {
            return new MySqlConnection(connectionString);
        }

        public string GetLastInsertIdStatement() {
            return "SELECT LAST_INSERT_ID();";
        }

        public bool LastInsertIdInSeparateQuery => false;

        public string GetParameterPlaceholder(string columnName) {
            return $"@{columnName}";
        }

        public string GetParameterName(string columnName) {
            return $"@{columnName}";
        }

        public string GetSelectColumnCast(Type type, string tableName, IMap map) {
            return $"{tableName}.{map.ColumnName} {map.PropertyName}";
        }
    }
}
