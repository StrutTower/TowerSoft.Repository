using System;

namespace TowerSoft.Repository.Attributes {
    /// <summary>
    /// Set the database table name for this domain object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute {
        /// <summary>
        /// Set the database table name for this domain object
        /// </summary>
        /// <param name="name">Name of the database table</param>
        public TableNameAttribute(string name) {
            Name = name;
        }

        /// <summary>
        /// Name of the database table
        /// </summary>
        public string Name { get; }
    }

}
