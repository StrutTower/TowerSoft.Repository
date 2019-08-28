using System;
using System.Collections.Generic;
using System.Data;

namespace TowerSoft.Repository {
    public interface IDbAdapter {
        /// <summary>
        /// Specifies if the last insert ID query needs to be run separately from the insert statement.
        /// </summary>
        bool LastInsertIdInSeparateQuery { get; }

        /// <summary>
        /// Returns the ADO.NET IDbCommand for this database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IDbConnection CreateNewDbConnection(string connectionString);

        /// <summary>
        /// Returns the parameter placeholder for the supplied column. This is used in the SQL query.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        string GetParameterPlaceholder(string columnName);

        /// <summary>
        /// Returns the parameter name for the supplied column. This is used in the parameter dictionary.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns></returns>
        string GetParameterName(string columnName);

        /// <summary>
        /// Gets the SELECT statement for this table and column. Typically this is just TableName.ColumnName
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableName"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        string GetSelectColumnCast(Type type, string tableName, IMap map);

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
