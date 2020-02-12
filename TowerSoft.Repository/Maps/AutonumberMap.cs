namespace TowerSoft.Repository.Maps {
    /// <summary>
    /// Autonumber map used to map a property to a column that is set to autoincrement
    /// </summary>
    public class AutonumberMap : Map {
        /// <summary>
        /// Initialize a new autonumber map where the property name and column name are the same.
        /// </summary>
        /// <param name="propertyAndColumnName">Name of the property and column</param>
        public AutonumberMap(string propertyAndColumnName) : base(propertyAndColumnName) { }

        /// <summary>
        /// Initialize a new autonumber map where the column and property names are different
        /// </summary>
        /// <param name="propertyName">Name of the property on the C# object</param>
        /// <param name="columnName">Name of the column in the database</param>
        public AutonumberMap(string propertyName, string columnName) : base(propertyName, columnName) { }
    }
}
