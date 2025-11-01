using System.Data;
using Microsoft.Data.SqlClient;

namespace TowerSoft.Repository.MicrosoftSql {
    /// <summary>
    /// DbAdapter for Microsoft's SQL Server
    /// </summary>
    /// <param name="connectionString">Database connection string</param>
    public class MicrosoftSqlDbAdapter(string connectionString) : DbAdapter(connectionString), IDbAdapter {

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

        /// <summary>
        /// Returns a limit and offset statement for the current database type
        /// </summary>
        /// <param name="limit">How many rows to return</param>
        /// <param name="offset">How many rows to skip</param>
        /// <param name="query">Current query builder</param>
        /// <returns></returns>
        public override string GetLimitOffsetStatement(int? limit, int? offset, QueryBuilder query) {
            if (!query.SqlQuery.Contains("ORDER BY", StringComparison.CurrentCultureIgnoreCase))
                throw new System.Exception("Microsoft's SQL server requires and ORDER BY when using LIMIT/FETCH.");

            if (limit.HasValue && offset.HasValue)
                return $"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY ";

            if (limit.HasValue)
                return $"OFFSET 0 ROWS FETCH NEXT {limit} ROWS ONLY ";

            return "";
        }
    }
}
