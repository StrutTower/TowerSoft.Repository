using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerSoft.Repository {
    //TODO Rename file
    /// <summary>
    /// Object to create complex SQL statements. Will rename in the future to avoid confusion with FluentQueryBuilder
    /// </summary>
    public class QueryBuilder {
        internal QueryBuilder(string tableName, IEnumerable<IMap> maps, IDbAdapter dbAdapter, Type domainType, IEnumerable<string> customColumns = null) {
            TableName = tableName;
            Parameters = new Dictionary<string, object>();

            List<string> columns = new List<string>();
            foreach (IMap map in maps) {
                columns.Add(dbAdapter.GetSelectColumnCast(domainType, tableName, map));
            }

            if (customColumns != null && customColumns.Any()) {
                columns.AddRange(customColumns);
            }

            SqlQuery = "SELECT " + string.Join(",", columns) + " FROM " + TableName + " ";
        }

        /// <summary>
        /// Table name to query
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// SQL statement
        /// </summary>
        public string SqlQuery { get; set; }

        /// <summary>
        /// List of parameters for the SQL statement
        /// </summary>
        public Dictionary<string, object> Parameters { get; private set; }

        /// <summary>
        /// Adds a new parameter
        /// </summary>
        /// <param name="parameterName">Name of the parameter as it appears in the SQL statement including the @ symbol for most databases.</param>
        /// <param name="value">Value of the parameter</param>
        public void AddParameter(string parameterName, object value) {
            Parameters.Add(parameterName, value);
        }
    }
}
