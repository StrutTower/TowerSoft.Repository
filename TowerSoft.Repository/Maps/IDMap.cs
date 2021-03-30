﻿namespace TowerSoft.Repository.Maps {
    /// <summary>
    /// Primary key map used to map a property to a column that is a primary key but does not autoincrement
    /// </summary>
    public class IDMap : Map {
        /// <summary>
        /// Initialize a new IDMap where the property name and column name are the same.
        /// </summary>
        /// <param name="propertyAndColumnName">Name of the property and column</param>
        public IDMap(string propertyAndColumnName) : base(propertyAndColumnName) { }

        /// <summary>
        /// Initialize a new IDMap where the column and property names are different
        /// </summary>
        /// <param name="propertyName">Name of the property on the C# object</param>
        /// <param name="columnName">Name of the column in the database</param>
        public IDMap(string propertyName, string columnName) : base(propertyName, columnName) { }
    }
}
