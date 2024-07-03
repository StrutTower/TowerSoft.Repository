using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Assigns this property to a different column name in the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnMapAttribute : Attribute {
        /// <summary>
        /// Name of the column in the database
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Assigns this property to a different column name in the database.
        /// </summary>
        /// <param name="columnName">Name of the column in the database</param>
        public ColumnMapAttribute(string columnName) {
            ColumnName = columnName;
        }
    }
}
