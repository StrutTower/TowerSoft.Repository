using System.Data;
using System.Data.SQLite;

namespace TowerSoft.Repository.SQLite {
    /// <summary>
    /// SQLite Database adapter
    /// </summary>
    /// <param name="connectionString">Database connection string</param>
    public class SQLiteDbAdapter(string connectionString) : DbAdapter(connectionString), IDbAdapter {

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public override IDbConnection CreateNewDbConnection(string connectionString) {
            return new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        public string GetLastInsertIdStatement() {
            return "SELECT last_insert_rowid();";
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
