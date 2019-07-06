using System.Collections.Generic;
using System.Data;

namespace TowerSoft.Repository {
    public interface IDbAdapter {
        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IDbConnection CreateNewDbConnection(string connectionString);

        /// <summary>
        /// Returns the parameter for the supplied column
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string GetParameterPlaceholder(string columnName);

        /// <summary>
        /// Gets the SELECT statement for this table and column. Typically this is just TableName.ColumnName
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string GetSelectColumnCast(string tableName, string columnName);

        /// <summary>
        /// SQL Statement to retrieve the last inserted ID for this database.
        /// </summary>
        /// <returns></returns>
        string GetLastInsertIdStatement();

        /// <summary>
        /// SQL statement to limit the number of results for this database.
        /// </summary>
        /// <returns></returns>
        string GetLimitStatement();

        /// <summary>
        /// SQL statement to offset the results by a number of rows for this database
        /// </summary>
        /// <returns></returns>
        string GetOffsetStatement();
    }
}
