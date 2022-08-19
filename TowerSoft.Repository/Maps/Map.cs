using System.Reflection;

namespace TowerSoft.Repository.Maps {
    /// <summary>
    /// Standard map used to map a property to a column
    /// </summary>
    public class Map : IMap {
        /// <summary>
        /// Name of the property of the C# object
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Name of the column in the database
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// Name of the function used to display the value
        /// </summary>
        public string FunctionName { get; }

        /// <summary>
        /// Initialize a new Map where the property name and column name are the same.
        /// </summary>
        /// <param name="propertyAndColumnName">Name of the property and column</param>
        public Map(string propertyAndColumnName) {
            PropertyName = propertyAndColumnName;
            ColumnName = propertyAndColumnName;
        }

        /// <summary>
        /// Initialize a new Map where the column and property names are different
        /// </summary>
        /// <param name="propertyName">Name of the property on the C# object</param>
        /// <param name="columnName">Name of the column in the database</param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        public Map(string propertyName, string columnName, string functionName = null) {
            PropertyName = propertyName;
            ColumnName = columnName;
            FunctionName = functionName;
        }

        /// <summary>
        /// Get the value for this property from the supplied entity
        /// </summary>
        /// <param name="entity">Entity to retrieve the value from</param>
        /// <returns></returns>
        public virtual object GetValue(object entity) {
            PropertyInfo prop = entity.GetType().GetProperty(PropertyName);
            return prop.GetValue(entity);
        }

        /// <summary>
        /// Set the value of the property on the supplied entity
        /// </summary>
        /// <param name="entity">Entity to set the value on</param>
        /// <param name="value">Value to set</param>
        public void SetValue(object entity, object value) {
            PropertyInfo prop = entity.GetType().GetProperty(PropertyName);
            prop.SetValue(entity, value);
        }
    }
}
