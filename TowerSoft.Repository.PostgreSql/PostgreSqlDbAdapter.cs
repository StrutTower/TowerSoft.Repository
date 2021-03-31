using Npgsql;
using System;
using System.Data;

namespace TowerSoft.Repository.PostgreSql {
    /// <summary>
    /// DbAdapter for PostgreSQL
    /// </summary>
    public class PostgreSqlDbAdapter : DbAdapter, IDbAdapter {
        /// <summary>
        /// Create a new DbAdapter for PostgreSQL
        /// </summary>
        /// <param name="connectionString">Connection string to the database</param>
        public PostgreSqlDbAdapter(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public override IDbConnection CreateNewDbConnection(string connectionString) {
            return new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        public string GetLastInsertIdStatement() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        public bool LastInsertIdInSeparateQuery => false;

        /// <summary>
        /// Specifies if the database allows multiple entities to be inserted in a single statement.
        /// </summary>
        public bool ListInsertSupported => false;
    }
}
