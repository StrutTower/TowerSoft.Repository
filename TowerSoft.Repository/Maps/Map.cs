using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TowerSoft.Repository.Maps {
    /// <summary>
    /// Standard map used to map a property to a column
    /// </summary>
    public class Map : IMap {
        /// <summary>
        /// Name of the property of the C# object
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Name of the column in the database
        /// </summary>
        public string ColumnName { get; private set; }

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
        public Map(string propertyName, string columnName) {
            PropertyName = propertyName;
            ColumnName = columnName;
        }

        /// <summary>
        /// Get the value for this property from the supplied entity
        /// </summary>
        /// <param name="entity">Entity to retrieve the value from</param>
        /// <returns></returns>
        public object GetValue(object entity) {
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
