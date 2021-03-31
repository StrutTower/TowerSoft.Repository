using System.Data;
using System.Data.SqlClient;

namespace TowerSoft.Repository.MicrosoftSql {
    /// <summary>
    /// DbAdapter for Microsoft's SQL Server
    /// </summary>
    public class MicrosoftSqlDbAdapter : DbAdapter, IDbAdapter {
        /// <summary>
        /// Create a new DbAdapter for Microsoft's SQL Server
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public MicrosoftSqlDbAdapter(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public override IDbConnection CreateNewDbConnection(string connectionString) {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        public string GetLastInsertIdStatement() {
            return "SELECT SCOPE_IDENTITY();";
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
