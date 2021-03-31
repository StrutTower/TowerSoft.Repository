using MySql.Data.MySqlClient;
using System.Data;

namespace TowerSoft.Repository.MySql {
    /// <summary>
    /// DbAdapter for MySQL
    /// </summary>
    public class MySqlDbAdapter : DbAdapter, IDbAdapter {
        /// <summary>
        /// Create a new DbAdapter for MySQL
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public MySqlDbAdapter(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public override IDbConnection CreateNewDbConnection(string connectionString) {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        public string GetLastInsertIdStatement() {
            return "SELECT LAST_INSERT_ID();";
        }

        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        public bool LastInsertIdInSeparateQuery => false;

        /// <summary>
        /// Specifies if the database allows multiple entities to be inserted in a single statement.
        /// </summary>
        public bool ListInsertSupported => true;
    }
}
