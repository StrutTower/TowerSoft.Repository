﻿using System;
using TowerSoft.Repository.Utilities;

namespace TowerSoft.Repository.Maps {
    /// <summary>
    /// Only use on Intersystems Caché databases. Uses the Fileman logical date format for this column with connecting to the database.
    /// </summary>
    public class CacheFilemanDateMap : Map {
        /// <summary>
        /// Initialize a new IDMap where the property name and column name are the same.
        /// </summary>
        /// <param name="propertyAndColumnName">Name of the property and column</param>
        public CacheFilemanDateMap(string propertyAndColumnName) : base(propertyAndColumnName) { }

        /// <summary>
        /// Initialize a new IDMap where the column and property names are different
        /// </summary>
        /// <param name="propertyName">Name of the property on the C# object</param>
        /// <param name="columnName">Name of the column in the database</param>
        /// <param name="functionName">
        /// Name of the function used to display the value.
        /// Currently only used by Cache and Iris databases.
        /// Typical options are %INTERNAL and %EXTERNAL.
        /// </param>
        public CacheFilemanDateMap(string propertyName, string columnName, string functionName = null) : base(propertyName, columnName, functionName) { }

        /// <summary>
        /// Gets the value from the supplied entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override object GetValue(object entity) {
            object value = base.GetValue(entity);
            if (value is DateTime date) {
                return InternalCacheUtilities.DateTimeToLogicalDate(date);
            }
            return value;
        }
    }
}
